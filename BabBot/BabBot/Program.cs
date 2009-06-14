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
  
    Copyright 2009 BabBot Team -
*/
using System;
using System.Windows.Forms;
using BabBot.Common;
using BabBot.Forms;

namespace BabBot
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                EasyHook.Config.Register(
                    "Dante.",
                    "Dante.dll");
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("This is an administrative task!", "Permission denied...", MessageBoxButtons.OK);

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = new MainForm();
            Application.ThreadException += mainForm.UnhandledThreadExceptionHandler;
            try
            {
#if !DEBUG
                AppHelper.StartHideProcess(); // comment this line if you have problem
#endif
                Application.Run(mainForm);
#if !DEBUG
                AppHelper.StopHideProcess(); // comment this line if you have problem
#endif
            }
            catch (Exception ex)
            {
                var form = new ExceptionForm(ex);
                DialogResult result;
                result = form.ShowDialog();
                Application.Exit();
            }
        }
    }
}