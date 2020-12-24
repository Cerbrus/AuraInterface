namespace Aura.SDK.Devices {
    using System;

    using Aura.SDK.Core;

    /// <summary>
    /// A <see cref="Motherboard"/> <see cref="AuraDevice"/>
    /// </summary>
    public class Motherboard: AuraDevice {
        /// <summary>
        /// Instantiate a <see cref="Motherboard"/> <see cref="AuraDevice"/>
        /// </summary>
        /// <param name="sdk">The Aura SDK wrapper</param>
        /// <param name="handle">The device's pointer</param>
        public Motherboard(AuraSDK sdk, IntPtr handle)
            : base(
                  handle,
                  sdk.SetMbMode,
                  sdk.SetMbColor,
                  sdk.GetMbLedCount) { }
    }
}
