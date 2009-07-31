using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dante
{
    public class Logger : MarshalByRefObject
    {
        public string InjectedDLLChannelName { get; set; }

        public void Log(string Message)
        {
            Console.WriteLine(Message);
        }
    }
}
