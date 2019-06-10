namespace ConsoleExtensions.Commandline.Converters
{
    using System;
    using System.Reflection;

    public class EnumConverter : IValueConverter
    {
        public bool CanConvert(Type type)
        {
            return type.IsEnum;
        }

        public string ToString(object source, ICustomAttributeProvider customAttributeProvider)
        {
            return source.ToString();
        }

        public object ToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            return Enum.Parse(type, source, true);
        }

        public ConverterPriority Priority => ConverterPriority.Later;
    }
}