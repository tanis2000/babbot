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
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using BabBot.Forms;
using BabBot.Manager;

namespace BabBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Forms.MainForm mainForm = new Forms.MainForm();
            Application.ThreadException += new ThreadExceptionEventHandler(mainForm.UnhandledThreadExceptionHandler);
            try
            {
                Application.Run(mainForm);
            }
            catch(Exception ex)
            {
                Forms.ExceptionForm form = new ExceptionForm(ex);
                DialogResult result;
                result = form.ShowDialog();
                Application.Exit();
            }
        }

    }
}
