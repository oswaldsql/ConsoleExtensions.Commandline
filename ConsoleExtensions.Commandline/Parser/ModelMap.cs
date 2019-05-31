// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelMap.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ConsoleExtensions.Commandline.Exceptions;

    /// <summary>
    ///     Class ModelMap. Handles the translation of command and options to methods and properties.
    /// </summary>
    public class ModelMap
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ModelMap" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="actions">The actions.</param>
        public ModelMap(IEnumerable<ModelOption> options, IEnumerable<ModelCommand> actions)
        {
            this.Options = options.ToDictionary(s => s.Name, StringComparer.InvariantCultureIgnoreCase);
            this.Commands = actions.ToDictionary(s => s.Name, StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        ///     Gets the commands.
        /// </summary>
        public Dictionary<string, ModelCommand> Commands { get; }

        /// <summary>
        ///     Gets the options.
        /// </summary>
        public Dictionary<string, ModelOption> Options { get; }

        /// <summary>
        ///     Gets or sets the option with the specified name.
        /// </summary>
        /// <param name="option">The option name.</param>
        /// <returns>A string representing the options value.</returns>
        /// <exception cref="InvalidArgumentFormatException" accessor="set">Thrown if the value conversation failed.</exception>
        /// <exception cref="UnknownOptionException" accessor="get">Thrown is the specified option is not known.</exception>
        /// <exception cref="UnknownOptionException" accessor="set">Thrown is the specified option is not known.</exception>
        public string this[string option]
        {
            get
            {
                if (this.Options.TryGetValue(option, out var p))
                {
                    return p.CurrentValue();
                }

                throw new UnknownOptionException(option, this.Options.Values);
            }

            set
            {
                if (this.Options.TryGetValue(option, out var p))
                {
                    p.Set(value);
                }
                else
                {
                    throw new UnknownOptionException(option, this.Options.Values);
                }
            }
        }

        /// <summary>
        ///     Adds a command to the model map.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The ModelMap.</returns>
        public ModelMap AddCommand(ModelCommand command)
        {
            this.Commands.Add(command.Name, command);
            return this;
        }

        /// <summary>
        ///     Adds option to the model map.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>The ModelMap.</returns>
        public ModelMap AddOption(ModelOption option)
        {
            this.Options.Add(option.Name, option);
            return this;
        }

        /// <summary>
        ///     Invokes the specified command.
        /// </summary>
        /// <param name="command">The name of the command to be invoked.</param>
        /// <param name="arguments">The arguments to be parsed to the command.</param>
        /// <returns>The result of the method as a object.</returns>
        /// <exception cref="UnknownCommandException">Thrown is the command in not known.</exception>
        /// <exception cref="System.ArgumentException">Invalid argument</exception>
        /// <exception cref="TooManyArgumentsException">Thrown is too many arguments was specified.</exception>
        /// <exception cref="MissingArgumentException">Thrown is one or more arguments was missing.</exception>
        /// <exception cref="InvalidParameterFormatException">Thrown is the specified value of a argument is not valid for that type.</exception>
        public object Invoke(string command, params string[] arguments)
        {
            if (!this.Commands.TryGetValue(command, out var method))
            {
                throw new UnknownCommandException(command, this.Commands.Values);
            }

            var infos = method.Method.GetParameters();

            if (infos.Length < arguments.Length)
            {
                throw new TooManyArgumentsException(infos.Select(s => s.Name).ToArray());
            }

            var p = new List<object>();
            for (var index = 0; index < infos.Length; index++)
            {
                var info = infos[index];

                object o;

                if (arguments.Length <= index)
                {
                    if (info.HasDefaultValue)
                    {
                        o = info.DefaultValue;
                    }
                    else
                    {
                        throw new MissingArgumentException(command, info.Name, infos.Select(s => s.Name).ToArray());
                    }
                }
                else
                {
                    if (info.ParameterType.IsEnum)
                    {
                        try
                        {
                            o = Enum.Parse(info.ParameterType, arguments[index], true);
                        }
                        catch (Exception e)
                        {
                            throw new InvalidParameterFormatException(arguments[index], info, e);
                        }
                    }
                    else
                    {
                        try
                        {
                            o = Convert.ChangeType(arguments[index], info.ParameterType);
                        }
                        catch (Exception e)
                        {
                            throw new InvalidParameterFormatException(arguments[index], info, e);
                        }
                    }
                }

                p.Add(o);
            }

            return method.Method.Invoke(method.Source, p.ToArray());
        }
    }
}