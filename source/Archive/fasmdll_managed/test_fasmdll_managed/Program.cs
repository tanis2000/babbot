using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace test_fasmdll_managed
{
	class Program
	{
		[DllImport("kernel32")]
		private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

		[DllImport("kernel32")]
		private static extern uint VirtualAllocEx(IntPtr hProcess, uint dwAddress, int nSize, uint dwAllocationType, uint dwProtect);

		[DllImport("kernel32")]
		private static extern bool VirtualFreeEx(IntPtr hProcess, uint dwAddress, int nSize, uint dwFreeType);

		static void Main(string[] args)
		{
			Process.EnterDebugMode();
			Process[] wow = Process.GetProcessesByName("WoW");
			IntPtr hProcess = OpenProcess(0x1F0FFF, false, (uint)wow[0].Id);

			//allocate memory for codecave
			uint dwBaseAddress = VirtualAllocEx(hProcess, 0, 0x1000, 0x1000, 0x40);

			Fasm.ManagedFasm fasm = new Fasm.ManagedFasm(hProcess);
			fasm.SetMemorySize(0x500);
			//fasm.AddLine("org " + dwBaseAddress.ToString("X")); //not necessary, .Inject does automatically
			fasm.AddLine("retn");
			fasm.AddLine("jmp 0x410000");
			fasm.AddLine("call 0x410000");
			byte[] a = fasm.Assemble();

			fasm.InjectAndExecute(hProcess, dwBaseAddress);
			fasm.Dispose();

			VirtualFreeEx(hProcess, dwBaseAddress, 0, 0x8000); //release memory
		}
	}
}
