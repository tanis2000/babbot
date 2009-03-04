using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32;

namespace BabBot.Common
{
    public class AppHelper
    {
        private const string APPNAME = "Wow.exe";
        private const string KEY = "InstallPath";
        private const string ROOT = @"SOFTWARE\Blizzard Entertainment\World of Warcraft";
        public const int TOKEN_DUPLICATE = 2;
        public const int TOKEN_IMPERSONATE = 0X00000004;
        public const int TOKEN_QUERY = 0X00000008;
        private const string WND_TITLE = "World of Warcraft";

        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(int hWnd);

        [DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurity]
        private static extern int OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

        [DllImport("kernel32", SetLastError = true), SuppressUnmanagedCodeSecurity]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL,
                                                 ref IntPtr DuplicateTokenHandle);


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
                    res = wowKey.GetValue(KEY) + APPNAME;
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
        /// Dummy version to get and check if the running wow process under the guest account
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
                        IntPtr hToken = IntPtr.Zero;
                        if (
                            OpenProcessToken(proc.Handle,
                                             TOKEN_QUERY | TOKEN_IMPERSONATE | TOKEN_DUPLICATE, ref hToken) != 0)
                        {
                            var newId = new WindowsIdentity(hToken);
                            try
                            {
                                const int SecurityImpersonation = 2;
                                IntPtr dupeTokenHandle = DupeToken(hToken, SecurityImpersonation);

                                if (IntPtr.Zero == dupeTokenHandle)
                                {
                                    string s = String.Format("Dup failed {0}, privilege not held",
                                                             Marshal.GetLastWin32Error());
                                    throw new Exception(s);
                                }
                                WindowsImpersonationContext impersonatedUser = newId.Impersonate();

                                IntPtr accountToken = WindowsIdentity.GetCurrent().Token;
                                // string accTk = accountToken.ToString();

                                string currUsr = WindowsIdentity.GetCurrent().Name;

                                if (currUsr.Contains(UserToCheck))
                                {
                                    res = proc;
                                }
                            }
                            finally
                            {
                                CloseHandle(hToken);
                            }
                        }
                        else
                        {
                            string sMsg = String.Format("OpenProcess Failed {0}, privilege not held",
                                                        Marshal.GetLastWin32Error());
                            throw new Exception(sMsg);
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

        private static IntPtr DupeToken(IntPtr token, int Level)
        {
            IntPtr dupeTokenHandle = IntPtr.Zero;
            bool retVal = DuplicateToken(token, Level, ref dupeTokenHandle);
            return dupeTokenHandle;
        }


        /// <summary>
        /// Dummy version to check and wait for Wow Process 
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
    }
}