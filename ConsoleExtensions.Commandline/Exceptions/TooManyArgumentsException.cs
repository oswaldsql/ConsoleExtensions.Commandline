namespace ConsoleExtensions.Commandline.Parser
{
    using ConsoleExtensions.Commandline.Exceptions;

    public class TooManyArgumentsException : ConsoleExtensionException
    {
        public string[] Arguments { get; }

        public TooManyArgumentsException(string[] arguments) : base("Too many arguments.")
        {
            this.Arguments = arguments;
        }
    }
}