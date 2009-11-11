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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using EasyHook;

namespace Dante
{
    public class LuaInterface : IEntryPoint
    {
        #region declarations

        private static readonly CommandHandlerDelegate CommandHandler = InputHandler;

        private static int BytesWritten;
        public static IntPtr CommandHandlerPtr = Marshal.GetFunctionPointerForDelegate(CommandHandler);
        public static Logger LoggingInterface;
        private static Lua_DoStringDelegate Lua_DoString;
        private static Lua_GetStateDelegate Lua_GetState;

        private static Lua_GetTopDelegate Lua_GetTop;
        private static Lua_RegisterDelegate Lua_Register;
        private static Lua_ToStringDelegate Lua_ToString;
        private static string lua_cmd = "plugin.lua";

        internal static string PendingDoString = string.Empty;
        public static List<string> Values = new List<string>();
        internal static bool ValueReceived;
        internal static byte LuaState = 255;
        internal static bool InputHandlerRegistered = false;

        #endregion

        #region delegates

        /*
            __cdecl: caller pushes and pops args off stack, args passed right to left.

            __stdcall: caller pushes, function pops args off stack. args passed right to left.

            So __stdcall generates smaller code, because the instructions to pop the args are only in your binary once, not following every call. Because of this __stdcall cannot be used for functions that have a variable number of arguments: abc(char*, ...).

            __fastcall: first 2 args are passed in registers, function pops args. Remaining parameters are passed left to right.

            this: used by class/struct member functions, the this pointer is passed in ecx. Otherwise like cdecl

            naked: caller removes args, no name decoration. Commonly used when writing asm code.
        */

        #region Nested type: CommandHandlerDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CommandHandlerDelegate(uint luaState);

        #endregion

        #region Nested type: Lua_DoStringDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Lua_DoStringDelegate(string command, string fileName, uint hwEvent); // hwEvent should always be passed as zero

        #endregion

        #region Nested type: Lua_GetStateDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint Lua_GetStateDelegate();

        #endregion

        #region Nested type: Lua_GetTopDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int Lua_GetTopDelegate(uint luaState);

        #endregion

        #region Nested type: Lua_RegisterDelegate

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void Lua_RegisterDelegate(string name, IntPtr function);

        #endregion

        #region Nested type: Lua_ToStringDelegate

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate string Lua_ToStringDelegate(uint luaState, int idx, uint length);

        #endregion

        #endregion

        #region DirectXHook

        private LocalHook Direct3DCreate9Hook;
        private IDirect3D9 IDirect3D9;

        // This is our fake version of Direct3DCreate9.  We call the real one but this let's us grab the IDirect3D pointer.
        private static unsafe D3D9.IDirect3D9* MyDirect3DCreate9(ushort SdkVersion)
        {
            // Since this function is static we need to get a reference to "this".
            var This = (LuaInterface) HookRuntimeInfo.Callback;
            if (This.Direct3DCreate9Hook.IsThreadIntercepted(0))
            {
                LoggingInterface.Log("Direct3DCreate9Hook.IsThreadIntercepted(0)");

                // Exclude the current thread (WoW main thread) from to be intercepted
                This.Direct3DCreate9Hook.ThreadACL.SetExclusiveACL(new Int32[] {0});
                LoggingInterface.Log("Underlying interception has been removed");
            }

            // Create one of our custom IDirect3D9 objects (which insantiates a real one and passes most calls through to it).
            LoggingInterface.Log(string.Format("MyDirect3DCreate9 Start...SDK Version {0}", SdkVersion));
            This.IDirect3D9 = new IDirect3D9(SdkVersion);
            LoggingInterface.Log("MyDirect3DCreate9 has been created");

            return This.IDirect3D9.NativeIDirect3D9;
        }

        // This is the wrapping that let's us give Win32 a delegate instead of a function pointer.
        [UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true)]
        private unsafe delegate D3D9.IDirect3D9* Direct3DCreate9Delegate(ushort SdkVersion);

        #endregion

        #region LoadLibraryHook

        private const int HIT_BARRIER = 1;
        private int hitCount;

        private LocalHook LoadLibraryHook;

        private static IntPtr MyLoadLibrary(string lpFileName)
        {
            // WoW main thread

            var This = (LuaInterface) HookRuntimeInfo.Callback;

            if (lpFileName == "d3d9.dll")
            {
                if (This.hitCount == HIT_BARRIER)
                {
                    LoggingInterface.Log(string.Format("LoadLibraryA on {0}", lpFileName));
                    IntPtr hModule = Kernel32.LoadLibrary(lpFileName);

                    if (hModule != IntPtr.Zero)
                    {
                        This.hitCount++;
                        This.InstallDirect3DHook();
                        return hModule;
                    }
                }

                This.hitCount++;
            }

            return Kernel32.LoadLibrary(lpFileName);
        }

