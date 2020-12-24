namespace Aura.SDK.Core {
    // https://github.com/nicoco007/AuraSDK

    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    using Aura.SDK.Devices;
    using Aura.SDK.Helpers;

    using AuraInterface.Shared.Helpers;

    /// <summary>
    /// The Aura SDK wrapper
    /// </summary>
    public class AuraSDK {
        /// <summary>
        /// Array of found motherboard controllers
        /// </summary>
        public Motherboard[] Motherboards { get; private set; }

        /// <summary>
        /// Array of found GPU controllers
        /// </summary>
        public GPU[] GPUs { get; private set; }

        /// <summary>
        /// The DLL pointer
        /// </summary>
        private IntPtr _dllHandle = IntPtr.Zero;

        /// <summary>
        /// The DLL path
        /// </summary>
        private string _dllPath = "AURA_SDK.dll";

        /// <summary>
        /// The IO manager
        /// </summary>
        private readonly IO _io;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int EnumerateMbControllerPointer(IntPtr handles, int size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetMbModePointer(IntPtr handle, int mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetMbLedCountPointer(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetMbColorPointer(IntPtr handle, byte[] colors, int size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int EnumerateGpuControllerPointer(IntPtr handles, int size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetGpuModePointer(IntPtr handle, int mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetGpuLedCountPointer(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetGpuColorPointer(IntPtr handle, byte[] colors, int size);

        private EnumerateMbControllerPointer _enumerateMbControllerPointer;
        private SetMbModePointer _setMbModePointer;
        private GetMbLedCountPointer _getMbLedCountPointer;
        private SetMbColorPointer _setMbColorPointer;

        private EnumerateGpuControllerPointer _enumerateGpuControllerPointer;
        private SetGpuModePointer _setGpuModePointer;
        private GetGpuLedCountPointer _getGpuLedCountPointer;
        private SetGpuColorPointer _setGpuColorPointer;

        /// <summary>
        /// Creates a new instance of the SDK class.
        /// </summary>
        /// <param name="io">The IO helper</param>
        public AuraSDK(IO io) 
            : this(io, null) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">The path to load the DLL from</param>
        /// <param name="io">The IO helper</param>
        public AuraSDK(IO io, string path) {
            _io = io;
            load(path ?? _dllPath);
        }

        /// <summary>
        /// Reloads all controllers.
        /// </summary>
        public void Reload() {
            Unload();
            load(_dllPath);
        }

        private void load(string path) {
            if (string.IsNullOrEmpty(path)) {
                throw new ArgumentNullException("Path cannot be null or empty");
            }

            var fileName = Path.GetFileName(path);
            var directory = Path.GetDirectoryName(path);

            if (!File.Exists(path)) {
                throw new FileNotFoundException(path + " not found");
            }

            _dllPath = path;

            NativeMethods.SetDllDirectory(
                string.IsNullOrEmpty(directory)
                    ? Directory.GetCurrentDirectory()
                    : directory);

            _dllHandle = NativeMethods.LoadLibrary(fileName);

            _enumerateMbControllerPointer = Util.GetMethod<EnumerateMbControllerPointer>(_dllHandle, "EnumerateMbController");
            _setMbModePointer = Util.GetMethod<SetMbModePointer>(_dllHandle, "SetMbMode");
            _getMbLedCountPointer = Util.GetMethod<GetMbLedCountPointer>(_dllHandle, "GetMbLedCount");
            _setMbColorPointer = Util.GetMethod<SetMbColorPointer>(_dllHandle, "SetMbColor");

            _enumerateGpuControllerPointer = Util.GetMethod<EnumerateGpuControllerPointer>(_dllHandle, "EnumerateGPU");
            _setGpuModePointer = Util.GetMethod<SetGpuModePointer>(_dllHandle, "SetGPUMode");
            _getGpuLedCountPointer = Util.GetMethod<GetGpuLedCountPointer>(_dllHandle, "GetGPULedCount");
            _setGpuColorPointer = Util.GetMethod<SetGpuColorPointer>(_dllHandle, "SetGPUColor");

            Motherboards = loadDevices<Motherboard>(EnumerateMbController);
            GPUs = loadDevices<GPU>(EnumerateGpuController);
        }

        private TDevice[] loadDevices<TDevice>(Func<IntPtr, int, int> enumerator)
            where TDevice : AuraDevice {
            var deviceType = typeof(TDevice);

            var controllerCount = 0;

            try {
                controllerCount = enumerator(IntPtr.Zero, 0);
            } catch {
            } finally {
                if (controllerCount == 0) {
                    _io.Exception($"No {deviceType.Name} controllers detected.");
                }
            }
            if (controllerCount == 0) {
                return null;
            }

            _io.WriteLine($"{controllerCount} {deviceType.Name} controller(s) detected.");

            var handles = Util.ArrayFromPointer(controllerCount, pointer => enumerator(pointer, controllerCount));

            return Enumerable.Range(0, controllerCount)
                 .Select(i => (TDevice)Activator.CreateInstance(deviceType, new object[] { this, handles[i] }))
                 .ToArray();
        }

        /// <summary>
        /// Unloads the SDK, removing all references to the DLL.
        /// </summary>
        public void Unload() {
            if (_dllHandle == IntPtr.Zero) {
                return;
            }

            while (NativeMethods.FreeLibrary(_dllHandle)) { }

            _dllHandle = IntPtr.Zero;

            Motherboards = new Motherboard[0];
            GPUs = new GPU[0];
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
