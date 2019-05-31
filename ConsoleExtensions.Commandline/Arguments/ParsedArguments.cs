// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParsedArguments.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Arguments
{
    using System.Collections.Generic;

    /// <summary>
    ///     Class ParsedArguments. The console arguments as mapped by the arguments parser.
    /// </summary>
    public class ParsedArguments
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParsedArguments" /> class.
        /// </summary>
        public ParsedArguments()
        {
            this.Arguments = new string[0];
            this.Properties = new Dictionary<string, List<string>>();
        }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        public string[] Arguments { get; set; }

        /// <summary>
        ///     Gets or sets the command.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        ///     Gets the properties.
        /// </summary>
        public Dictionary<string, List<string>> Properties { get; }
    }
}