        public unsafe void InstallDirect3DHook()
        {
            try
            {
                LoggingInterface.Log("Creating Direct3DCreate9Hook...");

                Direct3DCreate9Hook = LocalHook.Create(LocalHook.GetProcAddress("D3D9.DLL", "Direct3DCreate9"),
                                                       new Direct3DCreate9Delegate(MyDirect3DCreate9),
                                                       this);

                Direct3DCreate9Hook.ThreadACL.SetInclusiveACL(new Int32[] {0});
                if (Direct3DCreate9Hook.IsThreadIntercepted(0))
                {
                    LoggingInterface.Log("Direct3DCreate9Hook.IsThreadIntercepted(0)");
                }
            }
            catch (Exception e)
            {
                hitCount = 0;
                LoggingInterface.Log(string.Format("Exception on Direct3DCreate9Hook: " + e.Message));
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi)]
        private delegate IntPtr LoadLibraryDelegate(string lpFileName);

        #endregion

        #region LUA

        internal static void Patch()
        {
            LuaInterface.LoggingInterface.Log(string.Format("Patch() - Patching CommandHandler: {0:X} ...", (uint)LuaInterface.CommandHandlerPtr));
            int bw = LuaInterface.SetFunctionPtr(LuaInterface.CommandHandlerPtr);
            LuaInterface.LoggingInterface.Log(string.Format("Patch() - Patched CommandHandler. Bytes written: {0}", bw));
        }

        internal static void RestorePatch()
        {
            LuaInterface.LoggingInterface.Log(string.Format("RestorePatch() - Restoring patched CommandHandler ... "));
            int bw = LuaInterface.RestoreFunctionPtr();
            LuaInterface.LoggingInterface.Log(string.Format("RestorePatch() - Restored patched CommandHandler. Bytes written: {0}", bw));
        }

        // Calls LUA's DoString directly without using our InputHandler
        internal static void DoString(string command)
        {
            try
            {
                LoggingInterface.Log("DoString() - Calling ...");
                LoggingInterface.Log("DoString() - " + command);
                Lua_DoString(command, lua_cmd, 0);
                LoggingInterface.Log("DoString() - Done");
            }
            catch (SEHException e)
            {
                LoggingInterface.Log("DoString() exception: " + e.ToString());
            }
        }

        // Calls LUA's DoString encapsulating the command into our InputHandler
        public static void DoStringEx(string command)
        {
            try
            {
                string cmd = string.Format("InputHandler({0})", command);
                LoggingInterface.Log("DoStringEx() - Calling ...");
                LoggingInterface.Log("DoStringEx() - " + cmd);
                Lua_DoString(cmd, lua_cmd, 0);
                LoggingInterface.Log("DoString() - Done");
            }
            catch (SEHException e)
            {
                LoggingInterface.Log("DoStringEx() exception: " + e.ToString());
            }
            catch (Exception e)
            {
                LoggingInterface.Log("DoStringEx() exception: " + e.ToString());
            }
        }

        public static int InputHandler(uint luaState)
        {
            try
            {
                LoggingInterface.Log("InputHandler() - Calling ...");

                Values.Clear();
                int n = Lua_GetTop(Lua_GetState());
                LoggingInterface.Log(string.Format("InputHandler() - Passed LUA_State (ignored): {0:X}", luaState));
                LoggingInterface.Log(string.Format("InputHandler() - Our own LUA_State: {0:X}", Lua_GetState()));
                LoggingInterface.Log("InputHandler() - Vars num: " + n);
                for (int i = 1; i <= n; i++)
                {
                    string res = Lua_ToString(Lua_GetState(), i, 0);
                    LoggingInterface.Log(string.Format(
                                             "InputHandler() - Var[{0}] = {1}", i, res));
                    Values.Add(res);
                }
                LoggingInterface.Log("InputHandler() - Done");
            } catch (Exception e)
            {
                LoggingInterface.Log("InputHandler() exception: " + e.ToString());                
            }

            ValueReceived = true;

            return 0;
        }

