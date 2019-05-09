using System;

namespace ConsoleExtensions.Commandline.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
			Controller.Run(new DemoObject(), args);

			Console.ReadLine();
        }
    }
}
