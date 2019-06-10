namespace ConsoleExtensions.Commandline.Converters
{
    using System;
    using System.Reflection;

    public class UriConverter : IValueConverter
    {
        public bool CanConvert(Type type)
        {
            return type == typeof(Uri);
        }

        public string ToString(object source, ICustomAttributeProvider customAttributeProvider)
        {
            return source.ToString();
        }

        public object ToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            return new Uri(source, UriKind.RelativeOrAbsolute);
        }

        public ConverterPriority Priority { get; }
    }
}