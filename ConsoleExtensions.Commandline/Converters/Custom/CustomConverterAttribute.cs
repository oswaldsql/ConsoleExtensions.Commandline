// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomConverterAttribute.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters.Custom
{
    using System;

    /// <summary>
    /// Class CustomConverterAttribute. Implements the
    /// <see cref="Attribute" />
    /// Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="T:System.Attribute" />
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public abstract class CustomConverterAttribute : Attribute
    {
        /// <summary>
        /// Determines whether this instance can convert the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance can convert the specified type;
        /// otherwise, <c>false</c> .</returns>
        public abstract bool CanConvert(Type type);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="value">The value to convert from.</param>
        /// <returns>A string representing the value.</returns>
        public abstract string ConvertToString(object value);

        /// <summary>
        /// Converts a string to the specified type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>A object of the specified type.</returns>
        public abstract object ConvertToValue(string value, Type type);
    }
}