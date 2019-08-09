// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoolConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///     Class BoolConverter. Converts string to and from bool values.
    ///     Implements the <see cref="IValueConverter" />
    ///     Implements the <see cref="ConsoleExtensions.Commandline.Converters.IValueConverter" />
    /// </summary>
    /// <seealso cref="ConsoleExtensions.Commandline.Converters.IValueConverter" />
    /// <seealso cref="IValueConverter" />
    public class BoolConverter : IValueConverter
    {
        /// <summary>
        ///     The value mapper containing all default mappings.
        /// </summary>
        private static readonly Dictionary<string, bool> ValueMapper =
            new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        ///     Initializes static members of the <see cref="BoolConverter" /> class.
        /// </summary>
        static BoolConverter()
        {
            ValueMapper.Add(string.Empty, true);
            ValueMapper.Add("true", true);
            ValueMapper.Add("1", true);
            ValueMapper.Add("on", true);
            ValueMapper.Add("yes", true);

            ValueMapper.Add("false", false);
            ValueMapper.Add("0", false);
            ValueMapper.Add("off", false);
            ValueMapper.Add("no", false);
        }

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
            return type == typeof(bool);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="customAttributeProvider">The custom attribute provider.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
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
        /// <exception cref="ArgumentException">Thrown if the conversion was not a success.</exception>
        public object ConvertToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            if (source == null)
            {
                return true;
            }

            if (ValueMapper.TryGetValue(source, out var result))
            {
                return result;
            }

            throw new ArgumentException();
        }
    }
}