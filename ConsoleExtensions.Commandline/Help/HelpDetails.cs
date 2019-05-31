// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpDetails.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ConsoleExtensions.Commandline.Help
{
    using System;

    using ConsoleExtensions.Commandline.Parser;

    /// <summary>
    /// Class HelpDetails.
    /// </summary>
    public class HelpDetails
    {
        /// <summary>
        /// Gets or sets the commands available.
        /// </summary>
        public ModelCommand[] Commands { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the model.
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the model version.
        /// </summary>
        public Version ModelVersion { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public ModelOption[] Options { get; set; }

        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
        public UsageDetails Usage { get; set; }
    }
}