using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dante
{
    public class DanteInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 InClientPID)
        {
            Console.WriteLine("FileMon has been installed in target {0}.\r\n", InClientPID);
        }

        public void OnEndScene(Int32 InClientPID, String InMessage)
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

        public void DumpParams(Int32 InClientPID, String InMessage)
        {
            Console.WriteLine("DumpParams() - " + InMessage);
        }

    
    }
}
