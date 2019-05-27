namespace ConsoleExtensions.Commandline.Demo
{
    using System.ComponentModel;

    public class DemoObject
    {
        public int Root { get; set; }

        [Description("This just adds two numbers")]
        public string DoSomething(int number)
        {
            return (number + this.Root).ToString();
        }

        public string SomethingComplicated(string first, int notFirst)
        {
            return "";
        }
    }
}