using System;
using System.Runtime.InteropServices;

namespace Dante
{
    public unsafe class IDirect3DDevice9 : IDisposable
    {
        // private IntPtr* OriginalVFTable = null;
        private IntPtr OriginalEndScene;

        public D3D9.IDirect3D9* NativeIDirect3D9 { get; private set; }
        public D3D9.IDirect3DDevice9* NativeIDirect3DDevice9 { get; private set; }

        public DelegateEndScene RealEndScene;
        public DelegateEndScene MyEndScene;


        public IDirect3DDevice9(D3D9.IDirect3D9* InNativeIDirect3D9, D3D9.IDirect3DDevice9* InNativeIDirect3DDevice9)
        {
            NativeIDirect3D9 = InNativeIDirect3D9;
            NativeIDirect3DDevice9 = InNativeIDirect3DDevice9;

            // Override the functions in NativeIDirect3DDevice9 with our own.
            OverrideFunctions();
        }

        private void OverrideFunctions()
        {
            LuaInterface.LoggingInterface.Log(string.Format("Address of Native EndScene (VFTable**) is {0:X}",
                                                            (uint) NativeIDirect3DDevice9->VFTable[0][42]));

            LuaInterface.LoggingInterface.Log("Backing up original end scene hook");
            OriginalEndScene = NativeIDirect3DDevice9->VFTable[0][42];

            RealEndScene =
                (DelegateEndScene) Marshal.GetDelegateForFunctionPointer(OriginalEndScene, typeof (DelegateEndScene));

            LuaInterface.LoggingInterface.Log(string.Format("Address of Original EndScene Backup is {0:X}",
                                                            (uint) OriginalEndScene));


            //get delegate and pointer to delegate for custom end scene
            //DelegateEndScene fuckinggarbagecollector = EndScene;

            MyEndScene = EndScene;
            IntPtr PointerToMyEndScene = Marshal.GetFunctionPointerForDelegate(MyEndScene);
            LuaInterface.LoggingInterface.Log(string.Format("Address of MyEndScene is {0:X}", (uint) PointerToMyEndScene));

            //do actual re-routing of the function to run our own custom end scene
            LuaInterface.LoggingInterface.Log("Redirect EndScene to New Function...");
            NativeIDirect3DDevice9->VFTable[0][42] = PointerToMyEndScene;
            LuaInterface.LoggingInterface.Log(string.Format("Address of New 'Native' EndScene is {0:X}",
                                                            (uint) NativeIDirect3DDevice9->VFTable[0][42]));
        }

        public void Dispose()
        {
            Dispose(false);
        }

        ~IDirect3DDevice9()
        {
            Dispose(true);
        }

        // Cleanup resources.  Destructing == true means we are getting garbage collected so don't reference any managed resources.
        private void Dispose(bool Destructing)
        {
        }

        #region Delegates

        public delegate uint DelegateEndScene(D3D9.IDirect3DDevice9 Device);

        #endregion

        public uint EndScene(D3D9.IDirect3DDevice9 Device)
        {
            lock (LuaInterface.oLocker)
            {
                //Check for pending registrations
                if (LuaInterface.PendingRegistration)
                {
                    LuaInterface.LoggingInterface.Log("EndScene() - Lua register");

                    LuaInterface.RegisterLuaInputHandler();

                    LuaInterface.PendingRegistration = false;
                }

                //check for pending do_string's
                if (!String.IsNullOrEmpty(LuaInterface.PendingDoString))
                {
                    LuaInterface.LoggingInterface.Log(string.Format("EndScene() - Lua DoString {0}",
                                                                    LuaInterface.PendingDoString));

                    LuaInterface.DoString(LuaInterface.PendingDoString);

                    LuaInterface.PendingDoString = string.Empty;
                }

                LuaInterface.LoggingInterface.Log(string.Format("{0:X} Device", (uint)&Device));
                LuaInterface.LoggingInterface.Log(string.Format("{0:X} EndScene()", (uint)OriginalEndScene));
            }

            return RealEndScene(Device);
        }
    }

    public unsafe class IDirect3D9 : IDisposable
    {
        // A pointer to the native IDirect3D9 object that we are providing overrides for.
        // A pointer to the original array of virtual functions.  We keep this around so we can call the originals.
        private IntPtr* OriginalVFTable = null;

        public DelegateCreateDevice RealCreateDevice;
        public DelegateCreateDevice MyCreateDevice;
        public D3D9.IDirect3D9* NativeIDirect3D9 { get; private set; }

        #region Construction

        // For the case where we already have a native IDirect3D9 object and we want to override some of it's functions.
        public IDirect3D9(D3D9.IDirect3D9* InNativeIDirect3D9)
        {
            NativeIDirect3D9 = InNativeIDirect3D9;

            // Override the functions in NativeIDirect3D9 with our own.
            OverrideFunctions();
        }

