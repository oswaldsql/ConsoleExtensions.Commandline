// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomValueConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     Class CustomValueConverter. Converts to and from a string to a objects.
    ///     Implements the <see cref="ConsoleExtensions.Commandline.Converters.IValueConverter{T}" />
    /// </summary>
    /// <typeparam name="T">The type of object to converts to and from.</typeparam>
    /// <seealso cref="ConsoleExtensions.Commandline.Converters.IValueConverter{T}" />
    public class CustomValueConverter<T> : IValueConverter
    {
        /// <summary>
        ///     The function used when converting to string.
        /// </summary>
        private readonly Func<object, string> toString;

        /// <summary>
        ///     The function used when converting to object.
        /// </summary>
        private readonly Func<string, object> toValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomValueConverter{T}" /> class.
        /// </summary>
        /// <param name="toValue">The function used when converting to object.</param>
        /// <param name="toString">The function used when converting to string.</param>
        public CustomValueConverter(Func<string, T> toValue, Func<T, string> toString)
        {
            this.toString = o => toString((T)o);
            this.toValue = s => toValue(s);
        }

        public bool CanConvert(Type type)
        {
            return type == typeof(T);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(object source, ICustomAttributeProvider customAttributeProvider)
        {
            return this.toString(source);
        }

        /// <summary>
        ///     Converts a string to a value of the specified type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A object of the specified type.</returns>
        public object ToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            return this.toValue(source);
        }

        public ConverterPriority Priority => ConverterPriority.First;
    }
}