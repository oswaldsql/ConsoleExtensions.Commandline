// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnknownCommandException.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Exceptions
{
    using System.Collections.Generic;
    using System.Linq;

    using ConsoleExtensions.Commandline.Parser;

    /// <summary>
    ///     Class UnknownCommandException. Thrown when a command was not known by the console extension.
    ///     Implements the <see cref="ConsoleExtensions.Commandline.Exceptions.ConsoleExtensionException" />
    /// </summary>
    /// <seealso cref="ConsoleExtensions.Commandline.Exceptions.ConsoleExtensionException" />
    public class UnknownCommandException : ConsoleExtensionException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnknownCommandException" /> class.
        /// </summary>
        /// <param name="command">The command that was attempted to be executed.</param>
        /// <param name="commands">The commands available in the system.</param>
        public UnknownCommandException(string command, IEnumerable<ModelCommand> commands)
            : base($"Unknown command '{command}'.")
        {
            this.Command = command;
            this.Commands = commands.ToArray();
        }

        /// <summary>
        ///     Gets the command that was attempted to be executed.
        /// </summary>
        public string Command { get; }

        /// <summary>
        ///     Gets the commands available in the system.
        /// </summary>
        public ModelCommand[] Commands { get; }
    }
}