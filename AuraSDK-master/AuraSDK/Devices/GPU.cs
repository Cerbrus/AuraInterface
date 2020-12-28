namespace Aura.SDK.Devices {
    using System;

    using Aura.SDK.Core;

    /// <summary>
    /// A <see cref="GPU"/> <see cref="AuraDevice"/>
    /// </summary>
    public class GPU: AuraDevice {
        /// <summary>
        /// Instantiate a <see cref="GPU"/> <see cref="AuraDevice"/>
        /// </summary>
        /// <param name="sdk">The Aura SDK wrapper</param>
        /// <param name="handle">The device's pointer</param>
        public GPU(AuraSDK sdk, IntPtr handle)
            : base(
                  handle,
                  sdk.GetGpuLedCount,
                  sdk.SetGpuMode,
                  sdk.SetGpuColor) { }
    }
}
