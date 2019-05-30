// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentDetails.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Help
{
    /// <summary>
    ///     Class ArgumentDetails.
    /// </summary>
    public class ArgumentDetails
    {
        /// <summary>
        ///     Gets or sets the default value.
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the argument is optional.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        public string Type { get; set; }
    }
}