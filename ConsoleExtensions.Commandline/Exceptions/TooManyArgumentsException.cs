// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TooManyArgumentsException.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Exceptions
{
    /// <summary>
    ///     Class TooManyArgumentsException. Thrown is too many arguments was specified for at command.
    ///     Implements the <see cref="ConsoleExtensions.Commandline.Exceptions.ConsoleExtensionException" />
    /// </summary>
    /// <seealso cref="ConsoleExtensions.Commandline.Exceptions.ConsoleExtensionException" />
    public class TooManyArgumentsException : ConsoleExtensionException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TooManyArgumentsException" /> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public TooManyArgumentsException(string[] arguments)
            : base("Too many arguments.")
        {
            this.Arguments = arguments;
        }

        /// <summary>
        ///     Gets the arguments available.
        /// </summary>
        public string[] Arguments { get; }
    }
}