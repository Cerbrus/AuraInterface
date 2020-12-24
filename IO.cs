namespace AuraInterface.Shared {
    using System;
    using System.IO;

    public class IO {
        /// <summary>
        /// Gets the standard input stream.
        /// </summary>
        /// <returns cref="TextReader">A System.IO.TextReader that represents the standard input stream</returns>
        public TextReader In => Debug ? Console.In : null;

        /// <summary>
        /// Gets the standard output stream.
        /// </summary>
        /// <returns cref="TextWriter">A System.IO.TextWriter that represents the standard output stream</returns>
        public TextWriter Out => Debug ? Console.Out : null;

        /// <summary>
        /// Gets the standard error output stream.
        /// </summary>
        /// <returns cref="TextWriter">A System.IO.TextWriter that represents the standard error output stream</returns>
        public TextWriter Error => Debug ? Console.Error : null;

        /// <summary>
        /// A function that returns a boolean indicating whether or not debugging is enabled
        /// </summary>
        private readonly Func<bool> _isDebug;

        /// <summary>
        /// Is debug mode enabled?
        /// </summary>
        /// <returns cref="bool">A boolean indicating whether or not debugging is enabled</returns>
        private bool Debug => _isDebug();

        /// <summary>
        /// Initialize the IO helper
        /// </summary>
        /// <param name="isDebug">A function that returns a boolean indicating whether or not debugging is enabled</param>
        public IO(Func<bool> isDebug) {
            _isDebug = isDebug;
        }

        /// <summary>
        /// Writes the specified string value to the standard output stream
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="IOException">An I/O error occurred</exception>
        public void Write(string value) {
            if (Debug) {
                Console.Write(value);
            }
        }

        /// <summary>
        /// Writes the current line terminator to the standard output stream
        /// </summary>
        /// <exception cref="IOException">An I/O error occurred</exception>
        public void WriteLine() {
            if (Debug) {
                Console.WriteLine();
            }
        }
    }
}
