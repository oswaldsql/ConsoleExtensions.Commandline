// <copyright company="Danfoss A/S">
//   2015 - 2019 Danfoss A/S. All rights reserved.
// </copyright>
namespace Demo
{
	using System;
	using System.Linq;

	public class NextController
	{
		private readonly object model;

		public ControllerSettings Settings { get; set; }

		public NextController(object model)
		{
			this.Settings = new ControllerSettings();
			DefaultSettings(this.Settings);

			this.model = model;
		}

		public static void Run(object model, string[] arguments, Action<ControllerSettings> setup = null)
		{
			var controller = new NextController(model);
			DefaultSettings(controller.Settings);
			setup?.Invoke(controller.Settings);

			var parsedArguments = new ArgumentParser().Parse(arguments);

			

			var i = 0;
			var initialCommand = arguments.Select(arg => new
				                                             {
					                                             index = i++, arg
				                                             })
				.ToArray();

			var firstPropertyIndex = initialCommand.FirstOrDefault(j => j.arg.StartsWith("-"));

			foreach (var command in initialCommand)
			{
				Console.WriteLine($"{command.index} : {command.arg}");
			}

			//controller.WithArguments(arguments).Run(controllerSettings);
		}

		private static void DefaultSettings(ControllerSettings controllerSettings)
		{
			
		}
	}
}