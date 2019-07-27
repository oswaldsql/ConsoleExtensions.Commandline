// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelMapValueConversionForCommandsTests.cs" company="Lasse Sjørup">
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
    ///     Class ModelMapValueConversionForCommandsTests.
    /// </summary>
    public class ModelMapValueConversionForCommandsTests
    {
        /// <summary>
        ///     Given a <see langword="bool" /> option when setting to given
        ///     <paramref name="value" /> then the <paramref name="value" />
        ///     should be set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expected">The expected value.</param>
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
            var actual = modelMap.Invoke("BoolMethod", value);

            // Assert
            Assert.Equal(expected.ToString(), actual);
        }

        /// <summary>
        ///     Given a <see langword="bool" /> option when setting to invalid
        ///     value then exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenABoolOption_WhenSettingToInvalidValue_ThenExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            var actual = Record.Exception(() => modelMap.Invoke("BoolMethod", "Invalid"));

            // Assert
            Assert.IsType<InvalidParameterFormatException>(actual);
            var actualException = actual as InvalidParameterFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("Invalid", actualException.Value);
            Assert.Equal("Boolean", actualException.ParameterInfo.ParameterType.Name);
        }

        /// <summary>
        ///     Given a <see langword="enum" /> option when setting to given
        ///     <paramref name="value" /> then the <paramref name="value" />
        ///     should be set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expected">The expected value.</param>
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
            var actual = modelMap.Invoke("DayOfWeekMethod", value);

            // Assert
            Assert.Equal(expected.ToString(), actual);
        }

        /// <summary>
        ///     Given a <see langword="enum" /> option when setting to invalid
        ///     value then exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenAEnumOption_WhenSettingToInvalidValue_ThenExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            var actual = Record.Exception(() => modelMap.Invoke("DayOfWeekMethod", "Invalid"));

            // Assert
            Assert.IsType<InvalidParameterFormatException>(actual);
            var actualException = actual as InvalidParameterFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("Invalid", actualException.Value);
            Assert.Equal("DayOfWeek", actualException.ParameterInfo.ParameterType.Name);
        }

        /// <summary>
        ///     Given a <see langword="int" /> option when setting option to
        ///     string then the correct exception is thrown value is set.
        /// </summary>
        [Fact]
        public void GivenAIntOption_WhenSettingOptionToString_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var actual = Record.Exception(() => modelMap.Invoke("IntMethod", "abc"));

            // Assert
            Assert.IsType<InvalidParameterFormatException>(actual);
            var actualException = actual as InvalidParameterFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("abc", actualException.Value);
            Assert.Equal("Int32", actualException.ParameterInfo.ParameterType.Name);
        }

        /// <summary>
        ///     Given a <see langword="int" /> option when setting option to to
        ///     large a number then the correct exception is thrown value is
        ///     set.
        /// </summary>
        [Fact]
        public void GivenAIntOption_WhenSettingOptionToToLargeANumber_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var actual = Record.Exception(() => modelMap.Invoke("IntMethod", "1.35"));

            // Assert
            Assert.IsType<InvalidParameterFormatException>(actual);
            var actualException = actual as InvalidParameterFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("1.35", actualException.Value);
            Assert.Equal("Int32", actualException.ParameterInfo.ParameterType.Name);
        }

        /// <summary>
        ///     Given a <see langword="int" /> option when setting option to too
        ///     large a number then the correct exception is thrown value is
        ///     set.
        /// </summary>
        [Fact]
        public void GivenAIntOption_WhenSettingOptionToTooLargeANumber_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var large = (long)int.MaxValue + 1;
            var actual = Record.Exception(() => modelMap.Invoke("IntMethod", large.ToString()));

            // Assert
            Assert.IsType<InvalidParameterFormatException>(actual);
            var actualException = actual as InvalidParameterFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("2147483648", actualException.Value);
            Assert.Equal("Int32", actualException.ParameterInfo.ParameterType.Name);
        }

        /// <summary>
        ///     Given a <see langword="int" /> value when setting option then
        ///     value is set.
        /// </summary>
        [Fact]
        public void GivenAIntValue_WhenSettingOption_ThenValueIsSet()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            var actual = modelMap.Invoke("IntMethod", "123");

            // Assert
            Assert.Equal("123", actual);
        }

        /// <summary>
        ///     Class Mock.
        /// </summary>
        public class Mock
        {
            /// <summary>
            ///     Method taking a <see langword="bool" />
            ///     <paramref name="value" /> and returning the string
            ///     representation.
            /// </summary>
            /// <param name="value">if set to <c>true</c> [value].</param>
            /// <returns>
            ///     The string representation.
            /// </returns>
            public string BoolMethod(bool value)
            {
                return value.ToString();
            }

            /// <summary>
            ///     Method taking a DayOfWeek <paramref name="value" /> and
            ///     returning the string representation.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>
            ///     The string representation.
            /// </returns>
            public string DayOfWeekMethod(DayOfWeek value)
            {
                return ((int)value).ToString();
            }

            /// <summary>
            ///     Method taking a <see langword="int" />
            ///     <paramref name="value" /> and returning the string
            ///     representation.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>
            ///     The string representation.
            /// </returns>
            public string IntMethod(int value)
            {
                return value.ToString();
            }
        }
    }
}