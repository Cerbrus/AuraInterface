namespace AuraInterface.Shared.Helpers {
    using System;
    using System.IO;

    /// <summary>
    /// The IO manager
    /// </summary>
    public class IO {
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
        /// The console's default foreground color
        /// </summary>
        private readonly ConsoleColor _defaultBackground = Console.BackgroundColor;

        /// <summary>
        /// The console's default background color
        /// </summary>
        private readonly ConsoleColor _defaultForeground = Console.ForegroundColor;

        /// <summary>
        /// Initialize the IO helper
        /// </summary>
        /// <param name="isDebug">A function that returns a boolean indicating whether or not debugging is enabled</param>
        public IO(Func<bool> isDebug) {
            _isDebug = isDebug;
        }

        #region Write(Line)

        /// <summary>
        /// Writes the specified string value to the standard output stream
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="IOException">An I/O error occurred</exception>
        /// <returns cref="IO">This instance, for chaining</returns>
        public IO Write(string value) {
            if (Debug) {
                Console.Write(value);
            }

            return this;
        }

        /// <summary>
        /// Writes the current line terminator to the standard output stream
        /// </summary>
        /// <exception cref="IOException">An I/O error occurred</exception>
        /// <returns cref="IO">This instance, for chaining</returns>
        public IO WriteLine() {
            if (Debug) {
                Console.WriteLine();
            }

            return this;
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurred</exception>
        /// <param name="value">The value to write</param>
        /// <returns cref="IO">This instance, for chaining</returns>
        public IO WriteLine(string value) {
            if (Debug) {
                Console.WriteLine(value);
            }

            return this;
        }

        #endregion Write(Line)

        #region Read(Line)

        /// <summary>
        /// Reads the next line of characters from the standard input stream.
        /// </summary>
        /// <returns>The next line of characters from the input stream, or null if no more lines are available.</returns>
        /// <exception cref="IOException">An I/O error occurred</exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string</exception>
        /// <exception cref="ArgumentOutOfRangeException">The number of characters in the next line of characters is greater than <see cref="int.MaxValue"/></exception>
        public string ReadLine() => 
            Debug
                ? Console.ReadLine()
                : null;

        #endregion Read(Line)

        #region Errors

        /// <summary>
        /// Writes an exception string followed by a line terminator to the text string or stream
        /// </summary>
        /// <param name="messages">The strings to write. If value is null, only the line terminator is written.</param>
        /// <exception cref="ObjectDisposedException">The System.IO.TextWriter is closed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        /// <returns cref="IO">This instance, for chaining</returns>
        public IO Exception(params string[] messages) =>
            Exception(false, messages);

        /// <summary>
        /// Writes an exception string followed by a line terminator to the text string or stream
        /// </summary>
        /// <param name="always">Log this message even when debug is disabled</param>
        /// <param name="messages">The strings to write. If value is null, only the line terminator is written.</param>
        /// <exception cref="ObjectDisposedException">The System.IO.TextWriter is closed</exception>
        /// <exception cref="IOException">An I/O error occurs</exception>
        /// <returns cref="IO">This instance, for chaining</returns>
        public IO Exception(bool always, params string[] messages) {
            if (Debug || always) {
                Console.Error.WriteLine(string.Join("\n", messages));
            }

            return this;
        }

        #endregion Errors

        #region Color

        /// <summary>
        /// Set the colors to use in the console
        /// </summary>
        /// <param name="foreground">The foreground color</param>
        /// <param name="background">The background color</param>
        /// <returns cref="IO">This instance, for chaining</returns>
        public IO SetColor(ConsoleColor foreground, ConsoleColor background = ConsoleColor.Black) {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;

            return this;
        }

        /// <summary>
        /// Reset the console colors to the original value
        /// </summary>
        /// <returns cref="IO">This instance, for chaining</returns>
        public IO ResetColor() {
            Console.BackgroundColor = _defaultBackground;
            Console.ForegroundColor = _defaultForeground;

            return this;
        }

        /// <summary>
        /// Set the console to a specified set of <see cref="ConsoleColor"/>s, and execute the passed action. Then, reset the colors
        /// </summary>
        /// <param name="actions">The actions to perform</param>
        /// <param name="foreground">The foreground color</param>
        /// <param name="background">The background color</param>
        /// <returns></returns>
        public IO WithColor(Action actions, ConsoleColor foreground, ConsoleColor background = ConsoleColor.Black) {
            SetColor(foreground, background);

            actions();

            return ResetColor();
        }

        #endregion Color
    }
}
