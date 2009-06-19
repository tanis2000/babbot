using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace Dante
{
    public class DanteInterface : MarshalByRefObject, ISharedAssembly
    {
        public void DoString(string command)
        {
            try
            {
                Log.Debug(string.Format("Calling DoString(\"{0}\")", command));
                Main.DoString(command);
                Log.Debug(string.Format("Done with DoString"));
            }
            catch(Exception e)
            {
                Log.Debug(e.ToString());
            }
        }

        public List<string> GetValues()
        {
            try {
                Log.Debug(string.Format("Calling GetValues"));
                foreach (string s in Main.Values)
                {
                    Log.Debug("[" + s + "]");
                }
                return Main.Values;
            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
            }
            return new List<string>();
        }

        public void Patch()
        {
            Log.Debug(string.Format("Patching WoW.. CommandHandler: {0:X}", (uint)Main.CommandHandlerPtr));
            int bw = Main.SetFunctionPtr(Main.CommandHandlerPtr);
            Log.Debug(string.Format("Bytes written: {0}", bw));
        }

        public void RestorePatch()
        {
            Log.Debug(string.Format("Patching WoW.. restoring patched CommandHandler"));
            int bw = Main.RestoreFunctionPtr();
            Log.Debug(string.Format("Bytes written: {0}", bw));
        }
    }
}
