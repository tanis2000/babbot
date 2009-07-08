using System;
using System.Runtime.InteropServices;

namespace Dante
{
    public unsafe class IDirect3DDevice9 : IDisposable
    {
        private IntPtr* OriginalVFTable = null;

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
            InitializeVFTable();

            //     STDMETHOD(QueryInterface)(THIS_ REFIID riid, void** ppvObj) PURE;
            //     STDMETHOD_(ULONG,AddRef)(THIS) PURE;
            //     STDMETHOD_(ULONG,Release)(THIS) PURE;
            //     STDMETHOD(TestCooperativeLevel)(THIS) PURE;
            //     STDMETHOD_(UINT, GetAvailableTextureMem)(THIS) PURE;
            //     STDMETHOD(EvictManagedResources)(THIS) PURE;
            //     STDMETHOD(GetDirect3D)(THIS_ IDirect3D9** ppD3D9) PURE;
            //     STDMETHOD(GetDeviceCaps)(THIS_ D3DCAPS9* pCaps) PURE;
            //     STDMETHOD(GetDisplayMode)(THIS_ UINT iSwapChain,D3DDISPLAYMODE* pMode) PURE;
            //     STDMETHOD(GetCreationParameters)(THIS_ D3DDEVICE_CREATION_PARAMETERS *pParameters) PURE;
            //     STDMETHOD(SetCursorProperties)(THIS_ UINT XHotSpot,UINT YHotSpot,IDirect3DSurface9* pCursorBitmap) PURE;
            //     STDMETHOD_(void, SetCursorPosition)(THIS_ int X,int Y,DWORD Flags) PURE;
            //     STDMETHOD_(BOOL, ShowCursor)(THIS_ BOOL bShow) PURE;
            //     STDMETHOD(CreateAdditionalSwapChain)(THIS_ D3DPRESENT_PARAMETERS* pPresentationParameters,IDirect3DSwapChain9** pSwapChain) PURE;
            //     STDMETHOD(GetSwapChain)(THIS_ UINT iSwapChain,IDirect3DSwapChain9** pSwapChain) PURE;
            //     STDMETHOD_(UINT, GetNumberOfSwapChains)(THIS) PURE;
            //     STDMETHOD(Reset)(THIS_ D3DPRESENT_PARAMETERS* pPresentationParameters) PURE;
            //     STDMETHOD(Present)(THIS_ CONST RECT* pSourceRect,CONST RECT* pDestRect,HWND hDestWindowOverride,CONST RGNDATA* pDirtyRegion) PURE;
            //     STDMETHOD(GetBackBuffer)(THIS_ UINT iSwapChain,UINT iBackBuffer,D3DBACKBUFFER_TYPE Type,IDirect3DSurface9** ppBackBuffer) PURE;
            //     STDMETHOD(GetRasterStatus)(THIS_ UINT iSwapChain,D3DRASTER_STATUS* pRasterStatus) PURE;
            //     STDMETHOD(SetDialogBoxMode)(THIS_ BOOL bEnableDialogs) PURE;
            //     STDMETHOD_(void, SetGammaRamp)(THIS_ UINT iSwapChain,DWORD Flags,CONST D3DGAMMARAMP* pRamp) PURE;
            //     STDMETHOD_(void, GetGammaRamp)(THIS_ UINT iSwapChain,D3DGAMMARAMP* pRamp) PURE;
            //     STDMETHOD(CreateTexture)(THIS_ UINT Width,UINT Height,UINT Levels,DWORD Usage,D3DFORMAT Format,D3DPOOL Pool,IDirect3DTexture9** ppTexture,HANDLE* pSharedHandle) PURE;
            //     STDMETHOD(CreateVolumeTexture)(THIS_ UINT Width,UINT Height,UINT Depth,UINT Levels,DWORD Usage,D3DFORMAT Format,D3DPOOL Pool,IDirect3DVolumeTexture9** ppVolumeTexture,HANDLE* pSharedHandle) PURE;
            //     STDMETHOD(CreateCubeTexture)(THIS_ UINT EdgeLength,UINT Levels,DWORD Usage,D3DFORMAT Format,D3DPOOL Pool,IDirect3DCubeTexture9** ppCubeTexture,HANDLE* pSharedHandle) PURE;
            //     STDMETHOD(CreateVertexBuffer)(THIS_ UINT Length,DWORD Usage,DWORD FVF,D3DPOOL Pool,IDirect3DVertexBuffer9** ppVertexBuffer,HANDLE* pSharedHandle) PURE;
            //     STDMETHOD(CreateIndexBuffer)(THIS_ UINT Length,DWORD Usage,D3DFORMAT Format,D3DPOOL Pool,IDirect3DIndexBuffer9** ppIndexBuffer,HANDLE* pSharedHandle) PURE;
            //     STDMETHOD(CreateRenderTarget)(THIS_ UINT Width,UINT Height,D3DFORMAT Format,D3DMULTISAMPLE_TYPE MultiSample,DWORD MultisampleQuality,BOOL Lockable,IDirect3DSurface9** ppSurface,HANDLE* pSharedHandle) PURE;
            //     STDMETHOD(CreateDepthStencilSurface)(THIS_ UINT Width,UINT Height,D3DFORMAT Format,D3DMULTISAMPLE_TYPE MultiSample,DWORD MultisampleQuality,BOOL Discard,IDirect3DSurface9** ppSurface,HANDLE* pSharedHandle) PURE;
            //     STDMETHOD(UpdateSurface)(THIS_ IDirect3DSurface9* pSourceSurface,CONST RECT* pSourceRect,IDirect3DSurface9* pDestinationSurface,CONST POINT* pDestPoint) PURE;
            //     STDMETHOD(UpdateTexture)(THIS_ IDirect3DBaseTexture9* pSourceTexture,IDirect3DBaseTexture9* pDestinationTexture) PURE;
            //     STDMETHOD(GetRenderTargetData)(THIS_ IDirect3DSurface9* pRenderTarget,IDirect3DSurface9* pDestSurface) PURE;
            //     STDMETHOD(GetFrontBufferData)(THIS_ UINT iSwapChain,IDirect3DSurface9* pDestSurface) PURE;
            //     STDMETHOD(StretchRect)(THIS_ IDirect3DSurface9* pSourceSurface,CONST RECT* pSourceRect,IDirect3DSurface9* pDestSurface,CONST RECT* pDestRect,D3DTEXTUREFILTERTYPE Filter) PURE;
            //     STDMETHOD(ColorFill)(THIS_ IDirect3DSurface9* pSurface,CONST RECT* pRect,D3DCOLOR color) PURE;
            //     STDMETHOD(CreateOffscreenPlainSurface)(THIS_ UINT Width,UINT Height,D3DFORMAT Format,D3DPOOL Pool,IDirect3DSurface9** ppSurface,HANDLE* pSharedHandle) PURE;
            //     STDMETHOD(SetRenderTarget)(THIS_ DWORD RenderTargetIndex,IDirect3DSurface9* pRenderTarget) PURE;
            //     STDMETHOD(GetRenderTarget)(THIS_ DWORD RenderTargetIndex,IDirect3DSurface9** ppRenderTarget) PURE;
            //     STDMETHOD(SetDepthStencilSurface)(THIS_ IDirect3DSurface9* pNewZStencil) PURE;
            //     STDMETHOD(GetDepthStencilSurface)(THIS_ IDirect3DSurface9** ppZStencilSurface) PURE;
            //     STDMETHOD(BeginScene)(THIS) PURE;
            //     STDMETHOD(EndScene)(THIS) PURE;
            DelegateEndScene MyEndScene = EndScene;
            IntPtr PointerToMyEndScene = Marshal.GetFunctionPointerForDelegate(MyEndScene);
            NativeIDirect3DDevice9->VFTable[42] = PointerToMyEndScene;

