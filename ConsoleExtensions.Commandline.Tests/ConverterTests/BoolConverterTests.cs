// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoolConverterTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests.ConverterTests
{
    using System;
    using System.Linq;

    using ConsoleExtensions.Commandline.Converters.Custom;
    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Parser;

    using JetBrains.Annotations;

    using Xunit;

    /// <summary>
    ///     Class BoolConverterTests. Tests the boolean converter.
    /// </summary>
    public class BoolConverterTests
    {
        /// <summary>
        ///     Given a <see langword="bool" /> value when converting then the
        ///     <paramref name="expected" /> value should be set.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expected">if set to <c>true</c> [expected].</param>
        [Theory]
        [InlineData("True", true)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("1", true)]
        [InlineData("On", true)]
        [InlineData("Yes", true)]
        [InlineData(null, true)]
        [InlineData("False", false)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        [InlineData("0", false)]
        [InlineData("Off", false)]
        [InlineData("No", false)]
        public void GivenABoolValue_WhenConverting_ThenTheExpectedValueShouldBeSet(string input, bool expected)
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            sut.SetOption("BoolValue", input);

            // Assert
            Assert.Equal(expected, model.BoolValue);
            Assert.Equal(expected.ToString(), sut.GetOption("BoolValue")[0]);
        }

        /// <summary>
        ///     Given a <see langword="bool" /> value when setting to a invalid
        ///     value then the argument exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenABoolValue_WhenSettingToAInvalidValue_ThenTheArgumentExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            var actual = Record.Exception(() => sut.SetOption("BoolValue", "Invalid"));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("BoolValue", actualException.Name);
            Assert.Equal("Invalid", actualException.Value);
            Assert.Equal("Boolean", actualException.Type);
        }

        /// <summary>
        ///     Given a <see langword="bool" /> value annotation when checking
        ///     can convert then only boolean should return true.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expected">if set to <c>true</c> [expected].</param>
        [Theory]
        [InlineData(typeof(bool), true)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int), false)]
        public void GivenABoolValueAnnotation_WhenCheckingCanConvert_ThenOnlyBooleanShouldReturnTrue(
            Type input,
            bool expected)
        {
            // Arrange
            var attribute = new BoolValueAnnotationAttribute("true", "false");

            // Act
            var actual = attribute.CanConvert(input);

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///     Given a <see langword="bool" /> value with custom values when
        ///     setting and getting values then the custom values are used.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="expected">if set to <c>true</c> [expected].</param>
        [Theory]
        [InlineData("yeps", true)]
        [InlineData("Yeps", true)]
        [InlineData("YEPS", true)]
        [InlineData("nope", false)]
        [InlineData("Nope", false)]
        [InlineData("NOPE", false)]
        public void GivenABoolValueWithCustomValues_WhenSettingAndGettingValues_ThenTheCustomValuesAreUsed(
            string input,
            bool expected)
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            sut.SetOption("CustomBoolValue", input);

            // Assert
            Assert.Equal(expected, model.CustomBoolValue);
        }

        /// <summary>
        ///     Given a <see langword="bool" /> value with custom values when
        ///     setting and getting values then the standard values are invalid.
        /// </summary>
        /// <param name="input">The input.</param>
        [Theory]
        [InlineData("True")]
        [InlineData("true")]
        [InlineData("TRUE")]
        [InlineData("1")]
        [InlineData("On")]
        [InlineData("Yes")]
        [InlineData("False")]
        [InlineData("false")]
        [InlineData("FALSE")]
        [InlineData("0")]
        [InlineData("Off")]
        [InlineData("No")]
        public void GivenABoolValueWithCustomValues_WhenSettingAndGettingValues_ThenTheStandardValuesAreInvalid(
            string input)
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            var actual = Record.Exception(() => sut.SetOption("CustomBoolValue", input));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("CustomBoolValue", actualException.Name);
            Assert.Equal(input, actualException.Value);
            Assert.Equal("Boolean", actualException.Type);
        }

        /// <summary>
        ///     Given a call to a method with boolean value when calling with
        ///     values then the values are converted.
        /// </summary>
        [Fact]
        public void GivenACallToAMethodWithBooleanValue_WhenCallingWithValues_ThenTheValuesAreConverted()
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            var actual = sut.Invoke("TestMethod", "true", "yeps");

            // Assert
            Assert.Equal("True : True", actual);
        }

        /// <summary>
        ///     Given a model with a <see langword="bool" />
        ///     <paramref name="value" /> when getting the string representation
        ///     then the correct converter is used.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <param name="expected">The expected.</param>
        [Theory]
        [InlineData("CustomBoolValue", true, "yeps")]
        [InlineData("CustomBoolValue", false, "nope")]
        [InlineData("BoolValue", true, "True")]
        [InlineData("BoolValue", false, "False")]
        public void GivenAModelWithABoolValue_WhenGettingTheStringRepresentation_ThenTheCorrectConverterIsUsed(
            string field,
            bool value,
            string expected)
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            model.CustomBoolValue = value;
            model.BoolValue = value;
            var actual = sut.GetOption(field).First();

            // Assert
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///     Given a model with a none <see langword="bool" /> value when the
        ///     value is marked with the <see langword="bool" /> converter then
        ///     <see langword="throw" /> exception.
        /// </summary>
        [Fact]
        public void GivenAModelWithANoneBoolValue_WhenTheValueIsMarkedWithTheBoolConverter_ThenThrowException()
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            var actualOnSet = Record.Exception(() => sut.SetOption("NotABoolValue", "False"));
            var actualOnGet = Record.Exception(() => sut.GetOption("NotABoolValue"));

            // Assert
            var onSet = actualOnSet as InvalidArgumentFormatException;
            Assert.NotNull(onSet);
            Assert.Equal("NotABoolValue", onSet.Name);

            var onGet = actualOnGet as ArgumentException;
            Assert.NotNull(onGet);
        }

        /// <summary>
        ///     Class Mock.
        /// </summary>
        public class Mock
        {
            /// <summary>
            ///     Gets or sets a value indicating whether the Boolean value is
            ///     set using standard converter.
            /// </summary>
            public bool BoolValue { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether the boolean value is
            ///     set using a custom converter.
            /// </summary>
            [BoolValueAnnotation("yeps", "nope")]
            public bool CustomBoolValue { get; set; }

            /// <summary>
            ///     Gets or sets the not a <see langword="bool" /> value. For
            ///     testing error handling.
            /// </summary>
            [BoolValueAnnotation("yeps", "nope")]
            [UsedImplicitly]
            public string NotABoolValue { get; set; }

            /// <summary>
            /// Test method for testing the boolean converter when calling methods.
            /// </summary>
            /// <param name="normal">if set to <c>true</c> [normal].</param>
            /// <param name="custom">if set to <c>true</c> [custom].</param>
            /// <returns>The parameters as string.</returns>
            [UsedImplicitly]
            public string TestMethod(bool normal, [BoolValueAnnotation("yeps", "nope")] bool custom)
            {
                return normal + " : " + custom;
            }
        }
    }
}