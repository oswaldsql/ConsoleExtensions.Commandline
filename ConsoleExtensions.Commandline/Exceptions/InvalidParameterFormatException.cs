namespace ConsoleExtensions.Commandline.Parser
{
    using System;
    using System.Reflection;

    using ConsoleExtensions.Commandline.Exceptions;

    public class InvalidParameterFormatException : ConsoleExtensionException
    {
        public string Value { get; }

        public ParameterInfo ParameterInfo { get; }

        public InvalidParameterFormatException(string value, ParameterInfo parameterInfo) : base($"Invalid parameter '{value}'")
        {
            this.Value = value;
            this.ParameterInfo = parameterInfo;
            this.Type = parameterInfo.ParameterType;
            this.Name = parameterInfo.Name;
        }
        public string Name { get; set; }

        public Type Type { get; set; }
    }
}