        // For the case where we don't have a native IDirect3D object so we want one created for us.
        public IDirect3D9(ushort SdkVersion)
        {
            // Create the real IDirect3D9 object.
            NativeIDirect3D9 = D3D9.Direct3DCreate9(SdkVersion);

            // Override the functions in NativeIDirect3D9 with our own.
            OverrideFunctions();
        }

        #endregion

        #region Destruction

        public void Dispose()
        {
            Dispose(false);
        }

        ~IDirect3D9()
        {
            Dispose(true);
        }

        // Cleanup resources.  Destructing == true means we are getting garbage collected so don't reference any managed resources.
        private void Dispose(bool Destructing)
        {
            if (OriginalVFTable != null)
            {
                Kernel32.HeapFree(Kernel32.GetProcessHeap(), 0, *OriginalVFTable);
                OriginalVFTable = null;
            }
        }

        #endregion

        #region Virtual Function Table Management

        // Backup the original native virtual function table and overwrite the pointer to it with our own (which is a copy of the original).
        private void InitializeVFTable()
        {
            // Save off the original VFTable (only if it really is the original).
            if (OriginalVFTable == null)
            {
                OriginalVFTable = NativeIDirect3D9->VFTable;
            }

            // IDirect3D9 has 17 members.
            const uint VFTableLength = 17;
            // Allocate space for our new VFTable.
            IntPtr* NewVFTable =
                (IntPtr*) Kernel32.HeapAlloc(Kernel32.GetProcessHeap(), 0, (UIntPtr) (VFTableLength*sizeof (IntPtr)));

            // Copy all of the original function pointers into our new VFTable.
            for (int i = 0; i < VFTableLength; i++)
            {
                NewVFTable[i] = OriginalVFTable[i];
            }

            // Set the Real IDirect3D9 implementation's VFTable to point at our custom one.
            NativeIDirect3D9->VFTable = NewVFTable;
        }

        // Reset the native virtual function table to point back at the original.
        private void ResetVFTable()
        {
            // If the original table is not defined do nothing.
            if (OriginalVFTable == null)
            {
                return;
            }
            // If the original table points to the same place as the current one do nothing.
            if (OriginalVFTable == NativeIDirect3D9->VFTable)
            {
                return;
            }
            // Cleanup memory allocated for our custom VFTable.
            Kernel32.HeapFree(Kernel32.GetProcessHeap(), 0, *NativeIDirect3D9->VFTable);
            // Set the VFTable back to the original.
            NativeIDirect3D9->VFTable = OriginalVFTable;
            // Set the original VFTable back to null.
            OriginalVFTable = null;
        }

        private void OverrideFunctions()
        {
            //Backup original table and replace with a copy that we will tweak
            InitializeVFTable();

            //Get and set new delegate for creating the device
            MyCreateDevice = CreateDevice;
            IntPtr PointerToMyCreateDevice = Marshal.GetFunctionPointerForDelegate(MyCreateDevice);
            NativeIDirect3D9->VFTable[16] = PointerToMyCreateDevice;
        }

        #endregion

        #region IDirect3D9 Interface Function Implementations

        #region Delegates

        public delegate uint DelegateGetAdapterCount(D3D9.IDirect3D9* This);

        public delegate uint DelegateCreateDevice(
            D3D9.IDirect3D9* This, uint adapter, uint deviceType, IntPtr focusWindow, uint behaviorFlags,
            IntPtr presentationParameters, D3D9.IDirect3DDevice9* deviceInterface);

        #endregion

        public uint CreateDevice(D3D9.IDirect3D9* This, uint adapter, uint deviceType, IntPtr focusWindow,
                                 uint behaviorFlags, IntPtr presentationParameters,
                                 D3D9.IDirect3DDevice9* deviceInterface)
        {
           
            LuaInterface.LoggingInterface.Log("CreateDevice Start...");
            RealCreateDevice =
                (DelegateCreateDevice)
                Marshal.GetDelegateForFunctionPointer(OriginalVFTable[16], typeof (DelegateCreateDevice));

            

            //Call the function to create the device.  The pointer to the device is stored in deviceInterface
            // The result code is saved to CreateDevice (0 means Good)
            LuaInterface.LoggingInterface.Log("RealCreateDevice Start...");
            uint CreateDevice = RealCreateDevice(This, adapter, deviceType, focusWindow, behaviorFlags,
                                                 presentationParameters, deviceInterface);



            //if creation was successful, then remap appopriate function pointers for EndScene
            if (CreateDevice == 0)
            {
                LuaInterface.LoggingInterface.Log(String.Format("CreateDevice = {0}", CreateDevice));
                IDirect3DDevice9 device = new IDirect3DDevice9(This, deviceInterface);
            }

            LuaInterface.LoggingInterface.Log("Returning CreateDevice");
            return CreateDevice;
        }

        #endregion

    }
}