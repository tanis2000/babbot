using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHook;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security;

namespace Dante
{
    public class Main : EasyHook.IEntryPoint
    {
        DanteInterface Interface;
        Stack<String> Queue = new Stack<String>();

        public uint L;
        public static string msg;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DumpParamsDelegate(uint luaState);

        /*
            __cdecl: caller pushes and pops args off stack, args passed right to left.

            __stdcall: caller pushes, function pops args off stack. args passed right to left.

            So __stdcall generates smaller code, because the instructions to pop the args are only in your binary once, not following every call. Because of this __stdcall cannot be used for functions that have a variable number of arguments: abc(char*, ...).

            __fastcall: first 2 args are passed in registers, function pops args. Remaining parameters are passed left to right.

            this: used by class/struct member functions, the this pointer is passed in ecx. Otherwise like cdecl

            naked: caller removes args, no name decoration. Commonly used when writing asm code.
        */

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void Lua_RegisterDelegate(string name, IntPtr function);
        //public delegate void Lua_RegisterDelegate([MarshalAs(UnmanagedType.LPTStr)] string name, IntPtr function);
        Lua_RegisterDelegate Lua_Register;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint Lua_GetTopDelegate(uint luaState);
        Lua_GetTopDelegate Lua_GetTop;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string Lua_ToStringDelegate(uint luaState, uint idx);
        Lua_ToStringDelegate Lua_ToString;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint Lua_GetStateDelegate();
        Lua_GetStateDelegate Lua_GetState;

        // WoW Function Addresses
        public static class Functions
        {
            public const uint
                CastSpellById = 0x004C4DB0, // 3.1.3
                CastSpellByName = 0x004C4DF0, // 3.1.3 TODO: Test this function 
                GetSpellIdByName = 0x006FF4A0, // 3.1.3
                SelectUnit = 0x006EF810, // 3.1.3
                GetUnitRelation = 0x005AA670, // 3.1.3
                CInputControl = 0x0113F8E4, // 3.1.3
                CInputControl_SetFlags = 0x00691BB0, // 3.1.3
                Lua_DoString = 0x0049AAB0, // 3.1.3
                Lua_GetLocalizedText = 0x005A82F0, // 3.1.3
                Lua_Register = 0x004998E0, // 3.1.3
                Lua_GetTop = 0x0091A8B0, // 3.1.3
                Lua_ToString = 0x0091ADC0, // 3.1.3
                Lua_GetState = 0x00499700; // 3.1.3

        }

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

                //L = Lua_GetState();
                Interface.SendMessage(RemoteHooking.GetCurrentProcessId(), string.Format("Lua_GetState() = {0:X}", L));

                DumpParamsDelegate x = DumpParams;

                IntPtr DumpParamsPtr = Marshal.GetFunctionPointerForDelegate(x);

                Interface.SetFunctionPtr(RemoteHooking.GetCurrentProcessId(), DumpParamsPtr);

                Lua_Register("DumpParams", (IntPtr)0x00401643); // This is the code hole we want to use
                Interface.SendMessage(RemoteHooking.GetCurrentProcessId(), "Registered DumpParams()");

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


        #region LUA


        public void DumpParams(uint luaState)
        {
            int a = 0x12345678;
            msg = "dump!";
            /*
              int n = LuaGetTop(L);  // number of arguments
              for (int i=1; i<=n; i++) 
              {
                    const char *out=lua_tostring(L,i,NULL);
                    if (out && out[0])
                        printf("%s",out);
              }
             */

            /*
            Main This = (Main)HookRuntimeInfo.Callback;

            lock (This.Queue)
            {
                This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                    RemoteHooking.GetCurrentThreadId() + "]: \"" + "Hello there, I'm DumpParams!" + "\"");
            }
            */

            /*
            uint n = Lua_GetTop(luaState);
            for (uint i = 1; i <= n; i++)
            {
                string res = Lua_ToString(luaState, i);
                //Console.WriteLine(res);
            }
            */

        }

        #endregion


    }
}
