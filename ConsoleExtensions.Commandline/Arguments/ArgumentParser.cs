// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentParser.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Arguments
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Class ArgumentParser. Extracts information about the arguments and maps them as a command, arguments and options.
    /// </summary>
    public class ArgumentParser
    {
        /// <summary>
        ///     RegEx to determine if a string is a property name,
        /// </summary>
        private static readonly Regex IsPropertyName = new Regex("^-\\D");

        /// <summary>
        ///     Parses the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The resulting ParsedArguments object.</returns>
        public static ParsedArguments Parse(params string[] args)
        {
            var result = new ParsedArguments();

            if (args.Length == 0)
            {
                return result;
            }

            var index = ParseCommand(args, result);

            var curPropertyName = string.Empty;
            while (index < args.Length)
            {
                if (IsPropertyName.IsMatch(args[index]))
                {
                    var key = args[index].Substring(1);
                    if (!result.Properties.ContainsKey(key))
                    {
                        result.Properties[key] = new List<string>();
                    }

                    curPropertyName = key;
                }
                else
                {
                    result.Properties[curPropertyName].Add(args[index]);
                }

                index++;
            }

            return result;
        }

        /// <summary>
        ///     Parses the command and arguments and sets the index to the position of the first option.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">The result.</param>
        /// <returns>Index of the first option.</returns>
        private static int ParseCommand(string[] args, ParsedArguments result)
        {
            var index = 0;
            if (!IsPropertyName.IsMatch(args[0]))
            {
                result.Command = args[0];

                var argList = new List<string>();
                index = 1;
                while (index < args.Length)
                {
                    if (!IsPropertyName.IsMatch(args[index]))
                    {
                        argList.Add(args[index]);
                    }
                    else
                    {
                        break;
                    }

                    index++;
                }

                result.Arguments = argList.ToArray();
            }

            return index;
        }
    }
}