using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using EasyHook;

namespace LuaInject
{
    public class Lua : MarshalByRefObject
    {

        public Lua()
        {
        }

        public List<string> GetValues()
        {
            return new List<string>();
        }

        public void DoString(string Command)
        {

        }

        public void DoStringInputHandler(string Command)
        {

        }

        public void Ping(string Message)
        {
            Console.WriteLine(Message);
            return;
        }
    }
}
