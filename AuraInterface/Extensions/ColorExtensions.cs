namespace AuraInterface.Extensions {
    using System.Drawing;
    using System.Linq;

    using Aura.SDK.Devices;

    /// <summary>
    /// Extensions for the <see cref="Color"/> class.
    /// </summary>
    public static class ColorExtensions {
        /// <summary>
        /// Converts the color to an array of colors for the passed device
        /// </summary>
        /// <param name="color">The color to convert to an array</param>
        /// <param name="device">The device to call the handler for</param>
        /// <returns cref="Color[]">An array of colors of the length required for the device</returns>
        public static Color[] ToArray(this Color color, AuraDevice device) =>
            Enumerable
                .Repeat(color, device.LedCount)
                .ToArray();

        /// <summary>
        /// Convert the color to an RGB string
        /// </summary>
        /// <param name="color">The color to convert to an string</param>
        /// <returns cref="string">The color in "R, G, B" format</returns>
        public static string ToRGBString(this Color color) =>
            $"{color.R}, {color.G}, {color.B}";
    }
}
