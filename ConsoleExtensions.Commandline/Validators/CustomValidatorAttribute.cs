namespace ConsoleExtensions.Commandline.Validators
{
    using System;

    /// <summary>
    ///     Class CustomValidatorAttribute. Implements the <see cref="Attribute" />. Used for adding custom validation to properties.
    /// </summary>
    /// <seealso cref="T:System.Attribute" />
    public abstract class CustomValidatorAttribute : Attribute
    {
        /// <summary>
        ///     Determines whether this instance can validate the specified
        ///     type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if this instance can validate the specified type;
        ///     otherwise, <c>false</c> .
        /// </returns>
        public abstract bool CanValidate(Type type);

        /// <summary>
        ///     Validates the specified value. Throws the appropriate exception if the value is not valid.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        public abstract void Validate(object value);
    }
}