// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsageDetails.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Help
{
    /// <summary>
    ///     Class UsageDetails.
    /// </summary>
    public class UsageDetails
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        public ArgumentDetails[] Arguments { get; set; }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the type of the return.
        /// </summary>
        public string ReturnType { get; set; }
    }
}