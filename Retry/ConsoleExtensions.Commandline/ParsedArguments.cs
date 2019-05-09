using System.Collections.Generic;

namespace ConsoleExtensions.Commandline
{
	public class ParsedArguments
	{
		public ParsedArguments()
		{
			this.Arguments = new string[0]; 
			this.Properties = new Dictionary<string, List<string>>();
		}

		public string Command { get; set; }

		public string[] Arguments { get; set; }

		public Dictionary<string, List<string>> Properties { get; set; }
	}
}