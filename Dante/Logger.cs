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
using System.Runtime.CompilerServices;

namespace Dante
{
    public class Logger : MarshalByRefObject
    {
        public string InjectedDLLChannelName { get; set; }

        public Logger()
        {
            File.Delete(Environment.CurrentDirectory+"\\Dante.txt");
            File.Delete(Environment.CurrentDirectory+"\\EndScene.txt");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(string Message)
        {
            /// Console.WriteLine(Message);
            /// System.Diagnostics.Debugger.Log(0, "dante",Message);
            
            using (StreamWriter w = new StreamWriter(Environment.CurrentDirectory+"\\Dante.txt", true))
            {
                w.WriteLine(DateTime.Now.ToLongTimeString() + "," + Message);
                w.Close();
            }
        }

        public void LogEndScene(bool state)
        {
            using (StreamWriter w = new StreamWriter(Environment.CurrentDirectory+"\\EndScene.txt", true))
            {
                w.WriteLine(DateTime.Now.ToLongTimeString() + ", EndScene(): " + 
                    ((state) ? "IN" : "OUT"));
                w.Close();
            }
        }
    }
}