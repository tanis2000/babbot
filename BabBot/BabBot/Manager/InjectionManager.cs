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
  
    Copyright 2009 BabBot Team
*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using BabBot.Common;
using BabBot.Wow;
using Dante;
using EasyHook;
using Magic;
using System.Collections;

namespace BabBot.Manager
{
    public class LuaExecutionError : Exception
    {
        public LuaExecutionError(string fname)
            : base("Number of returning parameter for function '" + fname + 
                " different from expected. Check the lua code and try again") { }
    }

    public class InjectionManager
    {
        // cache of GUID vs Relation to local player. These are static (more or less)

        private static DanteInterface RemoteObject;

        private readonly IDictionary<ulong, Descriptor.eUnitReaction> RelationCache =
            new Dictionary<ulong, Descriptor.eUnitReaction>();

        private readonly IDictionary<string, uint> SpellIdCache = new Dictionary<string, uint>();

        private bool registered;
        List<string> values;

        #region External function calls

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary(string libFileName);

        [DllImport("kernel32")]
        private static extern bool FreeLibrary(IntPtr hLibModule);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hLibModule, string procName);

        #endregion

        #region Common Assembly

        /// <summary>
        /// Adds code to the FASM assembly stack to update the Current Manager. 
        /// Used at the beginning of many injected function calls.
        /// </summary>
        private void AsmUpdateCurMgr()
        {
            wow.Asm.AddLine("FS mov eax, [0x2C]");
            wow.Asm.AddLine("mov eax, [eax]");
            wow.Asm.AddLine("add eax, 0x10");
            wow.Asm.AddLine("mov dword [eax], {0}", Globals.CurMgr);
        }

        /// <summary>
        /// Send a message back to the bot to know when to resume the mainthread
        /// </summary>
        private void AsmSendResumeMessage()
        {
            var sendmessage = (uint) GetProcAddress(LoadLibrary("User32.dll"), "SendMessageA");
            var hwnd = (uint) AppHelper.BotHandle;

            wow.Asm.AddLine("push eax");
            wow.Asm.AddLine("push {0}", 0x10);
            wow.Asm.AddLine("push {0}", 0x10);
            wow.Asm.AddLine("push {0}", 0xBEEF);
            wow.Asm.AddLine("push {0}", hwnd);
            wow.Asm.AddLine("mov eax, {0}", sendmessage);
            wow.Asm.AddLine("call eax");
            wow.Asm.AddLine("pop eax");
        }

        #endregion

        #region VMT

        /// <summary>
        /// Calls a VMT method for the specified object.
        /// Based on: http://www.mmowned.com/forums/wow-memory-editing/178133-event-based-framework-2.html#post1162121
        /// </summary>
        /// <param name="objAddress">Base Address of the object</param>
        /// <param name="method">Method offset. Note that this offset should already have been multiplied by 4 to get the absolute offset.</param>
        /// <returns>Address of the result if there is one.</returns>
        public uint CallVMT(uint objAddress, uint method)
        {
            ProcessManager.SuspendMainWowThread();

            uint codecave = wow.AllocateMemory();
            uint VMT = wow.ReadUInt(objAddress);
            uint result = 0;

            wow.Asm.Clear();
            AsmUpdateCurMgr();
            wow.Asm.AddLine("mov ecx, {0}", objAddress);
            wow.Asm.AddLine("call {0}", wow.ReadUInt(VMT + method));

            AsmSendResumeMessage();
            wow.Asm.AddLine("retn");

            try
            {
                result = wow.Asm.InjectAndExecute(codecave);
                //Thread.Sleep(10);
            }
            catch (Exception e)
            {
                ProcessManager.ResumeMainWowThread();
                throw e;
            }
            finally
            {
                wow.FreeMemory(codecave);
            }
            return result;
        }

        /// <summary>
        /// Helper override which takes a WowObject instance instead of base address.
        /// </summary>
        /// <param name="obj">WowObject Instance</param>
        /// <param name="method">Method offset. Note that this offset should already have been multiplied by 4 to get the absolute offset.</param>
        /// <returns>Address of the result if there is one.</returns>
        public uint CallVMT(WowObject obj, uint method)
        {
            return CallVMT(obj.ObjectPointer, method);
        }

        public uint Interact(uint objAddress)
        {
            return CallVMT(objAddress, Globals.VMT.Interact);
        }

        public uint Interact(WowObject obj)
        {
            return Interact(obj);
        }

        public string GetName(uint objAddress)
        {
            return wow.ReadASCIIString(CallVMT(objAddress, Globals.VMT.GetName), 255).Trim();
        }

