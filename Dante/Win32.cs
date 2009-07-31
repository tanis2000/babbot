using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Dante
{
    internal class Kernel32
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcessHeap();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool HeapFree(IntPtr hHeap, uint dwFlags, IntPtr lpMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

        public enum Protection : uint
        {
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400
        }
    }

    public unsafe class D3D9
    {
        [DllImport("d3d9.dll", EntryPoint = "Direct3DCreate9", CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurity]
        public static extern IDirect3D9* Direct3DCreate9(ushort SDKVersion);

        [DllImport("d3d9.dll", EntryPoint = "Direct3DCreate9Ex", CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurity]
        public static extern int Direct3DCreate9Ex(ushort SDKVersion, [Out] out IDirect3D9Ex ex);

        #region Nested type: IDirect3DDevice9

        [StructLayout(LayoutKind.Sequential, Pack=4)]
        public struct IDirect3DDevice9
        {
            public IntPtr** VFTable;
        }

        #endregion

        #region Nested type: IDirect3D9

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct IDirect3D9
        {
            public IntPtr* VFTable;
        }

        #endregion

        #region Nested type: IDirect3D9Ex

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct IDirect3D9Ex
        {
            public IntPtr* VFTable;
        }

        #endregion
    }
}