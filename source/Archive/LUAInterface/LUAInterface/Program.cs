using System;
using System.IO;
using System.Windows.Forms;
using EasyHook;

namespace LUAInterface
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            string LuaHostPath = Path.Combine(Application.StartupPath, "LUAHost.dll");
            string SharedInterfacePath = Path.Combine(Application.StartupPath, "SharedInterface.dll");
            Config.Register("LUAHost", LuaHostPath);
            Config.Register("SharedInterface", SharedInterfacePath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}