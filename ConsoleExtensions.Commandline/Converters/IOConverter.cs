namespace ConsoleExtensions.Commandline.Converters
{
    using System;
    using System.IO;
    using System.Reflection;

    public class IOConverter : IValueConverter
    {
        public bool CanConvert(Type type)
        {
            return type == typeof(FileInfo) || type == typeof(DirectoryInfo);
        }

        public string ToString(object source, ICustomAttributeProvider customAttributeProvider)
        {
            return source.ToString();
        }

        public object ToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            if(type == typeof(FileInfo))
            {
                return new FileInfo(source);
            }

            if (type == typeof(DirectoryInfo))
            {
                return new DirectoryInfo(source);
            }

            throw new ArgumentException("Type not supported by this converter.");
        }

        public ConverterPriority Priority => ConverterPriority.Later;
    }
}