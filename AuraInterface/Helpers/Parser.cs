namespace AuraInterface.Helpers {
    using System.CommandLine;
    using System.Drawing;

    using AuraInterface.Shared.Models;

    using CL = System.CommandLine.Parsing;

    /// <summary>
    /// The command line argument parsing helper
    /// </summary>
    public class Parser {
        /// <summary>
        /// The commandline parser
        /// </summary>
        private readonly CL.Parser _parser;

        /// <summary>
        /// The commandline parser's result
        /// </summary>
        private readonly CL.ParseResult _parseResult;

        /// <summary>
        /// The collection of parameters for this commandline configuration
        /// </summary>
        private readonly Options _options;

        /// <summary>
        /// Is debug mode enabled?
        /// </summary>
        /// <returns cref="bool">A boolean indicating whether or not debugging is enabled</returns>
        public bool IsDebug() => _parseResult.ValueForOption<bool>(_options.debugOption);

        /// <summary>
        /// Get the color option.
        /// </summary>
        /// <returns cref="Color">The parsed color value</returns>
        public Color GetColor => _parseResult.ValueForOption<Color>(_options.colorOption);

        /// <summary>
        /// Get the device option.
        /// </summary>
        /// <returns cref="Device">The parsed device value</returns>
        public Device GetDevice => _parseResult.ValueForOption<Device>(_options.deviceOption);

        /// <summary>
        /// Parse the commandline arguments
        /// </summary>
        /// <param name="rootCommand">The root command configuration</param>
        /// <param name="options">The collection of parameters for this commandline configuration</param>
        /// <param name="args">The arguments passed to the console application</param>
        public Parser(
            RootCommand rootCommand,
            Options options,
            string[] args) {
            _options = options;
            _parser = new CL.Parser(new[] { rootCommand });
            _parseResult = _parser.Parse(args);
        }
    }
}
