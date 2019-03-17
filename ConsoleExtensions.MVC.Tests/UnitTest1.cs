using System;
using Xunit;

namespace ConsoleExtensions.MVC.Tests
{
	using System.Text;

	using Demo;

	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			var model = new StringBuilder("https://github.com/oswaldsql/ConsoleExtensions.Templating");
			
			var modelMap = new ModelMap(model);

			foreach (var flag in modelMap.Flags)
			{
				var value = flag.Value;
				Console.WriteLine($"-{value.Name} = {value.CurrentValue()}");
			}

			modelMap.Flags["Capacity"].Set("100");

			foreach (var flag in modelMap.Flags)
			{
				var value = flag.Value;
				Console.WriteLine($"-{value.Name} = {value.CurrentValue()}");
			}
		}

		[Fact]
		public void Givengiven_Whenwhen_Thenthen()
		{
			var model = new Uri("https://github.com/oswaldsql/ConsoleExtensions.Templating");
			
			var modelMap = new ModelMap(model);

			foreach (var flag in modelMap.Actions)
			{
				var value = flag.Value;
				Console.WriteLine($"{value.Name}");
			}
		}

	}
}
