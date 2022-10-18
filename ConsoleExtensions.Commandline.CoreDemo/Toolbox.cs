using System.ComponentModel;

namespace ConsoleExtensions.Commandline.CoreDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Description("Tools for making life better.")]
    internal class Toolbox
    {
        [Description("Sets your age to the specified value.")]
        public Task<string> SetAgeAsync([Description("The requested age.")]int age)
        {
            if (age < 0)
                throw new Exception("Setting your age to a negative value will break time and space as we know it");

            Task.Delay(1000);

            return Task.FromResult($"Your age is now {age}");
        }
    }
}
