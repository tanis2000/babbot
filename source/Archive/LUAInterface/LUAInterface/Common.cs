using System;
using Microsoft.Win32;

namespace LUAInterface
{
    public class WoWHelper
    {
        private const string APPNAME = "Wow.exe";
        private const string KEY = "InstallPath";
        private const string ROOT = @"SOFTWARE\Blizzard Entertainment\World of Warcraft";

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

        #region CreationFlags enum

        /// <summary>
        /// Controls how the process is created. The DefaultErrorMode, NewConsole, and NewProcessGroup flags are enabled by default— even if you do not set the flag, the system will function as if it were set.
        /// </summary>
        [Flags]
        public enum CreationFlags
        {
            /// <summary>
            /// The primary thread of the new process is created in a suspended state, and does not run until the ResumeThread function is called.
            /// </summary>
            Suspended = 0x00000004,
            /// <summary>
            /// The new process has a new console, instead of inheriting the parent's console.
            /// </summary>
            NewConsole = 0x00000010,
            /// <summary>
            /// The new process is the root process of a new process group.
            /// </summary>
            NewProcessGroup = 0x00000200,
            /// <summary>
            /// This flag is only valid starting a 16-bit Windows-based application. If set, the new process runs in a private Virtual DOS Machine (VDM). By default, all 16-bit Windows-based applications run in a single, shared VDM. 
            /// </summary>
            SeperateWOWVDM = 0x00000800,
            /// <summary>
            /// Indicates the format of the lpEnvironment parameter. If this flag is set, the environment block pointed to by lpEnvironment uses Unicode characters.
            /// </summary>
            UnicodeEnvironment = 0x00000400,
            /// <summary>
            /// The new process does not inherit the error mode of the calling process.
            /// </summary>
            DefaultErrorMode = 0x04000000,
        }

        #endregion

    }
}