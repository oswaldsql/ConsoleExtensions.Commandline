namespace Demo
{
	using System;

	public class DemoObject
	{
		public string Name { get; set; }

		public string Say(string what)
		{
			throw new ArgumentException("Test");
			return $"{what} {this.Name}";
		}
	}
}