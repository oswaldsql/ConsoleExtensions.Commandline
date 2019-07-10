using System;

namespace ConsoleExtensions.Commandline.Parser
{
    public abstract class CustomValidatorAttribute : Attribute
    {
        public abstract bool CanValidate(Type type);

        public abstract void Validate(object source);
    }

    public class MinMaxValidatorAttribute : CustomValidatorAttribute
    {
        public object Min { get; }
        public object Max { get; }

        public MinMaxValidatorAttribute(object min, object max)
        {
            Min = min;
            Max = max;
        }

        public override bool CanValidate(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type);
        }

        public override void Validate(object source)
        {
            var comparable = source as IComparable;

            if (comparable.CompareTo(Min) >= 0 && comparable.CompareTo(Max) <= 0)
            {

            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}