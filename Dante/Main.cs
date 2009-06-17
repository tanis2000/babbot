using System;
using System.Collections.Generic;
using EasyHook;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security;
using System.Diagnostics;

namespace Dante
{
    public class Main : EasyHook.IEntryPoint
    {
        // WoW Function Addresses
        private static class Functions
        {
            public const uint
                Lua_DoString = 0x0049AAB0, // 3.1.3
                Lua_GetLocalizedText = 0x005A82F0, // 3.1.3
                Lua_Register = 0x004998E0, // 3.1.3
                Lua_GetTop = 0x0091A8B0, // 3.1.3
                Lua_ToString = 0x0091ADC0, // 3.1.3
                Lua_GetState = 0x00499700; // 3.1.3
        }
        
        // Common IPC interface
        private DanteInterface Interface;
        // Communication queue
        private Stack<String> Queue = new Stack<String>();

        // Our lua_State
        private static uint L;

        // Temp test msg
        private static string msg;


        /*
            __cdecl: caller pushes and pops args off stack, args passed right to left.

            __stdcall: caller pushes, function pops args off stack. args passed right to left.

            So __stdcall generates smaller code, because the instructions to pop the args are only in your binary once, not following every call. Because of this __stdcall cannot be used for functions that have a variable number of arguments: abc(char*, ...).

            __fastcall: first 2 args are passed in registers, function pops args. Remaining parameters are passed left to right.

            this: used by class/struct member functions, the this pointer is passed in ecx. Otherwise like cdecl

            naked: caller removes args, no name decoration. Commonly used when writing asm code.
        */

        #region delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CommandHandlerDelegate(uint luaState);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void Lua_RegisterDelegate(string name, IntPtr function);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint Lua_GetTopDelegate(uint luaState);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate string Lua_ToStringDelegate(uint luaState, uint idx, uint length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint Lua_GetStateDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Lua_DoStringDelegate(string command, string filename, uint pState);

        #endregion

        private static Lua_RegisterDelegate Lua_Register;
        private static Lua_GetTopDelegate Lua_GetTop;
        private static Lua_ToStringDelegate Lua_ToString;
        private static Lua_GetStateDelegate Lua_GetState;
        private static Lua_DoStringDelegate Lua_DoString;
        private static CommandHandlerDelegate CommandHandler = InputHandler;


        public static List<string> Values = new List<string>();

        public Main(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // connect to host...
            Interface = RemoteHooking.IpcConnectClient<DanteInterface>(InChannelName);

            Interface.Ping();
        }

        public void Run(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            RemoteHooking.WakeUpProcess();

            msg = "";
            // wait for host process termination...
            
            try
            {
                Lua_Register = (Lua_RegisterDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_Register, typeof(Lua_RegisterDelegate));
                Lua_GetTop = (Lua_GetTopDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_GetTop, typeof(Lua_GetTopDelegate));
                Lua_ToString = (Lua_ToStringDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_ToString, typeof(Lua_ToStringDelegate));
                Lua_GetState = (Lua_GetStateDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_GetState, typeof(Lua_GetStateDelegate));
                Lua_DoString = (Lua_DoStringDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_DoString, typeof(Lua_DoStringDelegate));

                // Init the lua_State
                L = Lua_GetState();
                Interface.SendMessage(RemoteHooking.GetCurrentProcessId(), string.Format("Lua_GetState() = {0:X}", L));


                IntPtr CommandHandlerPtr = Marshal.GetFunctionPointerForDelegate(CommandHandler);

                Interface.SetFunctionPtr(RemoteHooking.GetCurrentProcessId(), CommandHandlerPtr);

                Lua_Register("InputHandler", (IntPtr)0x00401643); // This is the code hole we want to use
                Interface.SendMessage(RemoteHooking.GetCurrentProcessId(), "Registered InputHandler()");

                while (true)
                {
                    Thread.Sleep(500);
                    
                    // transmit queued messages if any..
                    if (Queue.Count > 0)
                    {
                        String[] Package = null;

                        lock (Queue)
                        {
                            Package = Queue.ToArray();

                            Queue.Clear();
                        }

                        Interface.SendMessage(RemoteHooking.GetCurrentProcessId(), Package[0]);
                    }
                    else
                    {
                        Interface.Ping();
                        Interface.Ping2(msg);
                    }
                }
            }
            catch (Exception e)
            {
                Interface.SendMessage(RemoteHooking.GetCurrentProcessId(), e.ToString());
                // Ping() will raise an exception if host is unreachable
            }
        }

        public static T GetReturnVal<T>(string lua, uint retVal)
        {
            Lua_DoString(string.Format("InputHandler({0})", lua), "BabBot.lua", 0);
            object tmp;

            if (Values[(int)retVal] == "nil")
                return default(T);

            if (typeof(T) == typeof(bool))
            {
                tmp = Values[(int)retVal] == "1" || Values[(int)retVal].ToLower() == "true";
            }
            else
            {
                tmp = (T)Convert.ChangeType(Values[(int)retVal], typeof(T));
            }
            return (T)tmp;
        }

        public static void DoString(string command) 
        {
            Lua_DoString(string.Format("InputHandler({0})", command), "BabBot.lua", 0);
        }

        #region LUA


        public static int InputHandler(uint luaState)
        {
            msg = "dump!";

            /*
            Main This = (Main)HookRuntimeInfo.Callback;

            lock (This.Queue)
            {
                This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                    RemoteHooking.GetCurrentThreadId() + "]: \"" + "Hello there, I'm DumpParams!" + "\"");
            }
            */

            Values.Clear();
            uint n = Lua_GetTop(luaState);
            for (uint i = 1; i <= n; i++)
            {
                string res = Lua_ToString(luaState, i, 0);
                Values.Add(res);
            }
            return 0;
        }

        #endregion


    }
}
