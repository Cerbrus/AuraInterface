namespace Aura.SDK.Core {
    using System;
    using System.Runtime.InteropServices;

    using Aura.SDK.Helpers;

    public abstract class AuraDLL {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate int EnumerateMbControllerPointer(IntPtr handles, int size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate void SetMbModePointer(IntPtr handle, int mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate int GetMbLedCountPointer(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate void SetMbColorPointer(IntPtr handle, byte[] colors, int size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate int EnumerateGpuControllerPointer(IntPtr handles, int size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate void SetGpuModePointer(IntPtr handle, int mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate int GetGpuLedCountPointer(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        protected delegate void SetGpuColorPointer(IntPtr handle, byte[] colors, int size);

        protected EnumerateMbControllerPointer _enumerateMbControllerPointer;
        protected SetMbModePointer _setMbModePointer;
        protected GetMbLedCountPointer _getMbLedCountPointer;
        protected SetMbColorPointer _setMbColorPointer;

        protected EnumerateGpuControllerPointer _enumerateGpuControllerPointer;
        protected SetGpuModePointer _setGpuModePointer;
        protected GetGpuLedCountPointer _getGpuLedCountPointer;
        protected SetGpuColorPointer _setGpuColorPointer;

        protected void SetPointers(IntPtr _dllHandle) {
            _enumerateMbControllerPointer = Util.GetMethod<EnumerateMbControllerPointer>(_dllHandle, "EnumerateMbController");
            _setMbModePointer = Util.GetMethod<SetMbModePointer>(_dllHandle, "SetMbMode");
            _getMbLedCountPointer = Util.GetMethod<GetMbLedCountPointer>(_dllHandle, "GetMbLedCount");
            _setMbColorPointer = Util.GetMethod<SetMbColorPointer>(_dllHandle, "SetMbColor");

            _enumerateGpuControllerPointer = Util.GetMethod<EnumerateGpuControllerPointer>(_dllHandle, "EnumerateGPU");
            _setGpuModePointer = Util.GetMethod<SetGpuModePointer>(_dllHandle, "SetGPUMode");
            _getGpuLedCountPointer = Util.GetMethod<GetGpuLedCountPointer>(_dllHandle, "GetGPULedCount");
            _setGpuColorPointer = Util.GetMethod<SetGpuColorPointer>(_dllHandle, "SetGPUColor");
        }

        internal int EnumerateMbController(IntPtr handles, int size) => _enumerateMbControllerPointer(handles, size);
        internal void SetMbMode(IntPtr handle, int mode) => _setMbModePointer(handle, mode);
        internal int GetMbLedCount(IntPtr handle) => _getMbLedCountPointer(handle);
        internal void SetMbColor(IntPtr handle, byte[] colors, int size) => _setMbColorPointer(handle, colors, size);

        internal int EnumerateGpuController(IntPtr handles, int size) => _enumerateGpuControllerPointer(handles, size);
        internal void SetGpuMode(IntPtr handle, int mode) => _setGpuModePointer(handle, mode);
        internal int GetGpuLedCount(IntPtr handle) => _getGpuLedCountPointer(handle);
        internal void SetGpuColor(IntPtr handle, byte[] colors, int size) => _setGpuColorPointer(handle, colors, size);
    }
}
