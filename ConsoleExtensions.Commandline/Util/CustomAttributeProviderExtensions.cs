// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomAttributeProviderExtensions.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Class CustomAttributeProviderExtensions. Extends the attribute
    ///     provider with some nice helper functions.
    /// </summary>
    public static class CustomAttributeProviderExtensions
    {
        /// <summary>
        ///     Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T">The type of attribute to get.</typeparam>
        /// <param name="element">The element.</param>
        /// <returns>
        ///     A enumeration of attributes of the specified type.
        /// </returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider element)
            where T : Attribute
        {
            var attributes = element.GetCustomAttributes(typeof(T), true);

            foreach (var attribute in attributes)
            {
                yield return attribute as T;
            }
        }

        /// <summary>
        ///     Tries the get custom attribute.
        /// </summary>
        /// <typeparam name="T">The type of attribute to get.</typeparam>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     <para>
        ///         <c>true</c> if a attribute of the type exists, <c>false</c>
        ///     </para>
        ///     <para>otherwise.</para>
        /// </returns>
        public static bool TryGetCustomAttribute<T>(this ICustomAttributeProvider element, out T value)
            where T : Attribute
        {
            var attributes = element.GetCustomAttributes(typeof(T), true);

            if (attributes.Length > 0)
            {
                value = attributes.First() as T;
                return true;
            }

            value = null;
            return false;
        }
    }
}