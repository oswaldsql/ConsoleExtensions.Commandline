// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinMaxValidatorTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ConsoleExtensions.Commandline.Tests.ValidatorTests
{
    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Parser;
    using ConsoleExtensions.Commandline.Validators;

    using Xunit;

    /// <summary>
    /// Class MinMaxValidatorTests. Tests the Min Max Validator.
    /// </summary>
    public class MinMaxValidatorTests
    {
        /// <summary>
        /// Given a property not implementing comparable
        /// when validating minimum maximum
        /// then exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenAPropertyNotImplementingComparable_WhenValidatingMinMax_ThenExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            var actual = Record.Exception(() => sut.SetOption("BoolValue", "true"));

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<InvalidArgumentFormatException>(actual);
        }

        /// <summary>
        /// Given a property with a minimum maximum validation
        /// when setting the value outside the range
        /// then argument exception is thrown.
        /// </summary>
        /// <param name="value">The value.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("9")]
        [InlineData("24")]
        public void
            GivenAPropertyWithAMinMaxValidation_WhenSettingTheValueOutsideTheRange_ThenArgumentExceptionIsThrown(
                string value)
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            var exception = Record.Exception(() => sut.SetOption("IntValue", value));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(exception);
        }

        /// <summary>
        /// Given a property with a minimum maximum validation
        /// when setting the value within range
        /// then the value is set.
        /// </summary>
        [Fact]
        public void GivenAPropertyWithAMinMaxValidation_WhenSettingTheValueWithinRange_ThenTheValueIsSet()
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            sut.SetOption("IntValue", "10");

            // Assert
            Assert.Equal(10, model.IntValue);
        }

        /// <summary>
        /// Class Mock.
        /// </summary>
        public class Mock
        {
            /// <summary>
            /// Gets or sets a value indicating whether [bool value].
            /// </summary>
            /// <value><c>true</c> if [bool value]; otherwise, <c>false</c>.</value>
            [MinMaxValidator(10, 20)]
            public bool BoolValue { get; set; }

            /// <summary>
            /// Gets or sets the int value.
            /// </summary>
            /// <value>The int value.</value>
            [MinMaxValidator(10, 23)]
            public int IntValue { get; set; }
        }
    }
}