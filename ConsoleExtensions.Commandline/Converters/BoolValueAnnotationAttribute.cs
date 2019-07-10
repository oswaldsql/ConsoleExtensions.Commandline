using System;

namespace ConsoleExtensions.Commandline.Converters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class BoolValueAnnotationAttribute : CustomConverterAttribute
    {
        public string TrueValue { get; }

        public string FalseValue { get; }

        public BoolValueAnnotationAttribute(string trueValue, string falseValue)
        {
            this.TrueValue = trueValue;
            this.FalseValue = falseValue;
        }

        public override object ConvertToValue(string value, Type type)
        {
            if (value.Equals(this.TrueValue, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            if (value.Equals(this.FalseValue, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            throw new ArgumentException();
        }

        public override object ConvertToString(object value)
        {
            var b = value as bool?;

            if(b == null) throw new ArgumentException();

            return b == true ? TrueValue : FalseValue;
        }

        /// <summary>
        ///     Determines whether this instance can convert the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance can convert the specified type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type type)
        {
            return type == typeof(bool);
        }
    }
}