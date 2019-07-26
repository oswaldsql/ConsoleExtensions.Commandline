// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoolValueAnnotationAttribute.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters.Custom
{
    using System;

    /// <summary>
    ///     Class BoolValueAnnotationAttribute. Implements the
    ///     <see cref="CustomConverterAttribute" />
    /// </summary>
    /// <seealso cref="T:ConsoleExtensions.Commandline.Converters.CustomConverterAttribute" />
    public class BoolValueAnnotationAttribute : CustomConverterAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="BoolValueAnnotationAttribute" /> class.
        /// </summary>
        /// <param name="trueValue">The <see langword="true" /> value.</param>
        /// <param name="falseValue">The <see langword="false" /> value.</param>
        public BoolValueAnnotationAttribute(string trueValue, string falseValue)
        {
            this.TrueValue = trueValue;
            this.FalseValue = falseValue;
        }

        /// <summary>
        ///     Gets the <see langword="false" /> value.
        /// </summary>
        /// <value>
        ///     The <see langword="false" /> value.
        /// </value>
        private string FalseValue { get; }

        /// <summary>
        ///     Gets the <see langword="true" /> value.
        /// </summary>
        /// <value>
        ///     The <see langword="true" /> value.
        /// </value>
        private string TrueValue { get; }

        /// <summary>
        ///     Determines whether this instance can convert the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified type;
        ///     otherwise, <c>false</c> .
        /// </returns>
        public override bool CanConvert(Type type)
        {
            return type == typeof(bool);
        }

        /// <summary>
        ///     Converts the <paramref name="value" /> to a string.
        /// </summary>
        /// <param name="value">The value to convert to string.</param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if the <paramref name="value" /> is not a boolean value.
        /// </exception>
        /// <returns>
        ///     A string representing the value.
        /// </returns>
        public override string ConvertToString(object value)
        {
            var b = value as bool?;

            if (b == null)
            {
                throw new ArgumentException();
            }

            return b == true ? this.TrueValue : this.FalseValue;
        }

        /// <summary>
        ///     Converts a string to the specified type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if the string <paramref name="value" /> is not a valid
        ///     boolean.
        /// </exception>
        /// <returns>
        ///     A object of the specified type.
        /// </returns>
        public override object ConvertToValue(string value, Type type)
        {
            if (value.Equals(this.TrueValue, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            if (value.Equals(this.FalseValue, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            throw new ArgumentException();
        }
    }
}