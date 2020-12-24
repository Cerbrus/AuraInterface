namespace Aura.SDK.Devices {
    using System;
    using System.Drawing;

    using Aura.SDK.Models;

    /// <summary>
    /// A <see cref="AuraDevice"/>
    /// </summary>
    public abstract class AuraDevice {
        /// <summary>
        /// Number of color zones that can be controlled.
        /// </summary>
        public int LedCount => _ledCount;

        /// <summary>
        /// The device's pointer
        /// </summary>
        protected IntPtr _handle;

        /// <summary>
        /// Instruct the SDK to set the mode for the device.
        /// </summary>
        private readonly Action<IntPtr, int> _setMode;

        /// <summary>
        /// Instruct the SDK to set the color for the device.
        /// </summary>
        private readonly Action<IntPtr,byte[], int> _setColor;

        /// <summary>
        /// The device's led count
        /// </summary>
        protected int _ledCount;

        /// <summary>
        /// Instantiate a new <see cref="AuraDevice"/>
        /// </summary>
        /// <param name="handle">The device's pointer</param>
        internal AuraDevice(
            IntPtr handle,
            Action<IntPtr, int> setMode,
            Action<IntPtr, byte[], int> setColor,
            Func<IntPtr, int> getLedCount) {
            _handle = handle;
            _setMode = setMode;
            _setColor = setColor;
            _ledCount = getLedCount(handle);
        }

        /// <summary>
        /// Set the device's RGB mode (currently only default/automatic or software).
        /// </summary>
        /// <param name="mode"></param>
        public void SetMode(DeviceMode mode) =>
            _setMode(_handle, (int)mode);

        /// <summary>
        /// Set the device's colors. There must be the same number of colors as there are zones on the device.
        /// </summary>
        /// <param name="colors">Colors of the different zones</param>
        public void SetColors(Color[] colors) {
            if (colors.Length != LedCount) {
                throw new ArgumentException(string.Format("Argument colors must have a length of {0}, got {1}", LedCount, colors.Length));
            }

            var array = new byte[colors.Length * 3];

            for (var i = 0; i < colors.Length; i++) {
                array[i * 3] = colors[i].R;
                array[i * 3 + 1] = colors[i].B;
                array[i * 3 + 2] = colors[i].G;
            }

            _setColor(_handle, array, array.Length);
        }

        /// <summary>
        /// Set the device's colors. There must be the same number of colors as there are zones on the device times 3 bytes.
        /// </summary>
        /// <param name="colors">Colors of the different zones as bytes in RBG order</param>
        public void SetColors(byte[] colors) {
            if (colors.Length != LedCount * 3) {
                throw new ArgumentException(string.Format("Argument colors must have a length of {0}, got {1}", LedCount * 3, colors.Length));
            }

            _setColor(_handle, colors, colors.Length);
        }
    }
}