        public static int SetFunctionPtr(IntPtr pointer)
        {
            LoggingInterface.Log("SetFunctionPtr() - Starting ...");
            bool ReturnVal;
            uint p = (uint) pointer - Functions.Patch_Offset - 5;
            var buf = new byte[4];
            var buf2 = new byte[1];
            buf2[0] = 0xE9;
            buf[3] = (byte) ((p & 0xFF000000) >> 24);
            buf[2] = (byte) ((p & 0xFF0000) >> 16);
            buf[1] = (byte) ((p & 0xFF00) >> 8);
            buf[0] = (byte) ((p & 0xFF));

            IntPtr hProcess = Kernel32.GetCurrentProcess();
                // OpenProcess(ProcessAccessFlags.All, false, (UInt32)proc[0].Id);

            LoggingInterface.Log(string.Format("SetFunctionPtr() - hProcess = {0:X}", (uint) hProcess));

            ReturnVal = Kernel32.WriteProcessMemory(hProcess, (IntPtr) Functions.Patch_Offset, buf2, 1, out BytesWritten);
            if (!ReturnVal)
            {
                LoggingInterface.Log(string.Format("SetFunctionPtr() - Error during first WriteProcessMemory"));
            }
            LoggingInterface.Log(string.Format("SetFunctionPtr() - Written {0:d} bytes", BytesWritten));

            ReturnVal = Kernel32.WriteProcessMemory(hProcess, (IntPtr) (Functions.Patch_Offset + 1), buf, 4,
                                                    out BytesWritten);
            if (!ReturnVal)
            {
                LoggingInterface.Log(string.Format("SetFunctionPtr() - Error during second WriteProcessMemory"));
            }
            LoggingInterface.Log(string.Format("SetFunctionPtr() - Written {0:d} bytes", BytesWritten));

            LoggingInterface.Log("SetFunctionPtr() - Done");

            return BytesWritten;
        }

        public static int RestoreFunctionPtr()
        {
            LoggingInterface.Log("RestoreFunctionPtr() - Starting ...");
            bool ReturnVal;
            var buf = new byte[5];
            buf[0] = buf[1] = buf[2] = buf[3] = buf[4] = 0xCC;

            IntPtr hProcess = Kernel32.GetCurrentProcess();
                // OpenProcess(ProcessAccessFlags.All, false, (UInt32)proc[0].Id);

            LoggingInterface.Log(string.Format("RestoreFunctionPtr -> hProcess = {0:X}", 
                                                                            (uint) hProcess));

            ReturnVal = Kernel32.WriteProcessMemory(hProcess, 
                (IntPtr) Functions.Patch_Offset, buf, 5, out BytesWritten);

            LoggingInterface.Log("RestoreFunctionPtr() - Done");

            return BytesWritten;
        }

        private static void RegisterLUADelegate()
        {
            LoggingInterface.Log("RegisterLUADelegate:  Lua_Register");
            Lua_Register = Tools.GetRegisterDelegate<Lua_RegisterDelegate>(Functions.Lua_Register);

            LoggingInterface.Log("RegisterLUADelegate:  Lua_GetTop");
            Lua_GetTop = Tools.GetRegisterDelegate<Lua_GetTopDelegate>(Functions.Lua_GetTop);

            LoggingInterface.Log("RegisterLUADelegate:  Lua_ToString");
            Lua_ToString = Tools.GetRegisterDelegate<Lua_ToStringDelegate>(Functions.Lua_ToString);

            LoggingInterface.Log("RegisterLUADelegate:  Lua_GetState");
            Lua_GetState = Tools.GetRegisterDelegate<Lua_GetStateDelegate>(Functions.Lua_GetState);

            LoggingInterface.Log("RegisterLUADelegate:  Lua_DoString");
            Lua_DoString = Tools.GetRegisterDelegate<Lua_DoStringDelegate>(Functions.Lua_DoString);
        }

        private static void InitLUAState()
        {
            LoggingInterface.Log(string.Format("InitLUAState returned {0:X}", Lua_GetState()));
        }


        public static void RegisterLuaInputHandler()
        {
            LoggingInterface.Log(string.Format("RegisterLuaInputHandler() - Registering our InputHandler.."));
            Lua_Register("InputHandler", (IntPtr) Functions.Patch_Offset); // This is the code hole we want to use
            InitLUAState();
            LoggingInterface.Log(string.Format("RegisterLuaInputHandler() - InputHandler Registered"));
        }

        #endregion

        public LuaInterface(
            RemoteHooking.IContext InContext,
            string LogChannelName)
        {
            LoggingInterface = RemoteHooking.IpcConnectClient<Logger>(LogChannelName);

            LoggingInterface.Log("Main()");
            Process thisProcess = Process.GetCurrentProcess();
            LoggingInterface.Log(string.Format("Hooked Into: {0}, {1}", 
                                    thisProcess.ProcessName, thisProcess.Id));
        }

