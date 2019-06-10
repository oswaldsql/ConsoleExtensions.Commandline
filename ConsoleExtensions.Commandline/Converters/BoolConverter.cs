namespace ConsoleExtensions.Commandline.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class BoolConverter : IValueConverter
    {
        private Dictionary<string, bool> valueMapper = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);

        public BoolConverter()
        {
            this.valueMapper.Add("", true);
            this.valueMapper.Add("true", true);
            this.valueMapper.Add("1", true);
            this.valueMapper.Add("on", true);
            this.valueMapper.Add("yes", true);

            this.valueMapper.Add("false", false);
            this.valueMapper.Add("0", false);
            this.valueMapper.Add("off", false);
            this.valueMapper.Add("no", false);
        }

        public bool CanConvert(Type type)
        {
            return type == typeof(bool);
        }

        public string ToString(object source, ICustomAttributeProvider customAttributeProvider)
        {
            return source.ToString();
        }

        public object ToValue(string source, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            if (source == null)
            {
                return true;
            }

            if (this.valueMapper.TryGetValue(source, out var result))
            {
                return result;
            }

            throw new ArgumentException();
        }

        public ConverterPriority Priority => ConverterPriority.Later;
    }
}