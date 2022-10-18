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
        [Description("Uses relativistic time for calculating age around the nearest black hole.")]
        [DisplayName("Relativistic Time")]
        public bool R { get; set; } = false;

        [Description("Sets your age to the specified value.")]
        public async Task<string> SetAgeAsync([Description("The requested age.")]int age = 18, CancellationToken token = default)
        {
            if (age < 0)
                throw new Exception("Setting your age to a negative value will break time and space as we know it.");

            await Task.Delay(10000, token);

            return $"Your age is now {age}";
        }
    }
}
