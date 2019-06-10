// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelMap.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Parser
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ConsoleExtensions.Commandline.Converters;
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
            this.Options = new Dictionary<string, ModelOption>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var option in options)
            {
                this.AddOption(option);
            }

            this.Commands = new Dictionary<string, ModelCommand>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var action in actions)
            {
                this.AddCommand(action);
            }

            this.AddValueConverter(new EnumConverter(), new ConvertableConverter(), new IOConverter(), new BoolConverter());
        }

        /// <summary>
        ///     Gets the commands.
        /// </summary>
        public Dictionary<string, ModelCommand> Commands { get; }

        /// <summary>
        ///     Gets the options.
        /// </summary>
        public Dictionary<string, ModelOption> Options { get; }

        public void SetOption(string option, params string[] values)
        {
            var value = values.FirstOrDefault();

            if (this.Options.TryGetValue(option, out var p))
            {
                try
                {
                    p.Set(this.ConvertStringToObject(value, p.Property.PropertyType, p.Property));
                }
                catch (Exception e)
                {
                    throw new InvalidArgumentFormatException(value, p.Property, e);
                }
            }
            else
            {
                throw new UnknownOptionException(option, this.Options.Values);
            }
        }

        public string[] GetOption(string option)
        {
            if (this.Options.TryGetValue(option, out var p))
            {
                return new[] { this.ConvertObjectToString(p.CurrentValue(), p.Property.PropertyType, p.Property) };
            }

            throw new UnknownOptionException(option, this.Options.Values);
        }

        public List<IValueConverter> ValueConverters = new List<IValueConverter>();

        public object ConvertStringToObject(string stringValue, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            object result;

            var valueConverter = this.ValueConverters.FirstOrDefault(converter => converter.CanConvert(type));
            if (valueConverter != null)
            {
                result = valueConverter.ToValue(stringValue, type, customAttributeProvider);
            }
            else
            {
                throw new ArgumentException("Unable to convert type");
            }

            return result;
        }

        public string ConvertObjectToString(object value, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            string result;

            var valueConverter = this.ValueConverters.FirstOrDefault(converter => converter.CanConvert(type));
            if (valueConverter != null)
            {
                result = valueConverter.ToString(value, customAttributeProvider);
            }
            else
            {
                throw new ArgumentException("Unable to convert type");
            }

            return result;
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
                    var type = info.ParameterType;
                    var valueConverter = this.ValueConverters.FirstOrDefault(converter => converter.CanConvert(type));
                    if (valueConverter != null)
                    {
                        try
                        {
                            o = valueConverter.ToValue(arguments[index], type, info);
                        }
                        catch (Exception e)
                        {
                            throw new InvalidParameterFormatException(arguments[index], info, e);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Unable to convert type");
                    }
                }

                p.Add(o);
            }

            // TODO : catch all the exceptions that can occur and map them
            return method.Method.Invoke(method.Source, p.ToArray());
        }

        public ModelMap AddValueConverter(params IValueConverter[] valueConverters)
        {
            foreach (var valueConverter in valueConverters)
            {

                if (this.ValueConverters.Count == 0)
                {
                    this.ValueConverters.Add(valueConverter);
                }
                else
                {
                    var matchingPriority =
                        this.ValueConverters.FindIndex(converter => converter.Priority == valueConverter.Priority);
                    if (matchingPriority != -1)
                    {
                        this.ValueConverters.Insert(matchingPriority, valueConverter);
                    }
                    else
                    {
                        var firstWithHigherPriority = this.ValueConverters.FindIndex(
                            converter => converter.Priority > valueConverter.Priority);
                        if (firstWithHigherPriority != -1)
                        {
                            this.ValueConverters.Insert(firstWithHigherPriority, valueConverter);
                        }
                        else
                        {
                            this.ValueConverters.Add(valueConverter);
                        }
                    }
                }
            }

            return this;
        }

        public ModelMap AddValueConverter<T>(Func<string, T> toValue, Func<T, string> toString)
        {
            this.AddValueConverter(new CustomValueConverter<T>(toValue, toString));
            return this;
        }
    }
}