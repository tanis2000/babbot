using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace BabBot.Common
{
    public class AppHelper
    {
        private const string APPNAME = "Wow.exe";
        private const string KEY = "InstallPath";
        private const string ROOT = @"SOFTWARE\Blizzard Entertainment\World of Warcraft";
        private const string WND_TITLE = "World of Warcraft";

        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(int hWnd);


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
        /// Dummy version to check and wait for Wow Process 
        /// </summary>
        public static void WaitForWowWindow()
        {
            int hWnd = 0;
            bool isVisible = false;
            while (hWnd == 0 && !isVisible)
            {
                hWnd = FindWindow(null, WND_TITLE);
                if (hWnd != 0)
                {
                    isVisible = IsWindowVisible(hWnd);
                }
                Thread.Sleep(1000);
            }
        }
    }
}