namespace ConsoleExtensions.Commandline.Demo
{
    using System;
    using System.ComponentModel;
    using System.IO;

    using ConsoleExtensions.Commandline;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Controller.Run(new Toolbox());
        }
    }

    public class Toolbox
    {
        [Description("Folder to copy files from.")]
        public string SourceFolder { get; set; }

        [Description("Folder to copy files to.")]
        public string TargetFolder { get; set; }

        [Description("Copy the files from source to destination.")]
        public string Copy(string filter = "*")
        {
            /// your logic here
            return "Some files was copied";
        }
    }
}