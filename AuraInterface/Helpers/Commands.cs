namespace AuraInterface.Helpers {
    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.Drawing;

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
        /// <param name="setColor">The setter function to call when this command is executed</param>
        /// <param name="getColor">The getter function to call when this command is executed</param>
        public Commands(
            Options options,
            Action<string, Device?> setColor,
            Action getColor) {
            RootCommand = getRootCommand(options, setColor, getColor);
        }

        /// <summary>
        /// Get the root command.
        /// </summary>
        /// <param name="options">The collection of parameters for this commandline configuration</param>
        /// <param name="setColor">The setter function to call when this command is executed</param>
        /// <param name="getColor">The getter function to call when this command is executed</param>
        /// <returns cref="System.CommandLine.RootCommand">The root command configuration</returns>
        private RootCommand getRootCommand(
            Options options,
            Action<string, Device?> setColor,
            Action getColor) {
            var rootCommand = new RootCommand("Set the RGB lighting to a specific color") {
                Handler = null,
            };

            rootCommand.Add(getDeviceSpecificCommand(options, setColor, Device.Motherboard));
            rootCommand.Add(getDeviceSpecificCommand(options, setColor, Device.GPU));

            rootCommand.AddOption(options.debugOption);
            rootCommand.AddOption(options.getColorOption);
            rootCommand.Handler = CommandHandler.Create(getColor);

            return rootCommand;
        }

        /// <summary>
        /// Get the device-specific child command
        /// </summary>
        /// <param name="options">The collection of parameters for this commandline configuration</param>
        /// <param name="setColor">The setter function to call when this command is executed</param>
        /// <param name="device">The device to call the handler for</param>
        /// <returns cref="Command">The device-specific child command configuration</returns>
        private Command getDeviceSpecificCommand(
            Options options,
            Action<string, Device?> setColor,
            Device device) {
            var command = new Command(device.ToString(), $"Set the {device}'s RGB lighting to a specific color"){
                Handler = CommandHandler.Create<string>(color => setColor(color, device))
            };

            command.AddOption(options.debugOption);
            command.AddOption(options.colorOption);
            command.AddOption(options.deviceOption);

            return command;
        }        
    }
}
