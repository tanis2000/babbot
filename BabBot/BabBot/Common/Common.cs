/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BabBot.Common
{
    /// <summary>
    /// Application Helper Class 
    /// </summary>
    public class AppHelper
    {
        #region Const

        private const string APPNAME = "Wow.exe";
        public const string WOWERROR_APP_NAME = "WowError";
        private const string KEY = "InstallPath";
        private const string ROOT = @"SOFTWARE\Blizzard Entertainment\World of Warcraft";
        private const string WND_TITLE = "World of Warcraft";

        #endregion

        /// <summary>
        /// Process ID of currently running (or started) wow.exe
        /// </summary>
        private static uint wow_pid;

        /// <summary>
        /// Windows handle that has pid as owner process
        /// </summary>
        private static uint wow_hnd;

        private static IntPtr botHandle;

        #region External Declarations

        /// <summary>
        /// The FindWindow function retrieves a handle to the top-level window 
        /// whose class name and window name match the specified strings. 
        /// This function does not search child windows. 
        /// This function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="className">Class Name or null</param>
        /// <param name="windowText">Window Title (case sensitive)</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern uint FindWindow(string className, string windowText);

        /// <summary>
        /// API: Get if a window is visible from its handle 
        /// </summary>
        /// <returns>
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(uint hWnd);

        /// <summary>
        /// API: Returns the id of the thread and process id that created the target window
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpdwProcessId"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary>
        /// The EnumWindows function enumerates all top-level windows on the screen by 
        /// passing the handle to each window, in turn, to an application-defined 
        /// callback function. EnumWindows continues until the last top-level window is 
        /// enumerated or the callback function returns FALSE. 
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, int lParam);

        /// <summary>
        /// The EnumWindowsProc function is an application-defined callback function 
        /// used with the EnumWindows
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        #endregion

        #region External Hider.dll (only for test)

        [DllImport("Hider.dll", EntryPoint = "HideProcess", ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        private static extern void HideProcess(string FileName);

        [DllImport("Hider.dll", EntryPoint = "StopHook", ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        private static extern void StopHook();

        #endregion

        #region Hider dummy methods

        public static void StartHideProcess()
        {
            /*
                You can disable the usage of the hosting process by clearing the “Enable the Visual Studio hosting process” 
                check box in project properties on the Debug tab.  
                You may want to try this if you’re getting unexpected failures when calling some APIs during the VS debugging, 
                and are not getting them when running the application outside of the debugger.
            */
            HideProcess(Application.ExecutablePath);
        }

        public static void StopHideProcess()
        {
            StopHook();
        }

        #endregion

        #region Registry methods

        /// <summary>
        /// Get the installation path from the registry 
        /// </summary>
        /// <returns>
        /// If the function succeeds, the return value is 
        /// the installation path else is null.
        /// </returns>
        public static string GetWowInstallationPath()
        {
            string res;
            RegistryKey wowKey = null;

            try
            {
                wowKey = Registry.LocalMachine.CreateSubKey(ROOT);
                if (wowKey != null)
                {
                    res = wowKey.GetValue(KEY).ToString();
                    if (!string.IsNullOrEmpty(res))
                    {
                        res += APPNAME;
                    }
                }
                else
                {
                    res = null;
                }
            }
            catch
            {
                res = null;
            }
            finally
            {
                if (wowKey != null)
                {
                    wowKey.Close();
                }
            }

            return res;
        }

        #endregion

        #region RunAS methods

        /// <summary>
        /// Get if is running on Vista 
        /// </summary>
        /// <returns>
        /// true if application is running on Vista 
        /// false if application is running on different OS
        /// </returns>
        public static bool IsVista()
        {
            return (Environment.OSVersion.Version.Major >= 6);
        }

        /// <summary>
        /// Dummy version for CreateProcessWithLogonW 
        /// </summary>
        /// <returns>
        /// return Process if the function succeeds
        /// </returns>
        public static Process RunAs(string Username, string Password, string Domain, string AppName)
        {
            return Common.RunAs.StartProcess(Username, Domain, Password, AppName);
        }

        /// <summary>
        /// Dummy version to get and check if wow process is running under  guest account
        /// </summary>
        /// <returns>
        /// return Process if the function succeeds else null
        /// </returns>
        public static Process GetRunningWoWProcess(string UserToCheck)
        {
            Process[] procs = Process.GetProcessesByName("wow");
            Process res = null;

            foreach (Process proc in procs)
            {
                if (proc.MainWindowTitle.Contains(WND_TITLE))
                {
                    if (!string.IsNullOrEmpty(UserToCheck))
                    {
                        string user;
                        string siduser;

                        if (GetProcessInfoByPID(proc.Id, out user, out siduser) != string.Empty)
                        {
                            if (user.Contains(UserToCheck))
                            {
                                res = proc;
                            }
                            else
                            {
                                throw new Exception("Wow is running with a different user!");
                            }
                        }
                        else
                        {
                            throw new Exception("ExGetProcessInfoByPID Failed!");
                        }
                    }
                    else
                    {
                        res = proc;
                    }
                }
            }
            return res;
        }

        #endregion

        #region Dummy API Windows functions

        /// <summary>
        /// Get informations about the process
        /// </summary>
        /// <returns>
        public static string GetProcessInfoByPID(int PID, out string User, out string Domain)
        {
            User = String.Empty;
            Domain = String.Empty;
            string OwnerSID = String.Empty;
            // string processname = String.Empty;
            try
            {
                var sq = new ObjectQuery
                    ("Select * from Win32_Process Where ProcessID = '" + PID + "'");
                var searcher = new ManagementObjectSearcher(sq);
                if (searcher.Get().Count == 0)
                {
                    return OwnerSID;
                }
                foreach (ManagementObject oReturn in searcher.Get())
                {
                    var o = new String[2];
                    //Invoke the method and populate the o var with the user name and domain
                    oReturn.InvokeMethod("GetOwner", o);

                    // int pid = (int)oReturn["ProcessID"];
                    // processname = (string) oReturn["Name"];
                    // dr[2] = oReturn["Description"];
                    User = o[0];
                    if (User == null)
                    {
                        User = String.Empty;
                    }
                    Domain = o[1];
                    if (Domain == null)
                    {
                        Domain = String.Empty;
                    }
                    var sid = new String[1];
                    oReturn.InvokeMethod("GetOwnerSid", sid);
                    OwnerSID = sid[0];
                    return OwnerSID;
                }
            }
            catch
            {
                return OwnerSID;
            }
            return OwnerSID;
        }

        /// <summary>
        /// Dummy version to check and wait for Wow window init 
        /// </summary>
        public static int WaitForWowWindow(uint pid)
        {
            uint hWnd = 0;
            bool isVisible = false;
            while (hWnd == 0 && !isVisible)
            {
                if (hWnd == 0)
                    hWnd = GetWowWindowHandle(pid);

                if (hWnd != 0)
                    isVisible = IsWindowVisible(hWnd);

                Thread.Sleep(1000);
            }

            return (int) hWnd;
        }

        public static bool CheckWowWindow(IntPtr hwnd, IntPtr lParam)
        {
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            bool found = (pid == wow_pid);
            if (found)
                wow_hnd = (uint) hwnd;

            return !found;
        }

        /// <summary>
        /// Find windows handle of started wow.exe process
        /// Comment: FindWindow doesn't work if multiple wow.exe started
        /// </summary>
        public static uint GetWowWindowHandle(uint pid)
        {
            // Old version
            // return FindWindow(null, WND_TITLE);

            wow_pid = pid;
            wow_hnd = 0;

            // Not checking for returning value since wow_hnd set in callback function
            EnumWindows(new EnumWindowsProc(CheckWowWindow), 0);

            return wow_hnd;
        }

        public static uint FindPidWindowByTitle(string title)
        {
            uint pid = 0;
            uint hWnd = FindWindow(null, title);
            if (hWnd != 0)
                GetWindowThreadProcessId((IntPtr)hWnd, out pid);
            return pid;
        }

        public static IntPtr BotHandle
        {
            get { return botHandle; }
            set { botHandle = value; }
        }

        #endregion
    }
}