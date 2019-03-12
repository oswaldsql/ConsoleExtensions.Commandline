using System;

namespace Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			Controller.Init<DemoObject>().WithArguments(args).Run();

			Console.ReadLine();
		}
	}

	public class DemoObject
	{
		public string Name { get; set; }

		public string Say(string what)
		{
			return $"{what} {Name}";
		}
	}
}
