using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading;

namespace Dante
{
    public class DanteInterface : MarshalByRefObject
    {
        public void DoString(string command)
        {
            try
            {
                lock (LuaInterface.oLocker)
                {
                    LuaInterface.PendingDoString = command;
                }
                //LuaInterface.Interface.Log(string.Format("Calling DoString(\"{0}\")", command));
                //LuaInterface.DoString(command);
                //LuaInterface.Interface.Log(string.Format("Done with DoString"));
            }
            catch(Exception e)
            {
                LuaInterface.LoggingInterface.Log(e.ToString());
            }
        }

        public void DoStringInputHandler(string command)
        {
            try
            {
                LuaInterface.LoggingInterface.Log(string.Format("Calling DoStringInputHandler(\"{0}\")", command));
                LuaInterface.DoStringInputHandler(command);
                LuaInterface.LoggingInterface.Log(string.Format("Done with DoStringInputHandler"));
            }
            catch (Exception e)
            {
                LuaInterface.LoggingInterface.Log(e.ToString());
            }
        }

        public List<string> GetValues()
        {
            try {
                LuaInterface.LoggingInterface.Log(string.Format("GetValues"));

                lock (LuaInterface.Values)
                {
                    foreach (string s in LuaInterface.Values)
                    {
                        LuaInterface.LoggingInterface.Log("[" + s + "]");
                    }
                    return LuaInterface.Values;
                }
            }
            catch (Exception e)
            {
                LuaInterface.LoggingInterface.Log(e.ToString());
            }
            return new List<string>();
        }

        public void RegisterLuaHandler()
        {
            lock (LuaInterface.oLocker)
            {
                LuaInterface.PendingRegistration = true;
            }
        }

        public void Patch()
        {            
            LuaInterface.LoggingInterface.Log(string.Format("Patching WoW.. CommandHandler: {0:X}", (uint)LuaInterface.CommandHandlerPtr));
            int bw = LuaInterface.SetFunctionPtr(LuaInterface.CommandHandlerPtr);
            LuaInterface.LoggingInterface.Log(string.Format("Bytes written: {0}", bw));
        }

        public void RestorePatch()
        {
            LuaInterface.LoggingInterface.Log(string.Format("Patching WoW.. restoring patched CommandHandler"));
            int bw = LuaInterface.RestoreFunctionPtr();
            LuaInterface.LoggingInterface.Log(string.Format("Bytes written: {0}", bw));
        }
    }
}
