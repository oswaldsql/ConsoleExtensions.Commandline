// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Demo
{
    using System.ComponentModel;

    /// <summary>
    ///     Class Program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            Controller.Run(new Toolbox(), args);
        }

        /// <summary>
        ///     Class Toolbox.
        /// </summary>
        public class Toolbox
        {
            /// <summary>
            ///     Gets or sets the source folder.
            /// </summary>
            /// <value>
            ///     The source folder.
            /// </value>
            [Description("Folder to copy files from.")]
            public string SourceFolder { get; set; }

            /// <summary>
            ///     Gets or sets the target folder.
            /// </summary>
            /// <value>
            ///     The target folder.
            /// </value>
            [Description("Folder to copy files to.")]
            public string TargetFolder { get; set; }

            /// <summary>
            ///     Copies the specified filter.
            /// </summary>
            /// <param name="filter">The filter.</param>
            /// <returns>
            ///     A status message.
            /// </returns>
            [Description("Copy the files from source to destination.")]
            public string Copy(string filter = "*")
            {
                // your logic here
                return "Some files was copied";
            }
        }
    }
}