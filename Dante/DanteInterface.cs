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
                return Main.Values;
            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
            }
            return new List<string>();
        }


    }
}
