using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Manager;

namespace Dante
{
    public class DanteInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 InClientPID)
        {
            Console.WriteLine("FileMon has been installed in target {0}.\r\n", InClientPID);
        }

        public void SendMessage(Int32 InClientPID, String InMessage)
        {
            Console.WriteLine(InMessage);
        }

        public void ReportException(Exception InInfo)
        {
            Console.WriteLine("The target process has reported an error:\r\n" + InInfo.ToString());
        }

        public void Ping()
        {
            Console.WriteLine("Ping received");
        }

        public void Ping2(string msg)
        {
            Console.WriteLine("Ping: " + msg);
        }

        public void DumpParams(Int32 InClientPID, String InMessage)
        {
            Console.WriteLine("DumpParams() - " + InMessage);
        }

        public void SetFunctionPtr(Int32 InClientPID, IntPtr Pointer)
        {
            Console.WriteLine(string.Format("Pointer: {0:X}", Pointer.ToInt32()));
            ProcessManager.Injector.PatchCode(Pointer);
        }

    }
}
