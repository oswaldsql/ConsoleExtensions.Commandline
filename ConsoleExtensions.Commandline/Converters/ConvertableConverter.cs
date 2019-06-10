namespace ConsoleExtensions.Commandline.Converters
{
    using System;
    using System.Reflection;

    public class ConvertableConverter : IValueConverter
    {
        public bool CanConvert(Type type)
        {
            return typeof(IConvertible).IsAssignableFrom(type);
        }

        public string ToString(object source, ICustomAttributeProvider customAttributeProvider)
        {
            return source.ToString();
        }

        public object ToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            return Convert.ChangeType(source, type);
        }

        public ConverterPriority Priority => ConverterPriority.Last;
    }
}