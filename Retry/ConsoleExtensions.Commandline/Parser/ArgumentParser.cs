namespace ConsoleExtensions.Commandline.Parser
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class ArgumentParser
    {
        private static readonly Regex IsPropertyName = new Regex("^-\\D");

        public static ParsedArguments Parse(params string[] args)
        {
            var result = new ParsedArguments();

            var index = ParseCommand(args, result);

            var curPropertyName = string.Empty;
            while (index < args.Length)
            {
                if (IsPropertyName.IsMatch(args[index]))
                {
                    var key = args[index].Substring(1);
                    if (!result.Properties.ContainsKey(key))
                    {
                        result.Properties[key] = new List<string>();
                    }

                    curPropertyName = key;
                }
                else
                {
                    result.Properties[curPropertyName].Add(args[index]);
                }

                index++;
            }

            return result;
        }

        private static int ParseCommand(string[] args, ParsedArguments result)
        {
            var index = 0;
            if (!IsPropertyName.IsMatch(args[0]))
            {
                result.Command = args[0];

                var argList = new List<string>();
                index = 1;
                while (index < args.Length)
                {
                    if (!IsPropertyName.IsMatch(args[index]))
                    {
                        argList.Add(args[index]);
                    }
                    else
                    {
                        break;
                    }

                    index++;
                }

                result.Arguments = argList.ToArray();
            }

            return index;
        }
    }
}