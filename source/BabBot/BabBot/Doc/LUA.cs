using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Lua
{
    class Lua
    {
        private uint State;
        private int LastArg;
        private Magic.BlackMagic _magic;

        private Process _processHandle;
        private Process ProcessHandle
        {
            get
            {
                if (_processHandle == null)
                {
                    _processHandle = Process.GetProcessById(_magic.ProcessId);
                }
                return _processHandle;
            }
        }

        private IntPtr _mainThread;
        private IntPtr MainThread
        {
            get
            {
                if (_mainThread == IntPtr.Zero)
                {
                    _mainThread = ThreadSusspender.GetWowsMainThread(ProcessHandle);
                }
                return _mainThread;
            }
        }

        private uint CurrentManager
        {
            get
            {
                return _magic.ReadUInt(_magic.ReadUInt((uint)Offsets.ClientConnection) + (uint)Offsets.ClientManager);
            }
        }

        private enum Offsets : uint
        {
            ClientConnection = 0x011CA310,
            ClientManager = 0x000028A4,

            LuaDoString = 0x0077E060,
            LuaGetTop = 0x007AD6E0,
            LuaPushString = 0x007ADE10,
            LuaToNumber = 0x007ADB40,
            LuaToInteger = 0x007ADB80,
            LuaToString = 0x007ADBF0,
            LuaToUserdata = 0x007ADD20,
            LuaToBoolean = 0x007ADBC0,
            LuaGetState = 0x0077CCF0,
            LuaThreadLock = 0x012EA444,
        }

        public int CurrentArgument
        {
            get
            {
                if (LastArg == 0)
                {
                    return 1;
                }
                return LastArg;
            }
            set
            {
                LastArg = value;
            }
        }

        public Lua(Magic.BlackMagic magic)
        {
            _magic = magic;
            State = GetState();
            LastArg = GetTop();
        }

        public void DoString(string lua)
        {
            Synchronize();
            uint codeCave = _magic.AllocateMemory(0x2048);

            _magic.WriteASCIIString(codeCave + 0x1024, lua);

            _magic.Asm.Clear();
            AsmUpdateCurrentManager();

            //_magic.Asm.AddLine("push {0}", State);
            _magic.Asm.AddLine("push {0}", 0);
            _magic.Asm.AddLine("mov eax, {0}", codeCave + 0x1024);
            _magic.Asm.AddLine("push eax");
            _magic.Asm.AddLine("push eax");
            _magic.Asm.AddLine("call {0}", (uint) Offsets.LuaDoString);
            _magic.Asm.AddLine("add esp, 0xC");
            _magic.Asm.AddLine("retn");

            _magic.Asm.InjectAndExecute(codeCave);
            _magic.FreeMemory(codeCave);
            ResumeMainThread();
        }

        public void CallMethod(uint method)
        {
            Synchronize();
            uint codeCave = _magic.AllocateMemory(0x1048);
            _magic.Asm.Clear();
            AsmUpdateCurrentManager();
            _magic.Asm.AddLine("push {0}", State);
            _magic.Asm.AddLine("call {0}", method);
            _magic.Asm.AddLine("add esp, 0x4");
            _magic.Asm.AddLine("retn");

            _magic.Asm.InjectAndExecute(codeCave);
            _magic.FreeMemory(codeCave);
            ResumeMainThread();
        }

        public int GetTop()
        {
            uint codeCave = _magic.AllocateMemory(0x1048);
            _magic.Asm.Clear();
            _magic.Asm.AddLine("push {0}", State);
            _magic.Asm.AddLine("call {0}", (uint) Offsets.LuaGetTop);
            _magic.Asm.AddLine("add esp, 0x4");
            _magic.Asm.AddLine("retn");

            uint result = _magic.Asm.InjectAndExecute(codeCave);
            _magic.FreeMemory(codeCave);
            return (int)result;
        }

        public void PushString(string str)
        {
            uint codeCave = _magic.AllocateMemory(0x2048);
            _magic.Asm.Clear();

            _magic.WriteASCIIString(codeCave + 0x1024, str);
            
            _magic.Asm.AddLine("push {0}", str.Length);
            _magic.Asm.AddLine("push {0}", codeCave + 0x1024);
            _magic.Asm.AddLine("push {0}", State);
            _magic.Asm.AddLine("call {0}", (uint) Offsets.LuaPushString);
            _magic.Asm.AddLine("add esp, 0xC");
            _magic.Asm.AddLine("retn");

            _magic.Asm.InjectAndExecute(codeCave);
            _magic.FreeMemory(codeCave);

            LastArg++;
        }

        public double ToNumber()
        {
            return ToNumber(GetArgument());
        }

        public double ToNumber(int argument)
        {
            uint codeCave = _magic.AllocateMemory(0x2048);

            _magic.Asm.Clear();
            _magic.Asm.AddLine("push {0}", argument);
            _magic.Asm.AddLine("push {0}", State);
            _magic.Asm.AddLine("call {0}", (uint)Offsets.LuaToNumber);
            _magic.Asm.AddLine("add esp, 0x8");
            _magic.Asm.AddLine("retn");

            uint result = _magic.Asm.InjectAndExecute(codeCave);
            double dbl;

            // TODO Not sure this is correct.
            try
            {
                dbl = _magic.ReadDouble(result);
            }
            catch (Exception e)
            {
                dbl = 0;
            }
            _magic.FreeMemory(codeCave);
            return (double) dbl;
        }

        public int ToInteger()
        {
            return ToInteger(GetArgument());
        }

        public int ToInteger(int argument)
        {
            uint codeCave = _magic.AllocateMemory(0x2048);

            _magic.Asm.Clear();
            _magic.Asm.AddLine("push {0}", argument);
            _magic.Asm.AddLine("push {0}", State);
            _magic.Asm.AddLine("call {0}", (uint)Offsets.LuaToInteger);
            _magic.Asm.AddLine("add esp, 0x8");
            _magic.Asm.AddLine("retn");

            uint result = _magic.Asm.InjectAndExecute(codeCave);
            _magic.FreeMemory(codeCave);
            return (int) result;
        }

        public override string ToString()
        {
            return ToString(GetArgument(), 50);
        }

        public string ToString(int argument, int length)
        {
            uint codeCave = _magic.AllocateMemory(0x2048);

            _magic.Asm.Clear();
            _magic.Asm.AddLine("push 0");
            _magic.Asm.AddLine("push {0}", argument);
            _magic.Asm.AddLine("push {0}", State);
            _magic.Asm.AddLine("call {0}", (uint)Offsets.LuaToString);
            _magic.Asm.AddLine("add esp, 0xC");
            _magic.Asm.AddLine("retn");

            uint result = _magic.Asm.InjectAndExecute(codeCave);
            _magic.FreeMemory(codeCave);

            try
            {
                return _magic.ReadASCIIString(result, length);
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public object ToUserdata()
        {
            return ToUserdata(GetArgument());
        }

        public object ToUserdata(int argument)
        {
            uint codeCave = _magic.AllocateMemory(0x1024);

            _magic.Asm.Clear();
            _magic.Asm.AddLine("push {0}", argument);
            _magic.Asm.AddLine("push {0}", State);
            _magic.Asm.AddLine("call {0}", (uint) Offsets.LuaToUserdata);
            _magic.Asm.AddLine("add esp, 0x8");
            _magic.Asm.AddLine("retn");

            uint result = _magic.Asm.InjectAndExecute(codeCave);
            _magic.FreeMemory(codeCave);
            // Untested
            return _magic.ReadObject(result, typeof(object));
        }

        public bool ToBoolean()
        {
            return ToBoolean(GetArgument());
        }

        public bool ToBoolean(int argument)
        {
            uint codeCave = _magic.AllocateMemory(0x2048);

            _magic.Asm.Clear();
            _magic.Asm.AddLine("push {0}", argument);
            _magic.Asm.AddLine("push {0}", State);
            _magic.Asm.AddLine("call {0}", (uint) Offsets.LuaToBoolean);
            _magic.Asm.AddLine("add esp, 0x8");
            _magic.Asm.AddLine("retn");

            uint result = _magic.Asm.InjectAndExecute(codeCave);
            _magic.FreeMemory(codeCave);
            return (result == 1 ? true : false);
        }

        public uint GetState()
        {
            uint codeCave = _magic.AllocateMemory(0x1048);

            _magic.Asm.Clear();
            _magic.Asm.AddLine("call {0}", (uint)Offsets.LuaGetState);
            _magic.Asm.AddLine("retn");

            uint result = _magic.Asm.InjectAndExecute(codeCave);
            _magic.FreeMemory(codeCave);
            return result;
        }

        private int GetArgument()
        {
            if (LastArg < GetTop())
            {
                LastArg++;
            }
            return LastArg;
        }

        private void ResumeMainThread()
        {
            ThreadSusspender.ResumeThread(MainThread);
        }

        private void AsmUpdateCurrentManager()
        {
            _magic.Asm.AddLine("mov EDX, {0}", CurrentManager);
            _magic.Asm.AddLine("FS mov EAX, [0x2C]");
            _magic.Asm.AddLine("mov EAX, [EAX]");
            _magic.Asm.AddLine("add EAX, 8");
            _magic.Asm.AddLine("mov [EAX], edx");
        }

        private void Synchronize()
        {
            while (_magic.ReadInt((uint)Offsets.LuaThreadLock) != 0)
            {
                Thread.Sleep(0);
            }
            ThreadSusspender.SuspendThread(MainThread);
        }
    }

    // CREDITS NOT TO ME
    class ThreadSusspender
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll")]
        private static extern bool Thread32First(IntPtr hSnapshot, ref THREADENTRY32 lppe);

        [DllImport("kernel32.dll")]
        private static extern bool Thread32Next(IntPtr hSnapshot, ref THREADENTRY32 lppe);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("ntdll.dll")]
        private static extern uint NtQueryInformationThread(IntPtr handle, uint infclass, ref THREAD_BASIC_INFORMATION info, uint length, IntPtr bytesread);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern uint GetSecurityInfo(
        IntPtr handle,
        _SE_OBJECT_TYPE ObjectType,
        SECURITY_INFORMATION SecurityInfo,
        out IntPtr pSidOwner,
        out IntPtr pSidGroup,
        out IntPtr pDacl,
        out IntPtr pSacl,
        out IntPtr pSecurityDescriptor);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern uint SetSecurityInfo(
        IntPtr handle,
        _SE_OBJECT_TYPE ObjectType,
        SECURITY_INFORMATION SecurityInfo,
        out IntPtr pSidOwner,
        out IntPtr pSidGroup,
        out IntPtr pDacl,
        out IntPtr pSacl,
        out IntPtr pSecurityDescriptor);

        enum SECURITY_INFORMATION
        {
            DACL_SECURITY_INFORMATION,
            LABEL_SECURITY_INFORMATION,
            GROUP_SECURITY_INFORMATION,
            OWNER_SECURITY_INFORMATION,
            PROTECTED_DACL_SECURITY_INFORMATION,
            PROTECTED_SACL_SECURITY_INFORMATION,
            SACL_SECURITY_INFORMATION,
            UNPROTECTED_DACL_SECURITY_INFORMATION,
            UNPROTECTED_SACL_SECURITY_INFORMATION
        };

        enum _SE_OBJECT_TYPE
        {
            SE_UNKNOWN_OBJECT_TYPE = 0,
            SE_FILE_OBJECT,
            SE_SERVICE,
            SE_PRINTER,
            SE_REGISTRY_KEY,
            SE_LMSHARE,
            SE_KERNEL_OBJECT,
            SE_WINDOW_OBJECT,
            SE_DS_OBJECT,
            SE_DS_OBJECT_ALL,
            SE_PROVIDER_DEFINED_OBJECT,
            SE_WMIGUID_OBJECT,
            SE_REGISTRY_WOW64_32KEY,
            SE_OBJECT_TYPE,
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct THREADENTRY32
        {
            public UInt32 dwSize;
            public UInt32 cntUsage;
            public UInt32 th32ThreadID;
            public UInt32 th32OwnerProcessID;
            public Int32 tpBasePri;
            public Int32 tpDeltaPri;
            public UInt32 dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct THREAD_BASIC_INFORMATION
        {
            public bool ExitStatus;
            public IntPtr TebBaseAddress;
            public uint processid;
            public uint threadid;
            public uint AffinityMask;
            public uint Priority;
            public uint BasePriority;
        }

        private const uint TH32CS_SNAPTHREAD = 0x00000004;
        private const uint THREAD_SUSPEND_RESUME = 0x0002;

        public static IntPtr GetWowsMainThread(Process process)
        {
            uint THREAD_QUERY_INFORMATION = 0x1F03FF;
            IntPtr snaphandle = IntPtr.Zero;
            IntPtr threadhandle = IntPtr.Zero;

            snaphandle = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);
            if (snaphandle != null)
            {
                THREADENTRY32 info = new THREADENTRY32();
                info.dwSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(THREADENTRY32));
                bool morethreads = true;
                bool found = false;
                if (Thread32First(snaphandle, ref info))
                {
                    while (morethreads && !found)
                    {
                        if (info.th32OwnerProcessID == process.Id)
                        {
                            threadhandle = OpenThread(THREAD_QUERY_INFORMATION, false, info.th32ThreadID);
                            if (threadhandle != null)
                            {
                                THREAD_BASIC_INFORMATION tbi = new THREAD_BASIC_INFORMATION();
                                NtQueryInformationThread(threadhandle, 0, ref tbi, (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(THREAD_BASIC_INFORMATION)), IntPtr.Zero);

                                if (tbi.processid == process.Id)
                                {
                                    IntPtr pSidOwner, pSidGroup, pDacl, pSacl, pSecurityDescriptor;
                                    GetSecurityInfo(threadhandle, _SE_OBJECT_TYPE.SE_UNKNOWN_OBJECT_TYPE, SECURITY_INFORMATION.UNPROTECTED_DACL_SECURITY_INFORMATION, out pSidOwner, out pSidGroup, out pDacl, out pSacl, out pSecurityDescriptor);
                                    SetSecurityInfo(threadhandle, _SE_OBJECT_TYPE.SE_UNKNOWN_OBJECT_TYPE, SECURITY_INFORMATION.UNPROTECTED_DACL_SECURITY_INFORMATION, out pSidOwner, out pSidGroup, out pDacl, out pSacl, out pSidOwner);
                                    break;
                                }
                            }
                        }

                        info.dwSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(THREADENTRY32));
                        morethreads = Thread32Next(snaphandle, ref info);
                    }
                }
                CloseHandle(snaphandle);
            }

            return threadhandle;
        }

    }
}
