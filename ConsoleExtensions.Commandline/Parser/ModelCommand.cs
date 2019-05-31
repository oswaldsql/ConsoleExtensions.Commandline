// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelCommand.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Parser
{
    using System.Reflection;

    /// <summary>
    ///     Class ModelCommand. Represents a model command.
    /// </summary>
    public class ModelCommand
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ModelCommand" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="method">The method.</param>
        /// <param name="source">The source.</param>
        public ModelCommand(string name, MethodInfo method, object source)
        {
            this.Name = name;
            this.Method = method;
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
        ///     Gets the method.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the source object the execute the method on.
        /// </summary>
        public object Source { get; }
    }
}