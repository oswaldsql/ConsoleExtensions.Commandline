// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueConverter.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Converters
{
    /// <summary>
    ///     Interface IValueConverter. Provides a way to convert strings to objects and back again when the converts is unable to.
    /// </summary>
    /// <typeparam name="T">The type the converter converts.</typeparam>
    public interface IValueConverter<T>
    {
        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString(T source);

        /// <summary>
        ///     Converts a string to a value of the specified type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A object of the specified type.</returns>
        T ToValue(string source);
    }
}