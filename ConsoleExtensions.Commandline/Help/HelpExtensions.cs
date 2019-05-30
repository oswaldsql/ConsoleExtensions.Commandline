// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpExtensions.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Help
{
    using System;
    using System.Reflection;

    using ConsoleExtensions.Commandline.Parser;

    /// <summary>
    ///     Class HelpExtensions.
    /// </summary>
    public static class HelpExtensions
    {
        /// <summary>
        ///     Adds the help command to the controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns>The Controller.</returns>
        public static Controller AddHelp(this Controller controller)
        {
            if (controller.ModelMap.Commands.ContainsKey("Help"))
            {
                return controller;
            }

            controller.TemplateParser.AddTypeTemplate<ModelOption>("  [c:white]-{Name}[/] : {Description}");
            controller.TemplateParser.AddTypeTemplate<ModelCommand>("  [c:white]{Name}[/] : {Description}");
            controller.TemplateParser.AddTypeTemplate<Version>(
                "[c:white]{Major}[/].[c:white]{Minor}[/][if:Build].[c:white]{Build}[if:Revision].[c:white]{Revision}[/][/][/][/]");

            controller.TemplateParser.AddTypeTemplate<UsageDetails>(
                "Usage : {Name}[if:Arguments][foreach:Arguments] [[{Name}][/]"
                + "[br/][br/]Arguments:[br/][foreach:Arguments] {}[br/][/][/]");
            controller.TemplateParser.AddTypeTemplate<ArgumentDetails>(
                "<[c:white]{Name}[/]:{Type}> [if:DisplayName]({DisplayName})[/] [if:Optional](Default : '{DefaultValue}') [/][if:Description][br/]  {Description}[/]");

            controller.TemplateParser.AddTypeTemplate<HelpDetails>(
                "[if:ModelName][c:white]{ModelName}[/][/][if:ModelVersion] ({ModelVersion})[/][br/]"
                + "[if:Description]{Description}[br/][/]" + "[if:Usage]{Usage}[br/][/][br/]"
                + "[if:Options][c:white]Options[/][hr/][foreach:Options]{}[br/][/][hr/]"
                + "[/][if:Commands][c:white]Commands[/][hr/][foreach:Commands]{}[br/][/][hr/][/]");

            var name = Assembly.GetEntryAssembly()?.GetName().Name;

            var helpGenerator = new HelpGenerator(controller);
            var method = helpGenerator.GetType().GetMethod("Help");
            var helpAction = new ModelCommand("Help", method, helpGenerator)
                                 {
                                     Description =
                                         $"Show command line help. '{name} Help [Topic]' for more information on a command or option.",
                                     DisplayName = "Help"
                                 };

            controller.ModelMap.AddCommand(helpAction);

            return controller;
        }
    }
}