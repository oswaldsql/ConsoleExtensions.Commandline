// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpGenerator.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Help
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    ///     Class HelpGenerator. Generates the help information required by the help command.
    /// </summary>
    public class HelpGenerator
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HelpGenerator" /> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public HelpGenerator(Controller controller)
        {
            this.Controller = controller;
        }

        /// <summary>
        ///     Gets the controller to base the help on.
        /// </summary>
        public Controller Controller { get; }

        /// <summary>
        ///     Generates Help based on the specified topic. If no topic is specified help will be provided for the entire
        ///     controller.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <returns>The generated HelpDetails.</returns>
        [UsedImplicitly]
        public HelpDetails Help(string topic = "")
        {
            var helpDetails = new HelpDetails();

            var type = this.Controller.Model.GetType();
            helpDetails.ModelName = GetDisplayName(type);
            helpDetails.Description = GetDescription(type);
            helpDetails.ModelVersion = type.Assembly.GetName().Version;

            var map = this.Controller.ModelMap;

            if (topic != string.Empty)
            {
                if (map.Commands.TryGetValue(topic, out var command))
                {
                    var details = new UsageDetails
                    {
                        Name = command.Name,
                        DisplayName = command.DisplayName,
                        Description = command.Description,
                        ReturnType = command.Method.ReturnType.Name,
                        Arguments = command.Method.GetParameters().Select(this.Map).ToArray(),
                    };

                    helpDetails.Usage = details;
                    helpDetails.Options = map.Options.Values.ToArray();
                }
                else if (map.Options.TryGetValue(topic, out var option))
                {
                    var details = new UsageDetails
                    {
                        Name = option.Name,
                        DisplayName = option.DisplayName,
                        Description = option.Description,
                        ReturnType = option.Property.PropertyType.Name,
                    };

                    helpDetails.Usage = details;
                    helpDetails.Options = map.Options.Values.ToArray();
                }
            }
            else
            {
                helpDetails.Commands = map.Commands.Values.ToArray();
                helpDetails.Options = map.Options.Values.ToArray();
            }

            return helpDetails;
        }

        /// <summary>
        ///     Gets the description from the type. If no description is provided or an exception is thrown null is returned.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The description of the type.</returns>
        private static string GetDescription(Type type)
        {
            try
            {
                return type.GetCustomAttribute<DescriptionAttribute>()?.Description;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Gets the display name. Is no display name is returned the type name is used.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The display name.</returns>
        private static string GetDisplayName(Type type)
        {
            try
            {
                return type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? type.Name;
            }
            catch
            {
                return type.Name;
            }
        }

        /// <summary>
        ///     Maps the specified argument to a arguments details object.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>The mapped ArgumentDetails.</returns>
        private ArgumentDetails Map(ParameterInfo arg)
        {
            var result = new ArgumentDetails
            {
                Name = arg.Name,
                DisplayName = arg.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
                Description = arg.GetCustomAttribute<DescriptionAttribute>()?.Description,
                Optional = arg.HasDefaultValue,
                DefaultValue = arg.HasDefaultValue ? arg.DefaultValue : null,
                Type = arg.ParameterType.Name,
            };

            return result;
        }
    }
}