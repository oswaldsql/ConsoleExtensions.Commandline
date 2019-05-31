// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnknownOptionException.cs" company="Lasse Sjørup">
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
    ///     Class UnknownOptionException. Thrown when the user tries to set a option that is not known in the system.
    ///     Implements the <see cref="ConsoleExtensions.Commandline.Exceptions.ConsoleExtensionException" />
    /// </summary>
    /// <seealso cref="ConsoleExtensions.Commandline.Exceptions.ConsoleExtensionException" />
    public class UnknownOptionException : ConsoleExtensionException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnknownOptionException" /> class.
        /// </summary>
        /// <param name="option">The option that was attempted to be set.</param>
        /// <param name="options">The options available in the system.</param>
        public UnknownOptionException(string option, IEnumerable<ModelOption> options)
            : base($"Unknown option '{option}'.")
        {
            this.Option = option;
            this.Options = options.ToArray();
        }

        /// <summary>
        ///     Gets the option that was attempted to be set.
        /// </summary>
        public string Option { get; }

        /// <summary>
        ///     Gets the options available in the system.
        /// </summary>
        public ModelOption[] Options { get; }
    }
}