// <copyright company="Danfoss A/S">
//   2015 - 2019 Danfoss A/S. All rights reserved.
// </copyright>
namespace Demo
{
	using System.Collections.Generic;

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