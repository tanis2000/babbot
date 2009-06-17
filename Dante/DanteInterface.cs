using System;
using System.Collections.Generic;

namespace Dante
{
    public abstract class DanteInterface : MarshalByRefObject
    {
        public abstract void IsInstalled(Int32 InClientPID);
        public abstract void SendMessage(Int32 InClientPID, String InMessage);
        public abstract void ReportException(Exception InInfo);
        public abstract void Ping();
        public abstract void Ping2(string msg);
        public abstract void DumpParams(Int32 InClientPID, String InMessage);
        public abstract void SetFunctionPtr(Int32 InClientPID, IntPtr Pointer);

        public void DoString(string command)
        {
            Main.DoString(command);
        }

        public List<string> GetValues()
        {
            return Main.Values;
        }
    }
}
