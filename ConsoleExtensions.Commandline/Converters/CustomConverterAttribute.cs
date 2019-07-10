using System;

namespace ConsoleExtensions.Commandline.Converters
{
    public abstract class CustomConverterAttribute : Attribute
    {
        public abstract object ConvertToValue(string value, Type type);

        public abstract object ConvertToString(object value);

        public abstract bool CanConvert(Type type);
    }
}