            //     STDMETHOD(Clear)(THIS_ DWORD Count,CONST D3DRECT* pRects,DWORD Flags,D3DCOLOR Color,float Z,DWORD Stencil) PURE;
            //     STDMETHOD(SetTransform)(THIS_ D3DTRANSFORMSTATETYPE State,CONST D3DMATRIX* pMatrix) PURE;
            //     STDMETHOD(GetTransform)(THIS_ D3DTRANSFORMSTATETYPE State,D3DMATRIX* pMatrix) PURE;
            //     STDMETHOD(MultiplyTransform)(THIS_ D3DTRANSFORMSTATETYPE,CONST D3DMATRIX*) PURE;
            //     STDMETHOD(SetViewport)(THIS_ CONST D3DVIEWPORT9* pViewport) PURE;
            //     STDMETHOD(GetViewport)(THIS_ D3DVIEWPORT9* pViewport) PURE;
            //     STDMETHOD(SetMaterial)(THIS_ CONST D3DMATERIAL9* pMaterial) PURE;
            //     STDMETHOD(GetMaterial)(THIS_ D3DMATERIAL9* pMaterial) PURE;
            //     STDMETHOD(SetLight)(THIS_ DWORD Index,CONST D3DLIGHT9*) PURE;
            //     STDMETHOD(GetLight)(THIS_ DWORD Index,D3DLIGHT9*) PURE;
            //     STDMETHOD(LightEnable)(THIS_ DWORD Index,BOOL Enable) PURE;
            //     STDMETHOD(GetLightEnable)(THIS_ DWORD Index,BOOL* pEnable) PURE;
            //     STDMETHOD(SetClipPlane)(THIS_ DWORD Index,CONST float* pPlane) PURE;
            //     STDMETHOD(GetClipPlane)(THIS_ DWORD Index,float* pPlane) PURE;
            //     STDMETHOD(SetRenderState)(THIS_ D3DRENDERSTATETYPE State,DWORD Value) PURE;
            //     STDMETHOD(GetRenderState)(THIS_ D3DRENDERSTATETYPE State,DWORD* pValue) PURE;
            //     STDMETHOD(CreateStateBlock)(THIS_ D3DSTATEBLOCKTYPE Type,IDirect3DStateBlock9** ppSB) PURE;
            //     STDMETHOD(BeginStateBlock)(THIS) PURE;
            //     STDMETHOD(EndStateBlock)(THIS_ IDirect3DStateBlock9** ppSB) PURE;
            //     STDMETHOD(SetClipStatus)(THIS_ CONST D3DCLIPSTATUS9* pClipStatus) PURE;
            //     STDMETHOD(GetClipStatus)(THIS_ D3DCLIPSTATUS9* pClipStatus) PURE;
            //     STDMETHOD(GetTexture)(THIS_ DWORD Stage,IDirect3DBaseTexture9** ppTexture) PURE;
            //     STDMETHOD(SetTexture)(THIS_ DWORD Stage,IDirect3DBaseTexture9* pTexture) PURE;
            //     STDMETHOD(GetTextureStageState)(THIS_ DWORD Stage,D3DTEXTURESTAGESTATETYPE Type,DWORD* pValue) PURE;
            //     STDMETHOD(SetTextureStageState)(THIS_ DWORD Stage,D3DTEXTURESTAGESTATETYPE Type,DWORD Value) PURE;
            //     STDMETHOD(GetSamplerState)(THIS_ DWORD Sampler,D3DSAMPLERSTATETYPE Type,DWORD* pValue) PURE;
            //     STDMETHOD(SetSamplerState)(THIS_ DWORD Sampler,D3DSAMPLERSTATETYPE Type,DWORD Value) PURE;
            //     STDMETHOD(ValidateDevice)(THIS_ DWORD* pNumPasses) PURE;
            //     STDMETHOD(SetPaletteEntries)(THIS_ UINT PaletteNumber,CONST PALETTEENTRY* pEntries) PURE;
            //     STDMETHOD(GetPaletteEntries)(THIS_ UINT PaletteNumber,PALETTEENTRY* pEntries) PURE;
            //     STDMETHOD(SetCurrentTexturePalette)(THIS_ UINT PaletteNumber) PURE;
            //     STDMETHOD(GetCurrentTexturePalette)(THIS_ UINT *PaletteNumber) PURE;
            //     STDMETHOD(SetScissorRect)(THIS_ CONST RECT* pRect) PURE;
            //     STDMETHOD(GetScissorRect)(THIS_ RECT* pRect) PURE;
            //     STDMETHOD(SetSoftwareVertexProcessing)(THIS_ BOOL bSoftware) PURE;
            //     STDMETHOD_(BOOL, GetSoftwareVertexProcessing)(THIS) PURE;
            //     STDMETHOD(SetNPatchMode)(THIS_ float nSegments) PURE;
            //     STDMETHOD_(float, GetNPatchMode)(THIS) PURE;
            //     STDMETHOD(DrawPrimitive)(THIS_ D3DPRIMITIVETYPE PrimitiveType,UINT StartVertex,UINT PrimitiveCount) PURE;
            //     STDMETHOD(DrawIndexedPrimitive)(THIS_ D3DPRIMITIVETYPE,INT BaseVertexIndex,UINT MinVertexIndex,UINT NumVertices,UINT startIndex,UINT primCount) PURE;
            //     STDMETHOD(DrawPrimitiveUP)(THIS_ D3DPRIMITIVETYPE PrimitiveType,UINT PrimitiveCount,CONST void* pVertexStreamZeroData,UINT VertexStreamZeroStride) PURE;
            //     STDMETHOD(DrawIndexedPrimitiveUP)(THIS_ D3DPRIMITIVETYPE PrimitiveType,UINT MinVertexIndex,UINT NumVertices,UINT PrimitiveCount,CONST void* pIndexData,D3DFORMAT IndexDataFormat,CONST void* pVertexStreamZeroData,UINT VertexStreamZeroStride) PURE;
            //     STDMETHOD(ProcessVertices)(THIS_ UINT SrcStartIndex,UINT DestIndex,UINT VertexCount,IDirect3DVertexBuffer9* pDestBuffer,IDirect3DVertexDeclaration9* pVertexDecl,DWORD Flags) PURE;
            //     STDMETHOD(CreateVertexDeclaration)(THIS_ CONST D3DVERTEXELEMENT9* pVertexElements,IDirect3DVertexDeclaration9** ppDecl) PURE;
            //     STDMETHOD(SetVertexDeclaration)(THIS_ IDirect3DVertexDeclaration9* pDecl) PURE;
            //     STDMETHOD(GetVertexDeclaration)(THIS_ IDirect3DVertexDeclaration9** ppDecl) PURE;
            //     STDMETHOD(SetFVF)(THIS_ DWORD FVF) PURE;
            //     STDMETHOD(GetFVF)(THIS_ DWORD* pFVF) PURE;
            //     STDMETHOD(CreateVertexShader)(THIS_ CONST DWORD* pFunction,IDirect3DVertexShader9** ppShader) PURE;
            //     STDMETHOD(SetVertexShader)(THIS_ IDirect3DVertexShader9* pShader) PURE;
            //     STDMETHOD(GetVertexShader)(THIS_ IDirect3DVertexShader9** ppShader) PURE;
            //     STDMETHOD(SetVertexShaderConstantF)(THIS_ UINT StartRegister,CONST float* pConstantData,UINT Vector4fCount) PURE;
            //     STDMETHOD(GetVertexShaderConstantF)(THIS_ UINT StartRegister,float* pConstantData,UINT Vector4fCount) PURE;
            //     STDMETHOD(SetVertexShaderConstantI)(THIS_ UINT StartRegister,CONST int* pConstantData,UINT Vector4iCount) PURE;
            //     STDMETHOD(GetVertexShaderConstantI)(THIS_ UINT StartRegister,int* pConstantData,UINT Vector4iCount) PURE;
            //     STDMETHOD(SetVertexShaderConstantB)(THIS_ UINT StartRegister,CONST BOOL* pConstantData,UINT  BoolCount) PURE;
            //     STDMETHOD(GetVertexShaderConstantB)(THIS_ UINT StartRegister,BOOL* pConstantData,UINT BoolCount) PURE;
            //     STDMETHOD(SetStreamSource)(THIS_ UINT StreamNumber,IDirect3DVertexBuffer9* pStreamData,UINT OffsetInBytes,UINT Stride) PURE;
            //     STDMETHOD(GetStreamSource)(THIS_ UINT StreamNumber,IDirect3DVertexBuffer9** ppStreamData,UINT* pOffsetInBytes,UINT* pStride) PURE;
            //     STDMETHOD(SetStreamSourceFreq)(THIS_ UINT StreamNumber,UINT Setting) PURE;
            //     STDMETHOD(GetStreamSourceFreq)(THIS_ UINT StreamNumber,UINT* pSetting) PURE;
            //     STDMETHOD(SetIndices)(THIS_ IDirect3DIndexBuffer9* pIndexData) PURE;
            //     STDMETHOD(GetIndices)(THIS_ IDirect3DIndexBuffer9** ppIndexData) PURE;
            //     STDMETHOD(CreatePixelShader)(THIS_ CONST DWORD* pFunction,IDirect3DPixelShader9** ppShader) PURE;
            //     STDMETHOD(SetPixelShader)(THIS_ IDirect3DPixelShader9* pShader) PURE;
            //     STDMETHOD(GetPixelShader)(THIS_ IDirect3DPixelShader9** ppShader) PURE;
            //     STDMETHOD(SetPixelShaderConstantF)(THIS_ UINT StartRegister,CONST float* pConstantData,UINT Vector4fCount) PURE;
            //     STDMETHOD(GetPixelShaderConstantF)(THIS_ UINT StartRegister,float* pConstantData,UINT Vector4fCount) PURE;
            //     STDMETHOD(SetPixelShaderConstantI)(THIS_ UINT StartRegister,CONST int* pConstantData,UINT Vector4iCount) PURE;
            //     STDMETHOD(GetPixelShaderConstantI)(THIS_ UINT StartRegister,int* pConstantData,UINT Vector4iCount) PURE;
            //     STDMETHOD(SetPixelShaderConstantB)(THIS_ UINT StartRegister,CONST BOOL* pConstantData,UINT  BoolCount) PURE;
            //     STDMETHOD(GetPixelShaderConstantB)(THIS_ UINT StartRegister,BOOL* pConstantData,UINT BoolCount) PURE;
            //     STDMETHOD(DrawRectPatch)(THIS_ UINT Handle,CONST float* pNumSegs,CONST D3DRECTPATCH_INFO* pRectPatchInfo) PURE;
            //     STDMETHOD(DrawTriPatch)(THIS_ UINT Handle,CONST float* pNumSegs,CONST D3DTRIPATCH_INFO* pTriPatchInfo) PURE;
            //     STDMETHOD(DeletePatch)(THIS_ UINT Handle) PURE;
            //     STDMETHOD(CreateQuery)(THIS_ D3DQUERYTYPE Type,IDirect3DQuery9** ppQuery) PURE;
        }

