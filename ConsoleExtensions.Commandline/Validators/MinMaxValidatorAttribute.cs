namespace ConsoleExtensions.Commandline.Validators
{
    using System;

    using ConsoleExtensions.Commandline.Parser;

    /// <summary>
    ///     Class MinMaxValidatorAttribute. Validates a value to be between the
    ///     min and max value. Implements the
    ///     <see cref="CustomValidatorAttribute" />
    /// </summary>
    /// <seealso cref="T:ConsoleExtensions.Commandline.Parser.CustomValidatorAttribute" />
    public class MinMaxValidatorAttribute : CustomValidatorAttribute
    {
        /// <summary>
        ///     Determines the maximum value of the parameters.
        /// </summary>
        private readonly object max;

        /// <summary>
        ///     Determines the minimum value of the parameters.
        /// </summary>
        private readonly object min;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="MinMaxValidatorAttribute" /> class.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public MinMaxValidatorAttribute(object min, object max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        ///     Determines whether this instance can validate the specified
        ///     type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if this instance can validate the specified type;
        ///     otherwise, <c>false</c> .
        /// </returns>
        public override bool CanValidate(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type);
        }

        /// <summary>
        ///     Validates the specified source.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <exception cref="System.ArgumentException">
        ///     Thrown if the <paramref name="value" /> is out of bounds.
        /// </exception>
        public override void Validate(object value)
        {
            var comparable = value as IComparable;

            if (comparable == null)
            {
                throw new ArgumentException();
            }

            if (comparable.CompareTo(this.min) < 0)
            {
                throw new ArgumentException();
            }

            if (comparable.CompareTo(this.max) > 0)
            {
                throw new ArgumentException();
            }
        }
    }
}