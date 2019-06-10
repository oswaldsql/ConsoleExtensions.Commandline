// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterPriority.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ConsoleExtensions.Commandline.Converters
{
    /// <summary>
    /// Enum ConverterPriority
    /// </summary>
    public enum ConverterPriority
    {
        /// <summary>
        /// First priority. Converters with this priority are executed first. Custom converters are given this priority.
        /// </summary>
        First = 1,

        /// <summary>
        /// The sooner priority. Converters with this priority are executed before the default converters.
        /// </summary>
        Sooner = 2,

        /// <summary>
        /// The default priority. This is the priority of standard converters.
        /// </summary>
        Default = 3,

        /// <summary>
        /// The later priority. Converters with this priority are executed after the default converters.
        /// </summary>
        Later = 4,

        /// <summary>
        /// The last priority. Converters with this priority are executed after all other converters.
        /// </summary>
        Last = 5
    }
}