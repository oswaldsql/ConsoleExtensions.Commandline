namespace ConsoleExtensions.Commandline.Parser
{
    using System;
    using System.Reflection;

    public class ModelAction
    {
        public ModelAction(string name)
        {
            this.Name = name;
        }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public MethodInfo Method { get; set; }

        public string Name { get; }

        public ConsoleKeyInfo[] ShortcutKeys { get; set; }

        public object Source { get; set; }
    }
}