using System;
using System.Linq;

namespace ConsoleExtensions.Commandline
{
	public class Controller
	{
		private object model;
		private ModelMap modelMap;

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
			var controller = new Controller();
			controller.Model = model;
			var arguments = ArgumentParser.Parse(args);

			foreach (var argument in arguments.Properties)
				controller.modelMap[argument.Key] = argument.Value.FirstOrDefault();

			var result = controller.modelMap.Invoke(arguments.Command, arguments.Arguments);

			Console.WriteLine(result.ToString());
		}
	}
}