        private void InitializeVFTable()
        {
            if (NativeIDirect3D9 == null || NativeIDirect3DDevice9 == null)
            {
                return;
            }

            // Save off the original VFTable (only if it really is the original).
            if (OriginalVFTable == null)
            {
                OriginalVFTable = NativeIDirect3DDevice9->VFTable;
            }

            const uint VFTableLength = 119;
            // Allocate space for our new VFTable.
            IntPtr* NewVFTable =
                (IntPtr*) Kernel32.HeapAlloc(Kernel32.GetProcessHeap(), 0, (UIntPtr) (VFTableLength*sizeof (IntPtr)));

            // Copy all of the original function pointers into our new VFTable.
            for (int i = 0; i < VFTableLength; i++)
            {
                NewVFTable[i] = OriginalVFTable[i];
            }

            // Set the Real IDirect3D9 implementation's VFTable to point at our custom one.
            NativeIDirect3DDevice9->VFTable = NewVFTable;
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
            if (OriginalVFTable == NativeIDirect3DDevice9->VFTable)
            {
                return;
            }
            // Cleanup memory allocated for our custom VFTable.
            Kernel32.HeapFree(Kernel32.GetProcessHeap(), 0, *NativeIDirect3DDevice9->VFTable);
            // Set the VFTable back to the original.
            NativeIDirect3DDevice9->VFTable = OriginalVFTable;
            // Set the original VFTable back to null.
            OriginalVFTable = null;
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
            if (OriginalVFTable != null)
            {
                Kernel32.HeapFree(Kernel32.GetProcessHeap(), 0, *OriginalVFTable);
                OriginalVFTable = null;
            }
        }