        public string GetName(WowObject obj)
        {
            return GetName(obj.ObjectPointer);
        }

        #endregion

        public void CastSpellByName(string name, bool onSelf)
        {
            ProcessManager.Injector.Lua_DoString(
                string.Format(
                    @"(function()
    local name, rank, icon, cost, isFunnel, powerType, castTime, minRange, maxRange = GetSpellInfo(""{0}"")
    return name, rank, icon, cost, isFunnel, powerType, castTime, minRange, maxRange
end)()",
                    name));
            string castTime = ProcessManager.Injector.Lua_GetLocalizedText(6);

            // empty cast time means we do not know that spell
            if (castTime == "")
            {
                return;
            }

            Int32 realCastTime = Convert.ToInt32(castTime); // milliseconds

            Lua_DoString(string.Format("CastSpellByName(\"{0}\"{1})", name, onSelf ? ",\"player\"" : ""));
            Thread.Sleep(realCastTime + 100);
        }

        /// <summary>
        /// Checks if a unit has a given buff by ID
        /// </summary>
        /// <param name="unit">Unit to check for buffs</param>
        /// <param name="BuffToCheck">Spell ID of the buff to check</param>
        /// <returns>True if unit has the given buff.</returns>
        public bool HasBuffById(WowUnit unit, uint BuffToCheck)
        {
            uint CurrentBuff = unit.ObjectPointer + 
                ProcessManager.CurWoWVersion.Globals.FirstBuff;

            uint Buff = 1;
            while (Buff != 0 && BuffToCheck != 0)
            {
                Buff = wow.ReadUInt(CurrentBuff);
                if (Buff == BuffToCheck)
                {
                    return true;
                }

                CurrentBuff += ProcessManager.CurWoWVersion.Globals.NextBuff;
            }
            return false;
        }

        #region LUA

        public bool IsLuaRegistered
        {
            get { return registered; }
            set { registered = value; }
        }

        public void InjectLua()
        {
            InjectLua(wow.ProcessId);
        }

        public void InjectLua(int ProcessID)
        {
            try
            {
                //create IPC log channell for injected dll, so it can log to the console
                string logChannelName = null;
                IpcServerChannel ipcLogChannel = 
                    RemoteHooking.IpcCreateServer<Logger>(
                        ref logChannelName, WellKnownObjectMode.Singleton);
                var remoteLog = RemoteHooking.IpcConnectClient<Logger>(logChannelName);

                //inject dante.dll, pass in the log channel name
                RemoteHooking.Inject( 
                    ProcessID,
                    "Dante.dll",
                    "Dante.dll",
                    logChannelName);

                //wait for the remote object channel to be created
                Thread.Sleep(200);

                //get remote object from channel name
                RemoteObject = RemoteHooking.IpcConnectClient<DanteInterface>(
                                                remoteLog.InjectedDLLChannelName);
                
                // Register Lua Delegate
                RemoteObject.RegisterLuaDelegate(
                        ProcessManager.CurWoWVersion.Globals.LuaDelegates);

                // Set our codecave offset
                SetPatchOffset(Convert.ToUInt32(
                    ProcessManager.Config.CustomParams.LuaCallback, 16));

                Output.Instance.Log("Dante injected");
            }
            catch (Exception ExtInfo)
            {
                Output.Instance.Log("There was an error while connecting to the target DLL");
                Output.Instance.LogError(ExtInfo);
            }
        }

        public void ClearLuaInjection()
        {
            RemoteObject = null;
        }

        public void SetPatchOffset(uint poffset)
        {
            if (RemoteObject != null)
                RemoteObject.SetPatchOffset(poffset);
        }

        public void Lua_RegisterInputHandler()
        {
            if (!registered)
            {
                // Register
                while (!RemoteObject.SetEndSceneState(0))
                    Thread.Sleep(100);

                // Wait for completion
                int stime = 100;
                while (!RemoteObject.IsLuaRequestCompleted())
                    stime = Sleep(stime);

                registered = true;
            }
        }

        public void Lua_UnRegisterInputHandler()
        {
            // Unregister
            while (!RemoteObject.SetEndSceneState(3))
                Thread.Sleep(100);

            // Wait for completion
            int stime = 100;
            while (!RemoteObject.IsLuaRequestCompleted())
                stime = Sleep(stime);

            registered = false;
        }


        /// <summary>
        /// Wrapper for the RemoteDoString function.
        /// </summary>
        /// <param name="command"></param>
        public void Lua_DoString(string command)
        {
            Lua_DoStringEx(command);
            /*
            Output.Instance.Debug(command, this);
            RemoteObject.DoString(command);

            // Wait for completion
            int stime = 100; 
            while (!RemoteObject.IsLuaRequestCompleted())
                stime = Sleep(stime);
             */
        }

        /// <summary>
        /// Wrapper for the RemoteDoStringInputHandler function.
        /// </summary>
        /// <param name="command"></param>
        public void Lua_DoStringEx(string command)
        {
            Output.Instance.Debug(command, this);
            values = null;
            RemoteObject.DoStringEx(command);

            // Wait for completion
            int stime = 100;
            while (!RemoteObject.IsLuaRequestCompleted())
                stime = Sleep(stime);
        }

        /// <summary>
        /// Sleep given number of msec and return doubled time if need another sleep
        /// </summary>
        /// <param name="stime">Sleep time</param>
        /// <returns></returns>
        private int Sleep(int stime)
        {
            Thread.Sleep(stime);
            return stime * 2;
        }

        // NOTE: tanis - This is no longer calling GetLocalizedText. It's actually retrieving the values
        // from the list of the injected DLL. I am just too lazy to rename all the calls to this 
        // function to a better name :-P 
        public string Lua_GetLocalizedText(int position)
        {
            if (values == null)
                Lua_GetValues();

            if (values != null)
            {
                if (values.Count > position)
                {
                    if (values[position] == null) return "";

                    return values[position];
                }
            }

            return "";
        }

        public List<string> Lua_GetValues()
        {
            values = RemoteObject.GetValues();

            foreach (string s in values)
            {
                Output.Instance.Debug(string.Format("Value[{0}]", s), this);
            }

            return values;
        }

        /// <summary>
        /// Execute lua function by name that doesn't require returning parameters
        /// </summary>
        /// <param name="fname">Function name as defined in WoWData.xml</param>
        /// <param name="param_list">List of parameters</param>
        public void Lua_Exec(string fname, object[] param_list)
        {
            Lua_ExecByName(fname, param_list, null, false);
        }

        /// <summary>
        /// Execute lua function by name without input parameters
        /// </summary>
        /// <param name="fname">Function name as defined in WoWData.xml</param>
        /// <returns>
        ///     Array with first ret_size returning values from lua execution. 
        ///     Final array enumerated from 0 to (ret_size - 1).
        /// </returns>
        public string[] Lua_ExecByName(string fname)
        {
            return Lua_ExecByName(fname, null);
        }

        /// <summary>
        /// Execute lua function by name. 
        /// Number of returning parameters automatically extracted from function definition
        /// </summary>
        /// <param name="fname">Function name as defined in WoWData.xml</param>
        /// <param name="param_list">List of parameters</param>
        /// <returns>
        ///     Array with first ret_size returning values from lua execution. 
        ///     Final array enumerated from 0 to (ret_size - 1).
        /// </returns>
        public string[] Lua_ExecByName(string fname, object[] param_list)
        {
            // Find Function
            // Possible exception
            LuaFunction lf = null;
            try
            {
                lf = FindLuaFunction(fname);
            }
            catch
            {
                ProcessManager.TerminateOnInternalBug(ProcessManager.Bugs.LUA_NOT_FOUND,
                                "Lua Function '" + fname + " not found");
            }

            int ret_size = lf.RetSize;
            
            int[] ret_list = null;
            bool get_all = ret_size < 0;
            if (!get_all && (ret_size > 0))
            {
                // Initialize returning list
                ret_list = new int[ret_size];
                for (int i = 0; i < ret_size; i++)
                    ret_list[i] = i;
            }

            return ExecLua(lf, param_list, ret_list, get_all);
        }

        /// <summary>
        /// Execute lua function by name
        /// </summary>
        /// <param name="fname">Function name as defined in WoWData.xml</param>
        /// <param name="param_list">List of parameters</param>
        /// <param name="res_list">List of parameters needs to be returned</param>
        /// <returns>
        ///     Array with result. Final array enumerated from 0 to (size of returning list - 1).
        ///     For ex if you need parameters 2 and 5 (total 2) to be returned from lua calls than
        ///     in returning array the parameter 2 can be found by index 0 and parameter 5 by index 1
        /// </returns>
        public string[] Lua_ExecByName(string fname, object[] param_list, 
                                                int[] res_list, bool get_all)
        {
            return ExecLua(FindLuaFunction(fname), param_list, res_list, get_all);
        }

        /// <summary>
        /// Execute lua function by name
        /// </summary>
        /// <param name="fname">Function name as defined in WoWData.xml</param>
        /// <param name="param_list">List of parameters</param>
        /// <param name="res_list">List of parameters needs to be returned</param>
        /// <returns>
        ///     Array with result. Final array enumerated from 0 to (size of returning list - 1).
        ///     For ex if you need parameters 2 and 5 (total 2) to be returned from lua calls than
        ///     in returning array the parameter 2 can be found by index 0 and parameter 5 by index 1
        /// </returns>
        private string[] ExecLua(LuaFunction lf, object[] param_list, 
                                            int[] res_list, bool get_all)
        {
            string[] res = null;
            
            // Check number of parameters
            int pcount = (param_list == null) ? 0 : param_list.Length;
            if (lf.ParamSize != pcount)
                ProcessManager.TerminateOnInternalBug(
                    ProcessManager.Bugs.WRONG_LUA_PARAMETER_LIST,
                    "Number of parameters '" + pcount + 
                    "' different from " + lf.ParamSize +
                    " that configured for lua function '" + 
                    lf.Name + "'. Terminating application.");

            // format lua code with parameter list
            string code = lf.Code;
            if (param_list != null)
            {
                try
                {
                    code = string.Format(code, param_list);
                }
                catch (Exception e)
                {
                    ProcessManager.TerminateOnInternalBug(
                        ProcessManager.Bugs.WRONG_LUA_RESULT_LIST,
                        "Unable format parameters '" +
                        param_list.ToString() + "' with lua function '" + 
                        lf.Name + "'. " + e.Message);
                }
            }

            // Check if result expected
            if (!get_all && (res_list == null))
                Lua_DoString(code);
            else
            {
                Lua_DoStringEx(code);
                values = RemoteObject.GetValues();

                // Check returning result
                if (values == null)
                    ProcessManager.TerminateOnInternalBug(
                        ProcessManager.Bugs.NULL_LUA_RETURN,
                        "Null returning result from lua function '" + 
                        lf.Name + "'");
                    
                if (get_all)
                {
                    res = new string[values.Count];
                    values.CopyTo(res);
                }
                else
                {
                    if (values.Count != res_list.Length)
                        ProcessManager.TerminateOnInternalBug(
                            ProcessManager.Bugs.WRONG_LUA_RETURN_LIST,
                            "Number of returning parameters " + values.Count + 
                            " from lua function '" + lf.Name + 
                            "' is different from expected " + res_list.Length);
                      
                    res = new string[res_list.Length];
                    
                    // Initialize returning result
                    for (int i = 0; i < res_list.Length; i++)
                    {
                        if (i == values.Count)
                            break;
                        res[i] = values[i];
                        
                    }

                }
            }

            return res;
        }

        private LuaFunction FindLuaFunction(string fname)
        {
            LuaFunction res = ProcessManager.CurWoWVersion.FindLuaFunction(fname);

            if (res == null)
                ProcessManager.TerminateOnInternalBug(ProcessManager.Bugs.LUA_NOT_FOUND,
                    "Internal bug. The definition of the lua function '" +
                    fname + "' not found in WoWData.xml");

            return res;
        }

        #endregion

        #region FindPattern

        /// <summary>
        /// Quick and dirty method for finding patterns in the wow executable.
        /// </summary>
        public uint FindPattern(string pattern, string mask)
        {
            return FindPattern(pattern, mask, 0, 0);
        }

        /// <summary>
        /// Quick and dirty method for finding patterns in the wow executable.
        /// </summary>
        public uint FindPattern(string pattern, string mask, int add)
        {
            return FindPattern(pattern, mask, add, 0);
        }

        /// <summary>
        /// Quick and dirty method for finding patterns in the wow executable.
        /// Note: this is not perfect and should not be relied on, verify results.
        /// </summary>
        public uint FindPattern(string pattern, string mask, int add, uint rel)
        {
            uint loc = SPattern.FindPattern(wow.ProcessHandle, wow.MainModule, pattern, mask);
            uint mod = 0;
            if (add != 0)
            {
                mod = wow.ReadUInt((uint) (loc + add));
            }

            Output.Instance.Debug(string.Format("final: 0x{0:X08} + 0x{1:X} + 0x{2:X} =  0x{0:X08}", loc, mod, rel, loc + mod + rel), this);
            return loc + mod + rel;
        }

        #endregion

        private BlackMagic wow
        {
            get { return ProcessManager.WowProcess; }
        }

        private void ShowError(string err)
        {
            ProcessManager.ShowError(err);
        }
    }
}