using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels.Http;
using EasyHook;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Permissions;

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
                Lua_GetState = 0x00499700, // 3.1.3
                Patch_Offset = 0x00401643; // 3.1.3 this is our codecave
        }
        
        // Common IPC interface
        private DanteInterface Interface;
        // Communication queue
        private Stack<String> Queue = new Stack<String>();

        // Our lua_State
        private static uint L;

        // Temp test msg
        private static string msg;

        public static IntPtr WowProcessHandle;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
           byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
           uint dwProcessId);

        private static int BytesWritten;


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
        public static IntPtr CommandHandlerPtr = Marshal.GetFunctionPointerForDelegate(CommandHandler);

        public static List<string> Values = new List<string>();

        public Main(
            RemoteHooking.IContext InContext,
            IntPtr processHandle)
        {
            Log.Debug("Main()");
            WowProcessHandle = processHandle;
            Log.Debug(string.Format("ProcessHandle: {0}", WowProcessHandle));
        }

        public void Run(
            RemoteHooking.IContext InContext,
            IntPtr processHandle)
        {
            Log.Debug("Run()");
            msg = "";
            // wait for host process termination...
            
            try
            {
                // Start the server stuff
                Log.Debug(string.Format("Waking up process.."));
                RemoteHooking.WakeUpProcess();

                Dictionary<string, string> props = new Dictionary<string, string>();  
                props.Add("authorizedGroup", "Everyone");
                props.Add("portName", "localhost:8123");  
                props.Add("exclusiveAddressUse", "false");
                //IPC port name
                Log.Debug(string.Format("Creating new channel.."));
                IpcChannel ipcCh = new IpcChannel(props, null, null);

                Log.Debug(string.Format("Registering Service.."));
                ChannelServices.RegisterChannel(ipcCh, false);

                Log.Debug(string.Format("IPC Channel Name: {0}", ipcCh.ChannelName));
                Log.Debug(string.Format("IPC Channel Priority: {0}.", ipcCh.ChannelPriority));
                ChannelDataStore channelData = (ChannelDataStore)ipcCh.ChannelData;
                foreach (string uri in channelData.ChannelUris)
                {
                    Log.Debug(string.Format("Channel URI is {0}.", uri));
                }

                RemotingConfiguration.RegisterWellKnownServiceType
                   (typeof(Dante.DanteInterface),
                           "DanteRemoteObj",
                           WellKnownObjectMode.SingleCall);

                string[] urls = ipcCh.GetUrlsForUri("DanteRemoteObj");
                if (urls.Length > 0)
                {
                    string objectUrl = urls[0];
                    string objectUri;
                    string channelUri = ipcCh.Parse(objectUrl, out objectUri);
                    Log.Debug(string.Format("The object URI is {0}.", objectUri));
                    Log.Debug(string.Format("The channel URI is {0}.", channelUri));
                    Log.Debug(string.Format("The object URL is {0}.", objectUrl));
                }


                Log.Debug(string.Format("Registering LUA functions.."));
                Lua_Register = (Lua_RegisterDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_Register, typeof(Lua_RegisterDelegate));
                Lua_GetTop = (Lua_GetTopDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_GetTop, typeof(Lua_GetTopDelegate));
                Lua_ToString = (Lua_ToStringDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_ToString, typeof(Lua_ToStringDelegate));
                Lua_GetState = (Lua_GetStateDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_GetState, typeof(Lua_GetStateDelegate));
                Lua_DoString = (Lua_DoStringDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_DoString, typeof(Lua_DoStringDelegate));

                // Init the lua_State
                L = Lua_GetState();
                Log.Debug(string.Format("GetState returned {0:X}", L));
                

                //Log.Debug(string.Format("Patching WoW.. CommandHandler: {0:X}", (uint)CommandHandlerPtr));
                //int bw = SetFunctionPtr(CommandHandlerPtr);
                //Log.Debug(string.Format("Bytes written: {0}", bw));


                Log.Debug(string.Format("Registering our InputHandler.."));
                Lua_Register("InputHandler", (IntPtr)Functions.Patch_Offset); // This is the code hole we want to use

                while (true)
                {
                    Thread.Sleep(25);
                }
            }
            catch (Exception e)
            {
                //Interface.SendMessage(RemoteHooking.GetCurrentProcessId(), e.ToString());
                // Ping() will raise an exception if host is unreachable
                Log.Debug(e.ToString());
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


        #region LUA

        public static void DoString(string command)
        {
            Lua_DoString(string.Format("InputHandler({0})", command), "BabBot.lua", 0);
        }

        public static int InputHandler(uint luaState)
        {
            msg = "dump!";

            Values.Clear();
            uint n = Lua_GetTop(luaState);
            //Log.Debug(string.Format("LUA_State: {0:X}", luaState));
            //Log.Debug(string.Format("Vars num: {0}", n));
            for (uint i = 1; i <= n; i++)
            {
                string res = Lua_ToString(luaState, i, 0);
                Values.Add(res);
            }
            return 0;
        }

        public static int SetFunctionPtr(IntPtr pointer)
        {
            bool ReturnVal;
            uint p = (uint)pointer - Functions.Patch_Offset - 5;
            byte[] buf = new byte[4];
            byte[] buf2 = new byte[1];
            buf2[0] = 0xE9;
            buf[3] = (byte)((p & 0xFF000000) >> 24);
            buf[2] = (byte)((p & 0xFF0000) >> 16);
            buf[1] = (byte)((p & 0xFF00) >> 8);
            buf[0] = (byte)((p & 0xFF));

            IntPtr hProcess = GetCurrentProcess(); // OpenProcess(ProcessAccessFlags.All, false, (UInt32)proc[0].Id);

            Log.Debug(string.Format("SetFunctionPtr -> hProcess = {0:X}", (uint)hProcess));

            ReturnVal = WriteProcessMemory(hProcess, (IntPtr)Functions.Patch_Offset, buf2, 1, out BytesWritten);
            ReturnVal = WriteProcessMemory(hProcess, (IntPtr)(Functions.Patch_Offset+1), buf, 4, out BytesWritten);
            return BytesWritten;
        }

        public static int RestoreFunctionPtr()
        {
            bool ReturnVal;
            byte[] buf = new byte[5];
            buf[0] = buf[1] = buf[2] = buf[3] = buf[4] = 0xCC;

            IntPtr hProcess = GetCurrentProcess(); // OpenProcess(ProcessAccessFlags.All, false, (UInt32)proc[0].Id);

            Log.Debug(string.Format("RestoreFunctionPtr -> hProcess = {0:X}", (uint)hProcess));

            ReturnVal = WriteProcessMemory(hProcess, (IntPtr)Functions.Patch_Offset, buf, 5, out BytesWritten);
            return BytesWritten;
        }

        #endregion


    }
}
