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
        public delegate void DumpParamsDelegate(uint luaState);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Lua_RegisterDelegate(string name, IntPtr function);
        Lua_RegisterDelegate Lua_Register;

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

            // wait for host process termination...
            
            try
            {
                Lua_Register = (Lua_RegisterDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)Functions.Lua_Register, typeof(Lua_RegisterDelegate));
                DumpParamsDelegate x = DumpParams;

                IntPtr DumpParamsPtr = Marshal.GetFunctionPointerForDelegate(x);

                Interface.SetFunctionPtr(RemoteHooking.GetCurrentProcessId(), DumpParamsPtr);

                Lua_Register("DumpParams", (IntPtr)0x00401643); // This is the code hole we want to use
                Interface.OnEndScene(RemoteHooking.GetCurrentProcessId(), "Registered DumpParams()");

                while (true)
                {
                    Thread.Sleep(500);
                    
                    // transmit newly monitored file accesses...
                    if (Queue.Count > 0)
                    {
                        String[] Package = null;

                        lock (Queue)
                        {
                            Package = Queue.ToArray();

                            Queue.Clear();
                        }

                        Interface.OnEndScene(RemoteHooking.GetCurrentProcessId(), Package[0]);
                    }
                    else
                        Interface.Ping();
                }
            }
            catch (Exception e)
            {
                Interface.OnEndScene(RemoteHooking.GetCurrentProcessId(), e.ToString());
                // Ping() will raise an exception if host is unreachable
            }
        }


        #region LUA
        /*
        public static uint Lua_GetTop(uint luaState)
        {
            uint result = 0;
            uint codecave = wow.AllocateMemory();

            wow.Asm.Clear();

            wow.Asm.AddLine("push {0}", luaState);
            wow.Asm.AddLine("call {0}", Functions.Lua_GetTop);
            wow.Asm.AddLine("add esp, 0x4");

            wow.Asm.AddLine("retn");

            try
            {
                result = wow.Asm.InjectAndExecute(codecave);
                Thread.Sleep(10);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                wow.FreeMemory(codecave);
            }

            return result;
        }

        public static string Lua_ToString(uint luaState, uint idx)
        {
            uint result = 0;
            string sResult = "";
            uint codecave = wow.AllocateMemory();

            wow.Asm.Clear();

            wow.Asm.AddLine("push 0");
            wow.Asm.AddLine("push {0}", idx);
            wow.Asm.AddLine("push {0}", luaState);
            wow.Asm.AddLine("call {0}", Functions.Lua_ToString);
            wow.Asm.AddLine("add esp, 0xC");

            wow.Asm.AddLine("retn");

            try
            {
                result = wow.Asm.InjectAndExecute(codecave);
                Thread.Sleep(10);
                if (result != 0)
                {
                    sResult = wow.ReadASCIIString(result, 256);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                wow.FreeMemory(codecave);
            }

            return sResult;
        }
        */

        public void DumpParams(uint luaState)
        {
            int a = 0x12345678;
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
                Console.WriteLine(res);
            }
            */

        }
        /*
        public static void Lua_Register(string name, DumpParamsDelegate function)
        {
            uint codecave = wow.AllocateMemory();
            uint stringcave = wow.AllocateMemory(name.Length + 1);
            wow.WriteASCIIString(stringcave, name);

            wow.Asm.Clear();

            wow.Asm.AddLine("mov eax,{0}", function.Method.MethodHandle.Value);
            wow.Asm.AddLine("push eax");
            wow.Asm.AddLine("mov ecx,{0}", stringcave);
            wow.Asm.AddLine("push ecx");
            wow.Asm.AddLine("call {0}", Functions.Lua_Register);
            wow.Asm.AddLine("add esp, 0x08");

            wow.Asm.AddLine("retn");

            try
            {
                wow.Asm.InjectAndExecute(codecave);
                Thread.Sleep(10);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                wow.FreeMemory(codecave);
            }
        }
         
         */
        #endregion






        /*



        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate void DEndScene(IntPtr device);

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
    CharSet = CharSet.Unicode,
    SetLastError = true)]
        delegate IntPtr DDirect3DCreate9(ushort SDKVersion);

        // just use a P-Invoke implementation to get native API access from C# (this step is not necessary for C++.NET)
        [DllImport("d3d9.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        static extern void EndScene(IntPtr device);

        [DllImport("d3d9.dll", EntryPoint = "Direct3DCreate9",
CallingConvention = CallingConvention.StdCall),
    SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern IntPtr Direct3DCreate9(ushort SDKVersion); 

        // this is where we are intercepting all file accesses!
        static void EndScene_Hooked(IntPtr device)
        {

            try
            {
                Main This = (Main)HookRuntimeInfo.Callback;

                lock (This.Queue)
                {
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                        RemoteHooking.GetCurrentThreadId() + "]: \"" + "test" + "\"");
                }
            }
            catch
            {
            }

            // call original API...
            EndScene(device);
        }

        static IntPtr Direct3DCreate9_Hooked(ushort SDKVersion)
        {



            // call original API...
            IntPtr p = Direct3DCreate9(SDKVersion);
            
            try
            {
                Main This = (Main)HookRuntimeInfo.Callback;

                lock (This.Queue)
                {
                    This.Queue.Push("[" + RemoteHooking.GetCurrentProcessId() + ":" +
                        RemoteHooking.GetCurrentThreadId() + "]: \"" + p.ToString() + "\"");
                }
            }
            catch
            {
            }
            
            return p;
        }
    
    */
    }
}
