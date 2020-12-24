namespace AuraInterface.Helpers {
    using System.CommandLine;

    using AuraInterface.Shared.Models;

    /// <summary>
    /// The collection of parameters for this commandline configuration
    /// </summary>
    public class Options {
        /// <summary>
        /// The "--debug" option
        /// </summary>
        public Option debugOption;

        /// <summary>
        /// The "--color" option
        /// </summary>
        public Option colorOption;

        /// <summary>
        /// The "--device" option
        /// </summary>
        public Option deviceOption;

        /// <summary>
        /// Build the collection of parameters for this commandline configuration
        /// </summary>
        public Options() {
            debugOption = buildDebugOption();
            colorOption = buildColorOption();
            deviceOption = buildDeviceOption();
        }

        #region Builders

        /// <summary>
        /// Build the "--debug" option configuration
        /// </summary>
        /// <returns cref="Option">The "--debug" option configuration</returns>
        private Option buildDebugOption() =>
            buildOption<bool>("Enable debug logging", "--debug");

        /// <summary>
        /// Build the "--color" option configuration
        /// </summary>
        /// <returns cref="Option">The "--color" option configuration</returns>
        private Option buildColorOption() =>
            buildOption<string>("The color to set the device to", "--color", "-c");

        /// <summary>
        /// Build the "--device" option configuration
        /// </summary>
        /// <returns cref="Option">The "--device" option configuration</returns>
        private Option buildDeviceOption() =>
            buildOption<Device>("The device to update", "--device", "-d");

        /// <summary>
        /// Build an option configuration
        /// </summary>
        /// <typeparam name="TValue">The option value's type</typeparam>
        /// <param name="description">The option's description</param>
        /// <param name="aliases">The option's aliases</param>
        /// <returns cref="Option">The configured option</returns>
        private Option buildOption<TValue>(string description, params string[] aliases) =>
            new Option(aliases, description) {
                Argument = new Argument<TValue>()
            };

        #endregion Builders
    }
}
