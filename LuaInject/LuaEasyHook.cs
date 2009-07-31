using System;
using System.Collections.Generic;
using System.Text;
using EasyHook;
using System.Runtime.InteropServices;
using System.Threading;

namespace LuaInject
{
    public class LuaEasyHook  : EasyHook.IEntryPoint
    {
        internal enum Luas
        {
            Lua_DoString = 0x0049AAB0,
            Lua_Register = 0x004998E0,
            Lua_GetTop = 0x0091A8B0,
            Lua_ToString = 0x0091ADC0,
            Lua_InvalidPtrCheck = 0x0046ED80,
        }

        #region Nested type: LuaDoString

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void LuaDoString(string lua, string fileName, uint pState);

        #endregion

        #region Nested type: LuaGetTop

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int LuaGetTop(IntPtr pLuaState);

        #endregion

        #region Nested type: LuaToString

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate string LuaToString(IntPtr pLuaState, int idx, int length);

        #endregion

        #region Delegates

        public delegate int ConsoleCommandCallback();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint LuaRegisterCommand(string szName, IntPtr pFunc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int RegisteredLuaCommandHandler(IntPtr pLuaState);

        #endregion

        private RegisteredLuaCommandHandler CommandParser;
        private LuaDoString DoStringHandler;
        private LuaGetTop GetTopHandler;
        private List<string> LuaValues = new List<string>();
        public LuaRegisterCommand RegisterCommandHandler;
        private LuaToString ToStringHandler;

        private Lua _Interface;

        public LuaEasyHook(RemoteHooking.IContext InContext, String InChannelName)
        {
            // connect to host...
            _Interface = RemoteHooking.IpcConnectClient<Lua>(InChannelName);
            // validate connection...
            _Interface.Ping("Loaded up..."); 
           
            
        }

        public void Run(RemoteHooking.IContext InContext,String InChannelName)
        {
            CommandParser = OnyxInputHandler;
            RegisterCommandHandler = RegisterDelegate<LuaEasyHook.LuaRegisterCommand>((uint)Luas.Lua_Register);
            DoStringHandler = RegisterDelegate<LuaEasyHook.LuaDoString>((uint)Luas.Lua_DoString);
            GetTopHandler = RegisterDelegate<LuaEasyHook.LuaGetTop>((uint)Luas.Lua_GetTop);
            ToStringHandler = RegisterDelegate<LuaEasyHook.LuaToString>((uint)Luas.Lua_ToString);

            _Interface.Ping("Delegates Setup...");
            _Interface.Ping("Inserting Command...");

            Thread.Sleep(2000);
            RegisterCommand("OnyxInput", CommandParser);

            _Interface.Ping("Command Inserted...");

            _Interface.Ping("Getting Time from WoW...");            
            
            //try getting the time
            try
            {
                int time = GetReturnVal<int>("GetTime()", 0);

                _Interface.Ping("Got Time!");

                _Interface.Ping("Time is " + time.ToString());
            }
            catch (Exception ex)
            {
                _Interface.Ping("Exception: " + ex.ToString());
            }

        }

        public T RegisterDelegate<T>(uint address) where T : class
        {
            return RegisterDelegate<T>(new IntPtr(address));
        }

        public T RegisterDelegate<T>(IntPtr address) where T : class
        {
            return Marshal.GetDelegateForFunctionPointer(address, typeof(T)) as T;
        }

        private int OnyxInputHandler(IntPtr pLuaState)
        {
            LuaValues.Clear();
            int num = GetTop(pLuaState);
            for (int i = 0; i < num; i++)
            {
                string tmp = ToString(pLuaState, i);
                LuaValues.Add(tmp);
            }
            return 0;
        }

        public void DoString(string lua)
        {
            DoStringHandler(lua, "Onyx.lua", 0);
        }

        public string[] GetReturnValues(string lua)
        {
            DoString(string.Format("OnyxInput({0})", lua));
            return LuaValues.ToArray();
        }

        public T GetReturnVal<T>(string lua, uint retVal)
        {
            DoString(string.Format("OnyxInput({0})", lua));
            object tmp;

            if (LuaValues[(int)retVal] == "nil")
                return default(T);

            if (typeof(T) == typeof(bool))
            {
                tmp = LuaValues[(int)retVal] == "1" || LuaValues[(int)retVal].ToLower() == "true";
            }
            else
            {
                tmp = (T)Convert.ChangeType(LuaValues[(int)retVal], typeof(T));
            }
            return (T)tmp;
        }

        public void RegisterCommand(string commandName, LuaEasyHook.RegisteredLuaCommandHandler handler)
        {
            RegisterCommandHandler(commandName, WriteLuaCallback(Marshal.GetFunctionPointerForDelegate(handler)));
            return;
        }

        private IntPtr WriteLuaCallback(IntPtr callbackPtr)
        {
            // You need to either patch the InvalidPtrCheck, or do something else to avoid the EndOfText scan
            // and check. Sorry, no code here.

            return callbackPtr;
        }

        private int GetTop(IntPtr pLuaState)
        {
            return GetTopHandler(pLuaState);
        }

        private string ToString(IntPtr pLuaState, int index)
        {
            return ToStringHandler(pLuaState, index + 1, 0);
        }
    }
}
