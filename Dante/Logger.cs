using System;

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