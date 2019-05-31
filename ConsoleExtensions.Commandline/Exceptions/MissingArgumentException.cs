// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingArgumentException.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Exceptions
{
    /// <summary>
    ///     Class MissingArgumentException. Throw when a command is missing a argument.
    ///     Implements the <see cref="ConsoleExtensionException" />
    /// </summary>
    /// <seealso cref="ConsoleExtensionException" />
    public class MissingArgumentException : ConsoleExtensionException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MissingArgumentException" /> class.
        /// </summary>
        /// <param name="command">The command that was invoked.</param>
        /// <param name="argument">The argument of the missing argument.</param>
        /// <param name="arguments">The arguments in the command.</param>
        public MissingArgumentException(string command, string argument, string[] arguments)
            : base($"Missing argument {argument}")
        {
            this.Argument = argument;
            this.Arguments = arguments;
            this.Command = command;
        }

        /// <summary>
        ///     Gets the name of the missing argument.
        /// </summary>
        public string Argument { get; }

        /// <summary>
        ///     Gets the arguments in the command.
        /// </summary>
        public string[] Arguments { get; }

        /// <summary>
        ///     Gets the command that was invoked.
        /// </summary>
        public string Command { get; }
    }
}