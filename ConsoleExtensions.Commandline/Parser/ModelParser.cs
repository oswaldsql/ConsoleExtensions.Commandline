// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelParser.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Parser
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    ///     Class ModelParser. Parses a object and returns a model map representing that object.
    /// </summary>
    public class ModelParser
    {
        /// <summary>
        ///     Parses the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A ModelMap representing the model.</returns>
        public static ModelMap Parse(object model)
        {
            return new ModelMap(PopulateOptions(model), PopulateCommands(model));
        }

        /// <summary>
        ///     Gets a enumerable of methods that can be mapped.
        /// </summary>
        /// <param name="methods">The runtime methods.</param>
        /// <returns>A Enumerable of MethodInfos.</returns>
        private static IEnumerable<MethodInfo> GetMappableMethods(IEnumerable<MethodInfo> methods)
        {
            return methods
                .Where(t => t.IsPublic && !t.IsSpecialName && !t.IsConstructor && t.DeclaringType != typeof(object))
                .ToLookup(info => info.Name).Where(t => t.Count() == 1).Select(t => t.First());
        }

        /// <summary>
        ///     Populates the actions.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A Enumerable of ModelCommands.</returns>
        private static IEnumerable<ModelCommand> PopulateCommands(object model)
        {
            var runtimeMethods = model.GetType().GetRuntimeMethods();
            var methodInfos = GetMappableMethods(runtimeMethods);

            foreach (var method in methodInfos)
            {
                var methodName = method.Name;

                if (methodName.EndsWith("Async") && methodName.Length > 5)
                {
                    if (method.ReturnType == typeof(Task) || method.ReturnType.BaseType == typeof(Task))
                    {
                        methodName = methodName.Substring(0, methodName.Length - 5);
                    }
                }

                var action = new ModelCommand(methodName, method, model)
                {
                    DisplayName =
                        method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                        ?? CreateFriendlyName(methodName),
                    Description = method.GetCustomAttribute<DescriptionAttribute>()?.Description,
                };

                yield return action;
            }
        }

        /// <summary>
        ///     Populates the options.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A Enumerable of ModelOptions.</returns>
        private static IEnumerable<ModelOption> PopulateOptions(object model)
        {
            var propertyInfos = model.GetType().GetProperties();

            foreach (var info in propertyInfos.Where(t => t.GetMethod.GetParameters().Length == 0))
            {
                var flag = new ModelOption(info.Name, info, model)
                {
                    DisplayName =
                        info.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                        ?? CreateFriendlyName(info.Name),
                    Description = info.GetCustomAttribute<DescriptionAttribute>()?.Description,
                };

                yield return flag;
            }
        }

        /// <summary>
        ///     Creates a friendly name from a method name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The friendly name.</returns>
        private static string CreateFriendlyName(string name)
        {
            var replace = Regex.Replace(name, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2");

            var splitCamelCase = Regex.Replace(replace, @"(\p{Ll})(\P{Ll})", "$1 $2");

            return splitCamelCase;
        }
    }
}