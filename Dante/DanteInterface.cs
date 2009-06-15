using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dante
{
    public abstract class DanteInterface : MarshalByRefObject
    {
        public abstract void IsInstalled(Int32 InClientPID);
        public abstract void OnEndScene(Int32 InClientPID, String InMessage);
        public abstract void ReportException(Exception InInfo);
        public abstract void Ping();
        public abstract void DumpParams(Int32 InClientPID, String InMessage);
        public abstract void SetFunctionPtr(Int32 InClientPID, IntPtr Pointer);    
    }
}
