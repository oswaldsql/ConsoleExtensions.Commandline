namespace ConsoleExtensions.Commandline.Demo
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Controller.Run(new DemoObject(), args);

            Console.ReadLine();
        }
    }
}