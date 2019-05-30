// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidArgumentFormatException.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Exceptions
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     Class InvalidArgumentFormatException.
    ///     Implements the <see cref="ConsoleExtensions.Commandline.Exceptions.ConsoleExtensionException" />
    /// </summary>
    /// <seealso cref="ConsoleExtensions.Commandline.Exceptions.ConsoleExtensionException" />
    public class InvalidArgumentFormatException : ConsoleExtensionException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidArgumentFormatException" /> class.
        /// </summary>
        /// <param name="stringValue">The string value the user tried to set.</param>
        /// <param name="property">The property that was attempted to be set.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidArgumentFormatException(string stringValue, PropertyInfo property, Exception innerException)
            : base($"Invalid argument format. '{property.Name}' can not be set to '{stringValue}'", innerException)
        {
            this.StringValue = stringValue;
            this.Property = property;
        }

        /// <summary>
        ///     Gets the property that was attempted to be set.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        ///     Gets the string value the user tried to set.
        /// </summary>
        public string StringValue { get; }
    }
}