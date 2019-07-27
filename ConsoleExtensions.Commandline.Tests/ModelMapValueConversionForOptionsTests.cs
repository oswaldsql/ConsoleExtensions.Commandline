// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelMapValueConversionForOptionsTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests
{
    using System;

    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    /// <summary>
    /// Class ModelMapValueConversionForOptionsTests.
    /// </summary>
    public class ModelMapValueConversionForOptionsTests
    {
        /// <summary>
        /// Given a bool option when setting to given value then the value should be set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expected">The expected result.</param>
        [Theory]
        [InlineData("True", true)]
        [InlineData("TRUE", true)]
        [InlineData("true", true)]
        [InlineData("False", false)]
        [InlineData("FALSE", false)]
        [InlineData("false", false)]
        public void GivenABoolOption_WhenSettingToGivenValue_ThenTheValueShouldBeSet(string value, bool expected)
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            modelMap.SetOption("BoolOption", value);

            // Assert
            Assert.Equal(expected, model.BoolOption);
        }

        /// <summary>
        /// Given a bool option when setting to invalid value then exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenABoolOption_WhenSettingToInvalidValue_ThenExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            var actual = Record.Exception(() => modelMap.SetOption("BoolOption", "Invalid"));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("Invalid", actualException.Value);
            Assert.Equal("Boolean", actualException.Property.PropertyType.Name);
        }

        /// <summary>
        /// Given a enum option when setting to given value then the value should be set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expected">The expected.</param>
        [Theory]
        [InlineData("monday", 1)]
        [InlineData("Monday", 1)]
        [InlineData("MONDAY", 1)]
        [InlineData("sunday", 0)]
        public void GivenAEnumOption_WhenSettingToGivenValue_ThenTheValueShouldBeSet(string value, int expected)
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            modelMap.SetOption("DayOfWeek", value);

            // Assert
            Assert.Equal(expected, (int)model.DayOfWeek);
        }

        /// <summary>
        /// Given a enum option when setting to invalid value then exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenAEnumOption_WhenSettingToInvalidValue_ThenExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            var actual = Record.Exception(() => modelMap.SetOption("DayOfWeek", "Invalid"));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("Invalid", actualException.Value);
            Assert.Equal("DayOfWeek", actualException.Property.PropertyType.Name);
        }

        /// <summary>
        /// Given a int option when setting option to string then the correct exception is thrown value is set.
        /// </summary>
        [Fact]
        public void GivenAIntOption_WhenSettingOptionToString_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var actual = Record.Exception(() => modelMap.SetOption("IntOption", "abc"));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("abc", actualException.Value);
            Assert.Equal("Int32", actualException.Type);
        }

        /// <summary>
        /// Given a int option when setting option to to large a number then the correct exception is thrown value is set.
        /// </summary>
        [Fact]
        public void GivenAIntOption_WhenSettingOptionToToLargeANumber_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var actual = Record.Exception(() => modelMap.SetOption("IntOption", "1.35"));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("1.35", actualException.Value);
            Assert.Equal("Int32", actualException.Property.PropertyType.Name);
        }

        /// <summary>
        /// Given a int option when setting option to too large a number then the correct exception is thrown value is set.
        /// </summary>
        [Fact]
        public void GivenAIntOption_WhenSettingOptionToTooLargeANumber_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var large = (long)int.MaxValue + 1;
            var actual = Record.Exception(() => modelMap.SetOption("IntOption", large.ToString()));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("2147483648", actualException.Value);
            Assert.Equal("Int32", actualException.Property.PropertyType.Name);
        }

        /// <summary>
        /// Given a int value when setting option then value is set.
        /// </summary>
        [Fact]
        public void GivenAIntValue_WhenSettingOption_ThenValueIsSet()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            modelMap.SetOption("IntOption", "123");

            // Assert
            Assert.Equal("123", modelMap.GetOption("IntOption")[0]);
            Assert.Equal(123, model.IntOption);
        }

        /// <summary>
        /// Given a string value when setting option then value is set.
        /// </summary>
        [Fact]
        public void GivenAStringValue_WhenSettingOption_ThenValueIsSet()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            modelMap.SetOption("StringOption", "123");

            // Assert
            Assert.Equal("123", modelMap.GetOption("StringOption")[0]);
            Assert.Equal("123", model.StringOption);
        }

        /// <summary>
        /// Class CustomType.
        /// </summary>
        public class CustomType
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CustomType"/> class.
            /// </summary>
            /// <param name="internalValue">The internal value.</param>
            public CustomType(string internalValue)
            {
                this.InternalValue = internalValue.ToLower();
            }

            /// <summary>
            /// Gets the internal value.
            /// </summary>
            /// <value>The internal value.</value>
            public string InternalValue { get; }
        }

        /// <summary>
        /// Class Mock.
        /// </summary>
        public class Mock
        {
            /// <summary>
            /// Gets or sets a value indicating whether [bool option].
            /// </summary>
            /// <value><c>true</c> if [bool option]; otherwise, <c>false</c>.</value>
            public bool BoolOption { get; set; }

            /// <summary>
            /// Gets or sets the day of week.
            /// </summary>
            /// <value>The day of week.</value>
            public DayOfWeek DayOfWeek { get; set; }

            /// <summary>
            /// Gets or sets the int option.
            /// </summary>
            /// <value>The int option.</value>
            public int IntOption { get; set; }

            /// <summary>
            /// Gets or sets the string option.
            /// </summary>
            /// <value>The string option.</value>
            public string StringOption { get; set; }
        }
    }
}