namespace AuraInterface.Helpers {
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    using AuraInterface.Shared.Models;

    /// <summary>
    /// A helper to configure the commandline commands
    /// </summary>
    public class Commands {
        /// <summary>
        /// The root command
        /// </summary>
        public RootCommand RootCommand { get; }

        /// <summary>
        /// Build the commandline commands
        /// </summary>
        /// <param name="options">The collection of parameters for this commandline configuration</param>
        /// <param name="handler">The function to call when this command is executed</param>
        public Commands(Options options, Action<string, Device?> handler) {
            RootCommand = getRootCommand(options, handler);
        }

        /// <summary>
        /// Get the root command.
        /// </summary>
        /// <param name="options">The collection of parameters for this commandline configuration</param>
        /// <param name="handler">The function to call when this command is executed</param>
        /// <returns cref="RootCommand">The root command configuration</returns>
        private RootCommand getRootCommand(
            Options options,
            Action<string, Device?> handler) {
            var rootCommand = new RootCommand("Set the RGB lighting to a specific color") {
                getDeviceSpecificCommand(options, handler, Device.Motherboard),
                getDeviceSpecificCommand(options, handler, Device.GPU)
            };

            addParametersAndHandler(rootCommand, options, handler);

            return rootCommand;
        }

        /// <summary>
        /// Get the device-specific child command
        /// </summary>
        /// <param name="options">The collection of parameters for this commandline configuration</param>
        /// <param name="handler">The function to call when this command is executed</param>
        /// <param name="device">The device to call the handler for</param>
        /// <returns cref="Command">The device-specific child command configuration</returns>
        private Command getDeviceSpecificCommand(
            Options options,
            Action<string, Device?> handler,
            Device device) {
            var command = new Command(device.ToString(), $"Set the {device}'s RGB lighting to a specific color");
            addParametersAndHandler(command, options, handler, device);

            return command;
        }

        /// <summary>
        /// Add the parameters and the handler to the command
        /// </summary>
        /// <param name="command">The command to add the parameters to</param>
        /// <param name="options">The collection of parameters for this commandline configuration</param>
        /// <param name="handler">The function to call when this command is executed</param>
        /// <param name="device">The device to call the handler for</param>
        private void addParametersAndHandler(
            Command command,
            Options options,
            Action<string, Device?> handler,
            Device? device = null) {
            command.AddOption(options.debugOption);

            if (device.HasValue) {
                command.AddOption(options.colorOption);
                command.AddOption(options.deviceOption);

                command.Handler = CommandHandler.Create<string>(color => handler(color, device));
            }
        }
    }
}
