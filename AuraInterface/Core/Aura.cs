namespace AuraInterface.Core {
    using System;
    using System.CommandLine;
    using System.Drawing;
    using System.Linq;

    using AuraInterface.Extensions;
    using AuraInterface.Helpers;
    using AuraInterface.Shared.Helpers;
    using AuraInterface.Shared.Models;

    using global::Aura.SDK.Core;
    using global::Aura.SDK.Devices;
    using global::Aura.SDK.Models;

    using Newtonsoft.Json;

    /// <summary>
    /// A <see cref="AuraSDK"/> wrapper.
    /// </summary>
    public class Aura {
        /// <summary>
        /// A reference to the <see cref="Motherboard"/>
        /// </summary>
        private Motherboard Motherboard => _sdk.Motherboards?.Length > 0
            ? _sdk.Motherboards[0]
            : null;

        /// <summary>
        /// A reference to the <see cref="GPU"/>
        /// </summary>
        private GPU GPU => _sdk.GPUs?.Length > 0
            ? _sdk.GPUs[0]
            : null;

        /// <summary>
        /// The Aura SDK
        /// </summary>
        private readonly AuraSDK _sdk;

        /// <summary>
        /// The IO manager
        /// </summary>
        private readonly IO _io;

        /// <summary>
        /// The collection of parameters for this commandline configuration
        /// </summary>
        private readonly Options _options;

        /// <summary>
        /// The helper to configure the commandline commands
        /// </summary>
        private readonly Commands _commands;

        /// <summary>
        /// The command line argument parsing helper
        /// </summary>
        private readonly Parser _parser;

        /// <summary>
        /// The main entrypoint of the Aura Interface.
        /// </summary>
        /// <param name="args"></param>
        public Aura(string[] args) {
            _options = new Options();
            _commands = new Commands(_options, setColor, getColor);
            _parser = new Parser(_commands.RootCommand, _options, args);

            _io = new IO(_parser.IsDebug);
            _sdk = new AuraSDK(_io);

            _commands.RootCommand.Invoke(args);

            _io.ReadLine();
        }

        #region Get color

        /// <summary>
        /// Get the <see cref="Color"/> of the <see cref="Motherboard"/> <see cref="AuraDevice"/>
        /// </summary>
        private void getColor() {
            if (Motherboard == null) {
                _io.Exception(true, "No motherboards have been found");
                return;
            }

            var colors = Motherboard.GetColors();
            var htmlColors = colors.Select(ColorTranslator.ToHtml);

            var json = JsonConvert.SerializeObject(htmlColors);
            _io.WriteLine(json, true);
        }

        #endregion Get color

        #region Set color

        /// <summary>
        /// Set the <see cref="Color"/> of the <see cref="AuraDevice"/> specified by <paramref name="device"/>
        /// </summary>
        /// <param name="color">The color to use</param>
        /// <param name="device">The specified <see cref="Device"/></param>
        private void setColor(string color, Device? device = Device.Motherboard) {
            Color parsedColor = default;

            try {
                parsedColor = color.StartsWith("#")
                    ? ColorTranslator.FromHtml(color)
                    : Color.FromName(color);
            } catch (Exception) {
                _io.Exception(true, $"Invalid color: `{color}`");
            }

            setColor(parsedColor, device);
        }

        /// <summary>
        /// Set the <see cref="Color"/> of the <see cref="AuraDevice"/> specified by <paramref name="device"/>
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to use</param>
        /// <param name="device">The specified <see cref="Device"/></param>
        private void setColor(Color color, Device? device = Device.Motherboard) {
            var targetDevice = getTargetDevice(device);
            if (targetDevice == null) {
                _io.Exception(true, "The specified device could not be found.");
                return;
            }

            targetDevice.SetMode(DeviceMode.Software);
            targetDevice.SetColors(color.ToArray(targetDevice));

            _io.Write($"Set \"{device}\" to [{color.ToRGBString()}]");
            if (color.IsNamedColor) {
                _io.Write($" ({color.Name})");

                if (Enum.TryParse(color.Name, out ConsoleColor consoleColor)) {
                    _io.Write(" ")
                        .WithColor(() => _io.Write(color.Name), consoleColor, consoleColor);
                }
            }
            _io.WriteLine();
        }

        #endregion Set color

        /// <summary>
        /// Get the target device
        /// </summary>
        /// <param name="device">The specified <see cref="Device"/></param>
        /// <returns cref="AuraDevice">The requested device</returns>
        private AuraDevice getTargetDevice(Device? device = Device.Motherboard) {
            switch (device) {
                case Device.Motherboard: return Motherboard;
                case Device.GPU: return GPU;
                default:
                    _io.Exception(true, "Incorrect device specified.");
                    return null;
            }
        }
    }
}
