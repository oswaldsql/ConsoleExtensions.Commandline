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
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using ConsoleExtensions.Commandline.Converters;
    using ConsoleExtensions.Commandline.Converters.Custom;
    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Util;
    using ConsoleExtensions.Commandline.Validators;

    /// <summary>
    ///     Class ModelMap. Handles the translation of command and options to methods and properties.
    /// </summary>
    public class ModelMap
    {
        static TaskFactory taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        ///     The value converters used to convert from string to objects and back.
        /// </summary>
        private readonly List<IValueConverter> valueConverters = new List<IValueConverter>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModelMap" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="commands">The commands.</param>
        public ModelMap(IEnumerable<ModelOption> options, IEnumerable<ModelCommand> commands)
        {
            this.Options = new Dictionary<string, ModelOption>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var option in options)
            {
                this.AddOption(option);
            }

            this.Commands = new Dictionary<string, ModelCommand>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var command in commands)
            {
                this.AddCommand(command);
            }

            this.AddValueConverter(
                new EnumConverter(),
                new ConvertibleConverter(),
                new IoConverter(),
                new BoolConverter(),
                new TimeSpanValueConverter());
        }

        /// <summary>
        ///     Gets the commands.
        /// </summary>
        internal Dictionary<string, ModelCommand> Commands { get; }

        /// <summary>
        ///     Gets the options.
        /// </summary>
        internal Dictionary<string, ModelOption> Options { get; }

        /// <summary>
        ///     Adds a command to the model map.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The ModelMap.</returns>
        public ModelMap AddCommand(ModelCommand command)
        {
            var commandName = command.Name;

            if (commandName.EndsWith("Async"))
            {
                var type = command.Method.ReturnParameter.ParameterType;
                if (type == typeof(Task) || type.IsSubclassOf(typeof(Task)))
                {
                    commandName = commandName.Substring(0, commandName.Length - 5);
                    command = new ModelCommand(commandName, command.Method, command.Source)
                        { Description = command.Description, DisplayName = command.DisplayName };
                }
            }

            this.Commands.Add(commandName, command);
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
        ///     Adds the value converter.
        /// </summary>
        /// <param name="converters">The value converters.</param>
        /// <returns>The ModelMap.</returns>
        public ModelMap AddValueConverter(params IValueConverter[] converters)
        {
            foreach (var valueConverter in converters)
            {
                if (this.valueConverters.Count == 0)
                {
                    this.valueConverters.Add(valueConverter);
                }
                else
                {
                    var matchingPriority =
                        this.valueConverters.FindIndex(converter => converter.Priority == valueConverter.Priority);
                    if (matchingPriority != -1)
                    {
                        this.valueConverters.Insert(matchingPriority, valueConverter);
                    }
                    else
                    {
                        var firstWithHigherPriority = this.valueConverters.FindIndex(
                            converter => converter.Priority > valueConverter.Priority);
                        if (firstWithHigherPriority != -1)
                        {
                            this.valueConverters.Insert(firstWithHigherPriority, valueConverter);
                        }
                        else
                        {
                            this.valueConverters.Add(valueConverter);
                        }
                    }
                }
            }

            return this;
        }

        /// <summary>
        ///     Adds a custom value converter.
        /// </summary>
        /// <typeparam name="T">The type of value converter to add.</typeparam>
        /// <param name="toValue">To value.</param>
        /// <param name="toString">To string.</param>
        /// <returns>The ModelMap.</returns>
        public ModelMap AddValueConverter<T>(Func<string, T> toValue, Func<T, string> toString)
        {
            this.AddValueConverter(new CustomValueConverter<T>(toValue, toString));
            return this;
        }

        /// <summary>
        ///     Gets the option.
        /// </summary>
        /// <param name="option">The option name.</param>
        /// <returns>Zero or more values representing the value of the option.</returns>
        /// <exception cref="UnknownOptionException">Thrown is the requested option is unknown.</exception>
        public string[] GetOption(string option)
        {
            // TODO : support multiple values.
            if (this.Options.TryGetValue(option, out var p))
            {
                return new[] { this.ConvertObjectToString(p.CurrentValue(), p.Property.PropertyType, p.Property) };
            }

            throw new UnknownOptionException(option, this.Options.Values);
        }

        public object Invoke(string command, params string[] arguments) =>
            Invoke(command, CancellationToken.None, arguments);

        /// <summary>
        ///     Invokes the specified command.
        /// </summary>
        /// <param name="command">The name of the command to be invoked.</param>
        /// <param name="arguments">The arguments to be parsed to the command.</param>
        /// <returns>The result of the method as a object.</returns>
        /// <exception cref="UnknownCommandException">Thrown is the command in not known.</exception>
        /// <exception cref="TooManyArgumentsException">Thrown is too many arguments was specified.</exception>
        /// <exception cref="MissingArgumentException">Thrown is one or more arguments was missing.</exception>
        /// <exception cref="InvalidParameterFormatException">
        ///     Thrown is the specified value of a argument is not valid for that
        ///     type.
        /// </exception>
        /// <exception cref="ArgumentException">Unable to convert type</exception>
        /// <exception cref="System.ArgumentException">Thrown is the command in not known.</exception>
        public object Invoke(string command, CancellationToken token, params string[] arguments)
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
                    if (info.ParameterType == typeof(CancellationToken))
                    {
                        o = token;
                    }
                    else
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
                    try
                    {
                        o = this.ConvertStringToObject(arguments[index], type, info);
                    }
                    catch (Exception e)
                    {
                        throw new InvalidParameterFormatException(arguments[index], info, e);
                    }
                }

                p.Add(o);
            }

            // TODO : catch all the exceptions that can occur and map them
            var result = method.Method.Invoke(method.Source, p.ToArray());

            if (result.GetType().IsSubclassOf(typeof(Task)))
            {
                var task = (Task)result;
                {
                    taskFactory.StartNew(() => task).Unwrap().GetAwaiter().GetResult();

                    if (method.Method.ReturnType == typeof(Task))
                    {
                        result = null;
                    }
                    else
                    {
                        result = task.GetType().GetProperty("Result").GetValue(task);
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     Sets the option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="values">The values.</param>
        /// <exception cref="InvalidArgumentFormatException">Thrown if the option can not be converted.</exception>
        /// <exception cref="UnknownOptionException">Thrown if the option is unknown.</exception>
        public void SetOption(string option, params string[] values)
        {
            // TODO : support multiple values.
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

        /// <summary>
        ///     Converts a object to string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <param name="customAttributeProvider">The custom attribute provider.</param>
        /// <returns>A string representing the object.</returns>
        /// <exception cref="ArgumentException">Unable to convert type</exception>
        private string ConvertObjectToString(object value, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            string result;

            if (customAttributeProvider.TryGetCustomAttribute<CustomConverterAttribute>(out var con))
            {
                result = con.ConvertToString(value);
            }
            else
            {
                var valueConverter = this.valueConverters.FirstOrDefault(converter => converter.CanConvert(type));
                if (valueConverter != null)
                {
                    result = valueConverter.ConvertToString(value, customAttributeProvider);
                }
                else
                {
                    throw new ArgumentException("Unable to convert type");
                }
            }

            return result;
        }

        /// <summary>
        ///     Converts the string to object.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="type">The type.</param>
        /// <param name="customAttributeProvider">The custom attribute provider.</param>
        /// <returns>Converts a string to the specified object type.</returns>
        /// <exception cref="ArgumentException">Unable to convert type</exception>
        private object ConvertStringToObject(
            string stringValue,
            Type type,
            ICustomAttributeProvider customAttributeProvider)
        {
            object result;

            if (customAttributeProvider.TryGetCustomAttribute<CustomConverterAttribute>(out var con))
            {
                result = con.ConvertToValue(stringValue, type);
            }
            else
            {
                var valueConverter = this.valueConverters.FirstOrDefault(converter => converter.CanConvert(type));
                if (valueConverter != null)
                {
                    result = valueConverter.ConvertToValue(stringValue, type, customAttributeProvider);
                }
                else
                {
                    throw new ArgumentException("Unable to convert type");
                }
            }

            var customValidatorAttributes = customAttributeProvider.GetCustomAttributes<CustomValidatorAttribute>();
            foreach (var validator in customValidatorAttributes)
            {
                validator.Validate(result);
            }

            return result;
        }
    }
}