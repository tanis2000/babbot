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
using System.IO;
using System.Windows.Forms;

namespace Dante
{
    public class Logger : MarshalByRefObject
    {
        private object Obj = new Object();

        public string InjectedDLLChannelName { get; set; }

        public Logger()
        {
            lock (Obj)
            {
                File.Delete(Environment.CurrentDirectory + "\\Dante.txt");
                File.Delete(Environment.CurrentDirectory + "\\EndScene.txt");
            }
        }

        public void Log(string Message)
        {
            /// Console.WriteLine(Message);
            /// System.Diagnostics.Debugger.Log(0, "dante",Message);
            lock (Obj)
            {
                try
                {
                    using (StreamWriter w = new StreamWriter(Environment.CurrentDirectory + "\\Dante.txt", true))
                    {
                        w.WriteLine(DateTime.Now.ToLongTimeString() + "," + Message);
                        w.Close();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void LogEndScene(bool state)
        {
            lock (Obj)
            {
                try
                {
                using (StreamWriter w = new StreamWriter(Environment.CurrentDirectory + "\\EndScene.txt", true))
                {
                    w.WriteLine(DateTime.Now.ToLongTimeString() + ", EndScene(): " +
                                ((state) ? "IN" : "OUT"));
                    w.Close();
                }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}
