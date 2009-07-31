using System;
using System.Runtime.InteropServices;

namespace Dante
{
    public unsafe class IDirect3DDevice9 : IDisposable
    {
        private IntPtr* OriginalVFTable = null;
        private IntPtr OriginalEndScene;

        public D3D9.IDirect3D9* NativeIDirect3D9 { get; private set; }
        public D3D9.IDirect3DDevice9* NativeIDirect3DDevice9 { get; private set; }


        public IDirect3DDevice9(D3D9.IDirect3D9* InNativeIDirect3D9, D3D9.IDirect3DDevice9* InNativeIDirect3DDevice9)
        {
            NativeIDirect3D9 = InNativeIDirect3D9;
            NativeIDirect3DDevice9 = InNativeIDirect3DDevice9;

            // Override the functions in NativeIDirect3DDevice9 with our own.
            OverrideFunctions();
        }

        private void OverrideFunctions()
        {
            //InitializeVFTable();

            LuaInterface.LoggingInterface.Log(string.Format("Address of Native EndScene (VFTable**) is {0}", NativeIDirect3DDevice9->VFTable[0][42]));

            LuaInterface.LoggingInterface.Log("Backing up original end scene hook");
            OriginalEndScene = NativeIDirect3DDevice9->VFTable[0][42];

            LuaInterface.LoggingInterface.Log(string.Format("Address of Original EndScene Backup is {0}", OriginalEndScene));


            //get delegate and pointer to delegate for custon end scene
            DelegateEndScene MyEndScene = EndScene;
            IntPtr PointerToMyEndScene = Marshal.GetFunctionPointerForDelegate(MyEndScene);

            LuaInterface.LoggingInterface.Log(string.Format("Address of MyEndScene is {0}", PointerToMyEndScene));

            //LuaInterface.LoggingInterface.Log("Unprotecting EndScene Memory...");
            //uint newProtection = (uint)Kernel32.Protection.PAGE_EXECUTE_READWRITE;
            //uint oldProtection;
            //Kernel32.VirtualProtect(NativeIDirect3DDevice9->VFTable[42], (uint)sizeof(IntPtr), newProtection, out oldProtection);

            //LuaInterface.LoggingInterface.Log(string.Format("Old Protection Method Was: {0}", (Kernel32.Protection)oldProtection));

            //do actual re-routing of the function to run our own custom end scene
            LuaInterface.LoggingInterface.Log("Redirect EndScene to New Function...");
            NativeIDirect3DDevice9->VFTable[0][42] = PointerToMyEndScene;

            LuaInterface.LoggingInterface.Log(string.Format("Address of New 'Native' EndScene is {0}", NativeIDirect3DDevice9->VFTable[0][42]));


            //LuaInterface.LoggingInterface.Log("Changing Memory protection back to what it was...");
            //Kernel32.VirtualProtect(NativeIDirect3DDevice9->VFTable[42], (uint)sizeof(IntPtr), oldProtection, out newProtection);

        }

        private void InitializeVFTable()
        {
            if (NativeIDirect3D9 == null || NativeIDirect3DDevice9 == null)
            {
                return;
            }

            IntPtr* inner = (IntPtr*)NativeIDirect3DDevice9;

            // Save off the original VFTable (only if it really is the original).
            if (OriginalVFTable == null)
            {
                LuaInterface.LoggingInterface.Log("Backing up original VFTable....");
                OriginalVFTable = inner;
            }

            const uint VFTableLength = 119;

            //contents of first element of jagged array
            IntPtr* NewVFTable = (IntPtr*)Kernel32.HeapAlloc(Kernel32.GetProcessHeap(), 0, (UIntPtr)(VFTableLength * sizeof(IntPtr)));

            LuaInterface.LoggingInterface.Log("Copying original values to New table.");

            // Copy all of the original function pointers into our new VFTable.
            for (int i = 0; i < VFTableLength; i++)
            {
                //first copy pointer address
                NewVFTable[i] = inner[i];
            }

            LuaInterface.LoggingInterface.Log(string.Format("Address of IDirect3DDevice9 is {0}", (int)NativeIDirect3DDevice9));
            LuaInterface.LoggingInterface.Log(string.Format("Address of NewVFTable is {0}", (int)&NewVFTable));

            // Set the Real IDirect3D9 implementation's VFTable to point at our custom one.
            //Marshal.WriteIntPtr((IntPtr)NativeIDirect3DDevice9, (IntPtr)NewVFTable);

            //NativeIDirect3DDevice9 = NewVFTable;

            //LuaInterface.LoggingInterface.Log("Copy Complete...");
        }

