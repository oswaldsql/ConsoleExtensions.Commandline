// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomValueConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters
{
    using System;

    /// <summary>
    ///     Class CustomValueConverter. Converts to and from a string to a objects.
    ///     Implements the <see cref="ConsoleExtensions.Commandline.Converters.IValueConverter{T}" />
    /// </summary>
    /// <typeparam name="T">The type of object to converts to and from.</typeparam>
    /// <seealso cref="ConsoleExtensions.Commandline.Converters.IValueConverter{T}" />
    public class CustomValueConverter<T> : IValueConverter<T>
    {
        /// <summary>
        ///     The function used when converting to string.
        /// </summary>
        private readonly Func<T, string> toString;

        /// <summary>
        ///     The function used when converting to object.
        /// </summary>
        private readonly Func<string, T> toValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomValueConverter{T}" /> class.
        /// </summary>
        /// <param name="toString">The function used when converting to string.</param>
        /// <param name="toValue">The function used when converting to object.</param>
        public CustomValueConverter(Func<T, string> toString, Func<string, T> toValue)
        {
            this.toString = toString;
            this.toValue = toValue;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(T source)
        {
            return this.toString(source);
        }

        /// <summary>
        ///     Converts a string to a value of the specified type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A object of the specified type.</returns>
        public T ToValue(string source)
        {
            return this.toValue(source);
        }
    }
}