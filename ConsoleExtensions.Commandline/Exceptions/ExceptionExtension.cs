// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionExtension.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;

namespace ConsoleExtensions.Commandline.Exceptions
{
    using System;

    /// <summary>
    /// Class ExceptionExtension. Extends the controller with the AddExceptionHandling method.
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Adds the exception handling.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns>The Controller.</returns>
        public static Controller AddExceptionHandling(this Controller controller)
        {
            controller.TemplateParser.AddTypeTemplate<ConsoleExtensionException>("[s:error]{Message}[/]");
            controller.TemplateParser.AddTypeTemplate<Exception>("[s:error]An unknown exception occurred.[br/][/] {Message}");
            controller.TemplateParser.AddTypeTemplate<TaskCanceledException>("[s:error]The task was canceled by the user.[br/][/]");

            return controller;
        }
    }
}