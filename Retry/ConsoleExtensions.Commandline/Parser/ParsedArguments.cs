namespace ConsoleExtensions.Commandline.Parser
{
    using System.Collections.Generic;

    public class ParsedArguments
    {
        public ParsedArguments()
        {
            this.Arguments = new string[0];
            this.Properties = new Dictionary<string, List<string>>();
        }

        public string[] Arguments { get; set; }

        public string Command { get; set; }

        public Dictionary<string, List<string>> Properties { get; set; }
    }
}