        public void Run(
            RemoteHooking.IContext InContext,
            string ChannelName)
        {
            LoggingInterface.Log("Run()");

            try
            {
                LoggingInterface.Log("Setting up IPC channel :)");
                string outChannelName = null;
                IpcServerChannel ipcLogChannel = RemoteHooking.IpcCreateServer<DanteInterface>(ref outChannelName,
                                                                                               WellKnownObjectMode.
                                                                                                   Singleton);

                //notify client of channel creation via logger
                LoggingInterface.InjectedDLLChannelName = outChannelName;

                // Register our LUA delegates
                RegisterLUADelegate();

                // Init LUA state
                InitLUAState();

                // Install the first hook...
                try
                {
                    hitCount = 0;
                    LoggingInterface.Log("Creating LoadLibraryHook...");

                    // Create the hook on the LoadLibraryA call 
                    LoadLibraryHook = LocalHook.Create(LocalHook.GetProcAddress("kernel32.dll", "LoadLibraryA"),
                                                       new LoadLibraryDelegate(MyLoadLibrary),
                                                       this);

                    LoggingInterface.Log("SetExclusiveACL on LoadLibraryHook...");
                    LoadLibraryHook.ThreadACL.SetExclusiveACL(new Int32[] {0});
                }
                catch (Exception e)
                {
                    hitCount = 0;
                    LoggingInterface.Log(string.Format("Exception on LoadLibraryHook: " + e.Message));
                }

                while (true)
                {
                    Thread.Sleep(250);
                }
            }
            catch (Exception e)
            {
                LoggingInterface.Log(string.Format("Exception in Run(): {0}", e));
            }
        }

        #region garbage

        /*
                

                LoggingInterface.Log("Setting up process stuff");
                // Start the server stuff
                LoggingInterface.Log(string.Format("Waking up process.."));
                //RemoteHooking.WakeUpProcess();


                LoggingInterface.Log(string.Format("Registering LUA functions.."));
                Lua_Register =
                    (Lua_RegisterDelegate)
                    Marshal.GetDelegateForFunctionPointer((IntPtr) Functions.Lua_Register, typeof (Lua_RegisterDelegate));
                Lua_GetTop =
                    (Lua_GetTopDelegate)
                    Marshal.GetDelegateForFunctionPointer((IntPtr) Functions.Lua_GetTop, typeof (Lua_GetTopDelegate));
                Lua_ToString =
                    (Lua_ToStringDelegate)
                    Marshal.GetDelegateForFunctionPointer((IntPtr) Functions.Lua_ToString, typeof (Lua_ToStringDelegate));
                Lua_GetState =
                    (Lua_GetStateDelegate)
                    Marshal.GetDelegateForFunctionPointer((IntPtr) Functions.Lua_GetState, typeof (Lua_GetStateDelegate));
                Lua_DoString =
                    (Lua_DoStringDelegate)
                    Marshal.GetDelegateForFunctionPointer((IntPtr) Functions.Lua_DoString, typeof (Lua_DoStringDelegate));

                // Init the lua_State
                L = Lua_GetState();
                LoggingInterface.Log(string.Format("GetState returned {0:X}", L));


                //Interface.Log(string.Format("Patching WoW.. CommandHandler: {0:X}", (uint)CommandHandlerPtr));
                //int bw = SetFunctionPtr(CommandHandlerPtr);
                //Interface.Log(string.Format("Bytes written: {0}", bw));
        public static T GetReturnVal<T>(string lua, uint retVal)
        {
            Lua_DoString(string.Format("InputHandler({0})", lua), "BabBot.lua", 0);
            object tmp;

            if (Values[(int) retVal] == "nil")
            {
                return default(T);
            }

            if (typeof (T) == typeof (bool))
            {
                tmp = Values[(int) retVal] == "1" || Values[(int) retVal].ToLower() == "true";
            }
            else
            {
                tmp = (T) Convert.ChangeType(Values[(int) retVal], typeof (T));
            }
            return (T) tmp;
            
        }
        */

        #endregion

        #region Nested type: Functions

        private static class Functions
        {
            public const uint
                // 3.2.2a
                Lua_DoString = 0x7CF6B0; // 3.1.3: 0x0049AAB0;

            public const uint
                // 3.2.2a
                Lua_GetState = 0x007CE280; // 3.1.3: 0x00499700;

            public const uint
                // 3.2.2a
                Lua_GetTop = 0x00803340; // 3.1.3: 0x0091A8B0;

            public const uint
                // 3.2.2a
                Lua_Register = 0x007CE460; // 3.1.3: 0x004998E0;

            public const uint
                // 3.2.2a
                Lua_ToString = 0x803850; // 3.1.3: 0x0091ADC0;

            public const uint
                // 3.2.2 but looks wrong
                Lua_GetCurrentMapZone = 0x004CDA20;
                
            // Also need check if offset correct
            // ZoneText = 0x113D784
            // SubZoneText = 0x113D780
            // InGame = 0x010508A0 (return 1 or 0)
            // CONTINENT_NAME 0x12dc8e8 //0x12C67F8 //0x10A51F8
            public const uint
                // 3.2.2a
                Patch_Offset = 0x00401643; //3.1.3: 0x00401643; // this is our codecave
        }

        #endregion
    }
}
