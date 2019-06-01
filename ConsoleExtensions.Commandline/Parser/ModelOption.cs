// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelOption.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Parser
{
    using System;
    using System.Reflection;

    using ConsoleExtensions.Commandline.Exceptions;

    /// <summary>
    ///     Class ModelOption.
    /// </summary>
    public class ModelOption
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ModelOption" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="property">The property.</param>
        /// <param name="source">The source.</param>
        public ModelOption(string name, PropertyInfo property, object source)
        {
            this.Name = name;
            this.Property = property;
            this.Source = source;
        }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the property.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        ///     Gets the source.
        /// </summary>
        public object Source { get; }

        /// <summary>
        ///     Returns the currents value of the option.
        /// </summary>
        /// <returns>A string representing the value.</returns>
        public string CurrentValue()
        {
            // TODO : Add Value converter logic.
            // TODO : catch all the exceptions that can occur and map them
            return this.Property.GetMethod.Invoke(this.Source, new object[0]).ToString();
        }

        /// <summary>
        ///     Sets the specified property to the value, converting the value as needed.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <exception cref="InvalidArgumentFormatException">Thrown if the value conversation failed.</exception>
        public void Set(string stringValue)
        {
            // TODO : Add Value converter logic.
            object o;

            try
            {
                if (this.Property.PropertyType.IsEnum)
                {
                    o = Enum.Parse(this.Property.PropertyType, stringValue, true);
                }
                else
                {
                    o = Convert.ChangeType(stringValue, this.Property.PropertyType);
                }
            }
            catch (Exception e)
            {
                throw new InvalidArgumentFormatException(stringValue, this.Property, e);
            }

            // TODO : catch all the exceptions that can occur and map them
            this.Property.SetMethod.Invoke(this.Source, new[] { o });
        }
    }
}