namespace ConsoleExtensions.Commandline.Parser
{
    using ConsoleExtensions.Commandline.Exceptions;

    public class MissingArgumentException : ConsoleExtensionException
    {
        public string Argument { get; }

        public string[] Arguments { get; }

        public MissingArgumentException(string argument, string[] arguments) : base($"Missing argument {argument}")
        {
            this.Argument = argument;
            this.Arguments = arguments;
        }
    }
}