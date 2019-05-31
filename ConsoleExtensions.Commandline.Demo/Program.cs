namespace ConsoleExtensions.Commandline.Demo
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Controller.Run(new DotNetCliMock());

            Console.ReadLine();
        }
    }
}