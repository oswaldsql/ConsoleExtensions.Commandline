// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleExtensionException.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Exceptions
{
    using System;

    /// <summary>
    ///     Class ConsoleExtensionException. Serves as a base class for exceptions thrown by console extension.
    ///     Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public abstract class ConsoleExtensionException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConsoleExtensionException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected ConsoleExtensionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConsoleExtensionException" /> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in
        ///     Visual Basic) if no inner exception is specified.
        /// </param>
        protected ConsoleExtensionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}