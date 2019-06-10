// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Controller.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("ConsoleExtensions.Commandline.Tests")]

namespace ConsoleExtensions.Commandline
{
    using System;
    using System.Linq;

    using ConsoleExtensions.Commandline.Arguments;
    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Help;
    using ConsoleExtensions.Commandline.Parser;
    using ConsoleExtensions.Proxy;
    using ConsoleExtensions.Templating;

    /// <summary>
    ///     Class Controller. Takes a object and presents is as a command line interface.
    /// </summary>
    public class Controller
    {
        /// <summary>
        ///     The result template. Used to present the result of a command.
        /// </summary>
        private readonly Template resultTemplate;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Controller" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="setup">
        ///     The setup. Optional overwrite of the extensions added to the console. Is not specified the Default
        ///     setup is applied.
        /// </param>
        public Controller(object model, Action<Controller> setup = null)
        {
            this.Model = model;
            this.Proxy = ConsoleProxy.Instance();
            this.TemplateParser = new TemplateParser();

            this.ModelMap = ModelParser.Parse(this.Model);

            setup = setup ?? this.DefaultSetup;

            setup(this);

            this.resultTemplate = this.TemplateParser.Parse("{}");
        }

        /// <summary>
        ///     Gets the model wrapped in the console.
        /// </summary>
        public object Model { get; }

        /// <summary>
        ///     Gets the model map used to map commands and options to methods and properties.
        /// </summary>
        public ModelMap ModelMap { get; }

        /// <summary>
        ///     Gets the proxy used to serve as a output of the console.
        /// </summary>
        public IConsoleProxy Proxy { get; }

        /// <summary>
        ///     Gets the template parser used to present results of command and exceptions.
        /// </summary>
        public TemplateParser TemplateParser { get; }

        /// <summary>
        ///     Instantiates a new controller with the model and standard setup and runs the arguments against the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="setup">The setup.</param>
        public static void Run(object model, string[] args = null, Action<Controller> setup = null)
        {
            var controller = new Controller(model, setup);

            args = args ?? Environment.GetCommandLineArgs().Skip(1).ToArray();

            controller.Run(args);
        }

        /// <summary>
        ///     Runs the specified arguments against the controllers model.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Run(string[] args = null)
        {
            args = args ?? Environment.GetCommandLineArgs().Skip(1).ToArray();

            try
            {
                if (args.Length == 0)
                {
                    args = new[] { "Help" };
                }

                var arguments = ArgumentParser.Parse(args);

                this.ValidateArgumentsAgainstModel(arguments);

                foreach (var argument in arguments.Properties)
                {
                    this.ModelMap.SetOption(argument.Key, argument.Value.ToArray());
                }

                var result = this.ModelMap.Invoke(arguments.Command, arguments.Arguments);

                this.Proxy.WriteTemplate(this.resultTemplate, result);
            }
            catch (Exception e)
            {
                this.Proxy.WriteTemplate(this.resultTemplate, e);
            }
        }

        /// <summary>
        ///     Applies the default setup to the controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        private void DefaultSetup(Controller controller)
        {
            controller.AddHelp().AddExceptionHandling();
        }

        /// <summary>
        ///     Validates the arguments against model.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="UnknownOptionException">Thrown when a requested options is unknown.</exception>
        /// <exception cref="UnknownCommandException">Thrown when a requested command is unknown.</exception>
        private void ValidateArgumentsAgainstModel(ParsedArguments arguments)
        {
            foreach (var argument in arguments.Properties)
            {
                if (!this.ModelMap.Options.TryGetValue(argument.Key, out _))
                {
                    throw new UnknownOptionException(argument.Key, this.ModelMap.Options.Values);
                }
            }

            if (!this.ModelMap.Commands.TryGetValue(arguments.Command, out _))
            {
                throw new UnknownCommandException(arguments.Command, this.ModelMap.Commands.Values);
            }
        }
    }
}