        #region Delegates

        public delegate uint DelegateEndScene(D3D9.IDirect3DDevice9 Device);

        #endregion

        public uint EndScene(D3D9.IDirect3DDevice9 Device)
        {
            // to do
            // [...]
            Log.Debug("EndScene()");
            DelegateEndScene RealEndScene =
                (DelegateEndScene) Marshal.GetDelegateForFunctionPointer(OriginalVFTable[42], typeof (DelegateEndScene));
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
            // If we don't have a real IDirect3D9 object yet then do nothing.
            if (NativeIDirect3D9 == null)
            {
                return;
            }

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
            InitializeVFTable();

            // #0: STDMETHOD(QueryInterface)(THIS_ REFIID riid, void** ppvObj) PURE;
            // #1: STDMETHOD_(ULONG,AddRef)(THIS) PURE;
            // #2: STDMETHOD_(ULONG,Release)(THIS) PURE;
            // TODO: Override this and Dispose this object if it is going to return 0.
            // #3: STDMETHOD(RegisterSoftwareDevice)(THIS_ void* pInitializeFunction) PURE;
            // #4: STDMETHOD_(UINT, GetAdapterCount)(THIS) PURE;

            DelegateGetAdapterCount MyAdapterCount = GetAdapterCount;
            IntPtr PointerToMyAdapterCount = Marshal.GetFunctionPointerForDelegate(MyAdapterCount);
            NativeIDirect3D9->VFTable[4] = PointerToMyAdapterCount;

            // #5: STDMETHOD(GetAdapterIdentifier)(THIS_ UINT Adapter,DWORD Flags,D3DADAPTER_IDENTIFIER9* pIdentifier) PURE;
            // #6: STDMETHOD_(UINT, GetAdapterModeCount)(THIS_ UINT Adapter,D3DFORMAT Format) PURE;
            // #7: STDMETHOD(EnumAdapterModes)(THIS_ UINT Adapter,D3DFORMAT Format,UINT Mode,D3DDISPLAYMODE* pMode) PURE;
            // #8: STDMETHOD(GetAdapterDisplayMode)(THIS_ UINT Adapter,D3DDISPLAYMODE* pMode) PURE;
            // #9: STDMETHOD(CheckDeviceType)(THIS_ UINT Adapter,D3DDEVTYPE DevType,D3DFORMAT AdapterFormat,D3DFORMAT BackBufferFormat,BOOL bWindowed) PURE;
            // #10: STDMETHOD(CheckDeviceFormat)(THIS_ UINT Adapter,D3DDEVTYPE DeviceType,D3DFORMAT AdapterFormat,DWORD Usage,D3DRESOURCETYPE RType,D3DFORMAT CheckFormat) PURE;
            // #11: STDMETHOD(CheckDeviceMultiSampleType)(THIS_ UINT Adapter,D3DDEVTYPE DeviceType,D3DFORMAT SurfaceFormat,BOOL Windowed,D3DMULTISAMPLE_TYPE MultiSampleType,DWORD* pQualityLevels) PURE;
            // #12: STDMETHOD(CheckDepthStencilMatch)(THIS_ UINT Adapter,D3DDEVTYPE DeviceType,D3DFORMAT AdapterFormat,D3DFORMAT RenderTargetFormat,D3DFORMAT DepthStencilFormat) PURE;
            // #13: STDMETHOD(CheckDeviceFormatConversion)(THIS_ UINT Adapter,D3DDEVTYPE DeviceType,D3DFORMAT SourceFormat,D3DFORMAT TargetFormat) PURE;
            // #14: STDMETHOD(GetDeviceCaps)(THIS_ UINT Adapter,D3DDEVTYPE DeviceType,D3DCAPS9* pCaps) PURE;
            // #15: STDMETHOD_(HMONITOR, GetAdapterMonitor)(THIS_ UINT Adapter) PURE;
            // #16: STDMETHOD(CreateDevice)(THIS_ UINT Adapter,D3DDEVTYPE DeviceType,HWND hFocusWindow,DWORD BehaviorFlags,D3DPRESENT_PARAMETERS* pPresentationParameters,IDirect3DDevice9** ppReturnedDeviceInterface) PURE;

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
            Log.Debug("CreateDevice Start...");
            DelegateCreateDevice RealCreateDevice =
                (DelegateCreateDevice)
                Marshal.GetDelegateForFunctionPointer(OriginalVFTable[16], typeof (DelegateCreateDevice));
            uint CreateDevice = RealCreateDevice(This, adapter, deviceType, focusWindow, behaviorFlags,
                                                 presentationParameters, deviceInterface);

            if (CreateDevice == 0)
            {
                // to do
                // IDirect3DDevice9 device = new IDirect3DDevice9(This, deviceInterface); 
                IDirect3DDevice9 device = new IDirect3DDevice9(This, deviceInterface);
            }

            return CreateDevice;
        }

        public uint GetAdapterCount(D3D9.IDirect3D9* This)
        {
            DelegateGetAdapterCount RealGetAdapterCount =
                (DelegateGetAdapterCount)
                Marshal.GetDelegateForFunctionPointer(OriginalVFTable[4], typeof (DelegateGetAdapterCount));
            uint AdapterCount = RealGetAdapterCount(This);
            return AdapterCount;
        }

        #endregion

        public D3D9.IDirect3D9* NativeIDirect3D9 { get; private set; }
        //public D3D9.IDirect3DDevice9* NativeIDirect3DDevice9 { get; private set; }
    }
}