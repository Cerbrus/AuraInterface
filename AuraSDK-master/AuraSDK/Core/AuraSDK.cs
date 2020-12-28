namespace Aura.SDK.Core {
    // https://github.com/nicoco007/AuraSDK

    using System;
    using System.IO;
    using System.Linq;

    using Aura.SDK.Devices;
    using Aura.SDK.Helpers;

    using AuraInterface.Shared.Helpers;

    /// <summary>
    /// The Aura SDK wrapper
    /// </summary>
    public class AuraSDK: AuraDLL {
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

        /// <summary>
        /// Creates a new instance of the SDK class
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
        /// Reloads all controllers
        /// </summary>
        public void Reload() {
            Unload();
            load(_dllPath);
        }

        /// <summary>
        /// Load the Aura SDK DLL for the specified <paramref name="path"/>
        /// </summary>
        /// <param name="path">The DLL path</param>
        private void load(string path) {
            if (string.IsNullOrEmpty(path)) {
                throw new ArgumentNullException("Path cannot be null or empty");
            }

            var fileName = Path.GetFileName(path);
            var directory = Path.GetDirectoryName(path);

            if (!File.Exists(path)) {
                throw new FileNotFoundException(path + " not found. Current working directory: " + Directory.GetCurrentDirectory());
            }

            _dllPath = path;

            NativeMethods.SetDllDirectory(
                string.IsNullOrEmpty(directory)
                    ? Directory.GetCurrentDirectory()
                    : directory);

            _dllHandle = NativeMethods.LoadLibrary(fileName);

            SetPointers(_dllHandle);

            Motherboards = loadDevices<Motherboard>(EnumerateMbController);
            GPUs = loadDevices<GPU>(EnumerateGpuController);
        }

        /// <summary>
        /// Load the <see cref="AuraDevice"/>s of the specified <typeparamref name="TDevice"/> type
        /// </summary>
        /// <typeparam name="TDevice">The type of <see cref="AuraDevice"/></typeparam>
        /// <param name="enumerator">The DLL method used to enumerate the <see cref="AuraDevice"/>s</param>
        /// <returns cref="AuraDevice[]">A list of found devices</returns>
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
        /// Unloads the SDK, removing all references to the DLL
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
    }
}
