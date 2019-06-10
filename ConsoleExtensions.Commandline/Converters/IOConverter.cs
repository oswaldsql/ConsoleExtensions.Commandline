// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    ///     Class IOConverter. Converts FileInfo and DirectoryInfo to and from string.
    ///     Implements the <see cref="IValueConverter" />
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class IoConverter : IValueConverter
    {
        /// <summary>
        ///     Gets the priority of the converter.
        /// </summary>
        public ConverterPriority Priority => ConverterPriority.Default;

        /// <summary>
        ///     Determines whether this instance can convert the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance can convert the specified type; otherwise, <c>false</c>.</returns>
        public bool CanConvert(Type type)
        {
            return type == typeof(FileInfo) || type == typeof(DirectoryInfo);
        }

        /// <summary>
        ///     Converts a value to a string.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="customAttributeProvider">The custom attribute provider.</param>
        /// <returns>A <see cref="T:System.String" /> that represents this instance.</returns>
        public string ConvertToString(object source, ICustomAttributeProvider customAttributeProvider)
        {
            return source.ToString();
        }

        /// <summary>
        ///     Converts a string to a value of the specified type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="type">The type.</param>
        /// <param name="customAttributeProvider">The custom attribute provider.</param>
        /// <returns>A object of the specified type.</returns>
        /// <exception cref="ArgumentException">Type not supported by this converter.</exception>
        public object ConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            if (type == typeof(FileInfo))
            {
                return new FileInfo(source);
            }

            if (type == typeof(DirectoryInfo))
            {
                return new DirectoryInfo(source);
            }

            throw new ArgumentException("Type not supported by this converter.");
        }
    }
}