        //public static IntPtr** IntPtrJaggedArrayToPointer(IntPtr[][] array)
        //{
        //    fixed (IntPtr* arrayPtr = array[0])
        //    {
        //        IntPtr*[] ptrArray = new IntPtr*[array.Length];
        //        for (int i = 0; i < array.Length; i++)
        //        {
        //            fixed (IntPtr* ptr = array[i])
        //                ptrArray[i] = ptr;
        //        }

        //        fixed (IntPtr** ptr = ptrArray)
        //            return ptr;
        //    }
        //}

        //// Reset the native virtual function table to point back at the original.
        //private void ResetVFTable()
        //{
        //    // If the original table is not defined do nothing.
        //    if (OriginalVFTable == null)
        //    {
        //        return;
        //    }
        //    // If the original table points to the same place as the current one do nothing.
        //    if (OriginalVFTable == NativeIDirect3DDevice9->VFTable)
        //    {
        //        return;
        //    }
        //    // Cleanup memory allocated for our custom VFTable. Possible Slight Memory leak here...
        //    Kernel32.HeapFree(Kernel32.GetProcessHeap(), 0, *NativeIDirect3DDevice9->VFTable);
        //    //Kernel32.HeapFree(Kernel32.GetProcessHeap(), 0, *NativeIDirect3DDevice9->VFTable[0]);
        //    // Set the VFTable back to the original.
        //    NativeIDirect3DDevice9->VFTable = OriginalVFTable;
        //    // Set the original VFTable back to null.
        //    OriginalVFTable = null;
        //}

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
            //if (OriginalVFTable != null)
            //{
            //    Kernel32.HeapFree(Kernel32.GetProcessHeap(), 0, *OriginalVFTable);
            //    OriginalVFTable = null;
            //}
        }

        #region Delegates

        public delegate uint DelegateEndScene(D3D9.IDirect3DDevice9 Device);

        #endregion
        private int iCounter = 0;

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
                    LuaInterface.LoggingInterface.Log(string.Format("EndScene() - Lua DoString {0}", LuaInterface.PendingDoString));

                    LuaInterface.DoString(LuaInterface.PendingDoString);

                    LuaInterface.PendingDoString = string.Empty;
                }
            }

            if (iCounter == 0)
            {
                LuaInterface.LoggingInterface.Log("EndScene()");
                iCounter = 1;
            }

            DelegateEndScene RealEndScene =
                (DelegateEndScene)Marshal.GetDelegateForFunctionPointer(OriginalEndScene, typeof(DelegateEndScene));
            return RealEndScene(Device);
        }
    }

    public unsafe class IDirect3D9 : IDisposable
    {
        // A pointer to the native IDirect3D9 object that we are providing overrides for.
        // A pointer to the original array of virtual functions.  We keep this around so we can call the originals.
        private IntPtr* OriginalVFTable = null;

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
            DelegateCreateDevice MyCreateDevice = CreateDevice;
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
            
            DelegateCreateDevice RealCreateDevice = 
                (DelegateCreateDevice)Marshal.GetDelegateForFunctionPointer(OriginalVFTable[16], typeof (DelegateCreateDevice));

            LuaInterface.LoggingInterface.Log("RealCreateDevice Start...");
            //Call the function to create the device.  The pointer to the device is stored in deviceInterface
            // The result code is saved to CreateDevice (0 means Good)
            uint CreateDevice = RealCreateDevice(This, adapter, deviceType, focusWindow, behaviorFlags,
                                                 presentationParameters, deviceInterface);


            LuaInterface.LoggingInterface.Log(String.Format("CreateDevice = {0}", CreateDevice));

            //if creation was successful, then remap appopriate function pointers for EndScene
            if (CreateDevice == 0)
            {
                IDirect3DDevice9 device = new IDirect3DDevice9(This, deviceInterface);
            }

            LuaInterface.LoggingInterface.Log("Returning...");
            return CreateDevice;
        }

        #endregion

        public D3D9.IDirect3D9* NativeIDirect3D9 { get; private set; }
        //public D3D9.IDirect3DDevice9* NativeIDirect3DDevice9 { get; private set; }
    }
}