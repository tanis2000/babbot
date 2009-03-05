using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
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
        private const int TOKEN_QUERY = 0X00000008;
        private const string WND_TITLE = "World of Warcraft";

        #endregion

        #region External Declarations

        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(int hWnd);

        [DllImport("kernel32", SetLastError = true), SuppressUnmanagedCodeSecurity]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32")]
        private static extern bool OpenProcessToken(
            IntPtr ProcessHandle, // handle to process
            int DesiredAccess, // desired access to process
            ref IntPtr TokenHandle // handle to open access token
            );

        [DllImport("AdvAPI32.dll", EntryPoint = "ConvertStringSidToSid")]
        public static extern bool ConvertStringSIdToSId(string Text, ref IntPtr SId);

        [DllImport("advapi32", CharSet = CharSet.Auto)]
        private static extern bool ConvertSidToStringSid(
            IntPtr pSID,
            [In, Out, MarshalAs(UnmanagedType.LPTStr)] ref string pStringSid
            );

        [DllImport("advapi32", CharSet = CharSet.Auto)]
        private static extern bool GetTokenInformation(
            IntPtr hToken,
            TOKEN_INFORMATION_CLASS tokenInfoClass,
            IntPtr TokenInformation,
            int tokeInfoLength,
            ref int reqLength
            );

        [DllImport("AdvAPI32.dll", EntryPoint = "LookupAccountSid")]
        private static extern bool LookupAccountSId(string SystemName, IntPtr SId,
                                                    StringBuilder Name, ref int NameBytes, StringBuilder DomainName,
                                                    ref int DomainNameBytes,
                                                    ref int NameUse);

        [DllImport("Kernel32.dll")]
        private static extern IntPtr LocalFree(IntPtr hMem);

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

        #region Nested type: _SID_AND_ATTRIBUTES

        [StructLayout(LayoutKind.Sequential)]
        public struct _SID_AND_ATTRIBUTES
        {
            public IntPtr Sid;
            public int Attributes;
        }

        #endregion

        #region Nested type: TOKEN_INFORMATION_CLASS

        private enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId
        }

        #endregion

        #region Nested type: TOKEN_USER

        [StructLayout(LayoutKind.Sequential)]
        private struct TOKEN_USER
        {
            public _SID_AND_ATTRIBUTES User;
        }

        #endregion

        #region Privilege Token methods

        /// <summary>
        /// Collect User Info
        /// </summary>
        /// <param name="pToken">Process Handle</param>
        /// <param name="SID">User SID</param>
        private static bool DumpUserInfo(IntPtr pToken, out IntPtr SID)
        {
            int Access = TOKEN_QUERY;
            IntPtr procToken = IntPtr.Zero;
            bool ret = false;
            SID = IntPtr.Zero;
            try
            {
                if (OpenProcessToken(pToken, Access, ref procToken))
                {
                    ret = ProcessTokenToSid(procToken, out SID);
                    CloseHandle(procToken);
                }
                return ret;
            }
            catch
            {
                return false;
            }
        }

        private static bool ProcessTokenToSid(IntPtr token, out IntPtr SID)
        {
            TOKEN_USER tokUser;
            const int bufLength = 256;
            IntPtr tu = Marshal.AllocHGlobal(bufLength);
            SID = IntPtr.Zero;
            try
            {
                int cb = bufLength;
                bool ret = GetTokenInformation(token, TOKEN_INFORMATION_CLASS.TokenUser, tu, cb, ref cb);
                if (ret)
                {
                    tokUser = (TOKEN_USER) Marshal.PtrToStructure(tu, typeof (TOKEN_USER));
                    SID = tokUser.User.Sid;
                }
                return ret;
            }
            catch
            {
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(tu);
            }
        }

        private static string SIdToName(string SIdText)
        {
            bool Result = false;
            IntPtr SId = IntPtr.Zero;
            var Name = new StringBuilder(10000);
            int NameBytes = 100;
            var DomainName = new StringBuilder(10000);
            int DomainNameBytes = 100;
            int NameUse = 0;
            Result = ConvertStringSIdToSId(SIdText, ref SId);
            if (Result)
            {
                Result = LookupAccountSId(null, SId, Name, ref NameBytes, DomainName,
                                          ref DomainNameBytes, ref NameUse);
            }
            LocalFree(SId);
            if (Result)
            {
                return DomainName + "\\" + Name;
            }
            return null;
        }

        private static string ExGetProcessInfoByPID(int PID, out string SID, out string User)
        {
            IntPtr _SID = IntPtr.Zero;
            SID = String.Empty;
            User = string.Empty;
            try
            {
                Process process = Process.GetProcessById(PID);
                if (DumpUserInfo(process.Handle, out _SID))
                {
                    ConvertSidToStringSid(_SID, ref SID);
                    if (!string.IsNullOrEmpty(SID))
                    {
                        User = SIdToName(SID);
                    }
                }
                return process.ProcessName;
            }
            catch
            {
                return "Unknown";
            }
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

                        if (ExGetProcessInfoByPID(proc.Id, out siduser, out user) != "Unknown")
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