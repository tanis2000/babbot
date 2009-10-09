/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team -
*/

using System;
using System.Collections.Generic;

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
                LuaInterface.LoggingInterface.Log(string.Format("Calling DoString(\"{0}\")", command));
                //LuaInterface.DoString(command);
                LuaInterface.LoggingInterface.Log(string.Format("Done with DoString"));
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
                Patch();
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
