namespace ConsoleExtensions.Commandline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ConsoleExtensions.Commandline.Parser;
    using ConsoleExtensions.Proxy;
    using ConsoleExtensions.Templating;

    public class Controller
    {
        private object model;

        private ModelMap modelMap;

        private TemplateParser parser;

        public Controller(object model)
        {
            this.Model = model;
            this.proxy = ConsoleProxy.Instance();
            this.parser = new TemplateParser();

            parser.AddTypeTemplate<ModelAction[]>("[foreach]{Name} ({DisplayName})[if:Description] : {Description} [/] [br/][/]");
            this.parser.AddTypeTemplate<int>("Some int {.}");
        }

        public object Model
        {
            get => this.model;
            set
            {
                this.model = value ?? throw new ArgumentException();

                this.modelMap = ModelMapper.Parse(this.model);
            }
        }

        public static void Run(object model, string[] args)
        {
            var controller = new Controller(model);

            controller.Run(args);
        }

        public void Run(string[] args)
        {
            var arguments = ArgumentParser.Parse(args);

            foreach (var argument in arguments.Properties)
            {
                this.modelMap[argument.Key] = argument.Value.FirstOrDefault();
            }

            object result;
            try
            {
                result = this.modelMap.Invoke(arguments.Command + 2, arguments.Arguments);
            }
            catch (Exception e)
            {
                var template1 = this.parser.Parse("Unknown command. [hr/]Known commands are [br/]{}[hr/]");
                this.proxy.WriteTemplate(template1, this.modelMap.Actions.Values.ToArray());
                return;
            }

            var template = parser.Parse("{}");
            this.proxy.WriteTemplate(template, result);
        }

        internal IConsoleProxy proxy { get; set; }
    }


}