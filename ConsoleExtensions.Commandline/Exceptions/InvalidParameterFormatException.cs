// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidParameterFormatException.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Exceptions
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     Class InvalidParameterFormatException. Thrown when a parameter given to a command is not valid for the type of
    ///     parameter.
    ///     Implements the <see cref="ConsoleExtensionException" />
    /// </summary>
    /// <seealso cref="ConsoleExtensionException" />
    public class InvalidParameterFormatException : ConsoleExtensionException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidParameterFormatException" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterInfo">The parameter information.</param>
        /// <param name="exception"></param>
        public InvalidParameterFormatException(string value, ParameterInfo parameterInfo, Exception exception)
            : base($"Invalid parameter '{value}'", exception)
        {
            this.Value = value;
            this.ParameterInfo = parameterInfo;
            this.Type = parameterInfo.ParameterType;
            this.Name = parameterInfo.Name;
        }

        /// <summary>
        /// Gets the name of the parameter that was set.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the parameter information.
        /// </summary>
        public ParameterInfo ParameterInfo { get; }

        /// <summary>
        ///     Gets the type of the parameter that was set.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        ///     Gets the value that was set.
        /// </summary>
        public string Value { get; }
    }
}