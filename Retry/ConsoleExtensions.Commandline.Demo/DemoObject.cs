namespace ConsoleExtensions.Commandline.Demo
{
	public class DemoObject
	{
		public string DoSomething(int number)
		{
			return (number + Root).ToString();
		}

		public int Root { get; set; }
	}
}