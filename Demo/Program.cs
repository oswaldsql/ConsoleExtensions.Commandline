using System;

namespace Demo
{
	using System.IO;

	class Program
	{
		static void Main(string[] args)
		{
			Controller.Run<DemoObject>(args, Settings.Default);

			Console.ReadLine();
		}

		private static void Setup(ControllerSettings setup)
		{
			setup
				.ViewFor<Exception>("[s:Error]{Message}[/]")
				.ViewFor<FileNotFoundException>("[s:Error][if:FileName]Unable to find : '{FileName}'[/][ifnot:FileName]{Message}[/][/]");
		}
	}

	public class Settings
	{
		public static void Default(ControllerSettings setup)
		{
		}
	}
}
