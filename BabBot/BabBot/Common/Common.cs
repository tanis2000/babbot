using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace BabBot.Common
{
    public class AppHelper
    {
        #region Const

        private const string APPNAME = "Wow.exe";
        private const string KEY = "InstallPath";
        private const string ROOT = @"SOFTWARE\Blizzard Entertainment\World of Warcraft";
        private const string WND_TITLE = "World of Warcraft";

        #endregion

        #region External Declarations

        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(int hWnd);

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
        public static void WaitForWowWindow()
        {
            int hWnd = 0;
            bool isVisible = false;
            while (hWnd == 0 && !isVisible)
            {
                hWnd = GetWowWindowHandle();
                if (hWnd != 0)
                {
                    isVisible = IsWindowVisible(hWnd);
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Dummy version to get the Wow window handle
        /// </summary>
        public static int GetWowWindowHandle()
        {
            return FindWindow(null, WND_TITLE);
        }

        #endregion
    }
}