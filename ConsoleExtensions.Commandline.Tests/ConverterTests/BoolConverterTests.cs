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

    using ConsoleExtensions.Commandline.Converters;
    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    public class BoolConverterTests
    {
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

        [Theory]
        [InlineData("yeps", true)]
        [InlineData("Yeps", true)]
        [InlineData("YEPS", true)]
        [InlineData("nope", false)]
        [InlineData("Nope", false)]
        [InlineData("NOPe", false)]
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
            Assert.Equal("CustomBoolValue", actualException.Name);
            Assert.Equal(input, actualException.Value);
            Assert.Equal("Boolean", actualException.Type);
        }

        [Theory]
        [InlineData("CustomBoolValue", true, "yeps")]
        [InlineData("CustomBoolValue", false, "nope")]
        [InlineData("BoolValue", true, "True")]
        [InlineData("BoolValue", false, "False")]
        public void GivenAModelWithABoolValue_WhenGettingTheStringRepresentation_ThenTheCorrectConverterIsUsed(string field, bool value, string expected)
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

        [Theory]
        [InlineData(typeof(bool), true)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int), false)]
        public void GivenABoolValueAnnotation_WhenChechingCanConvert_ThenOnlyBooleanShouldReturnTrue(Type input, bool expected)
        {
            // Arrange
            var attribute = new BoolValueAnnotationAttribute("true", "false");

            // Act
            var actual = attribute.CanConvert(input);
            // Assert
            Assert.Equal(expected, actual);
        }

        public class Mock
        {
            public bool BoolValue { get; set; }

            [BoolValueAnnotation("yeps", "nope")]
            public bool CustomBoolValue { get; set; }

            [BoolValueAnnotation("yeps", "nope")]
            public string NotABoolValue { get; set; }
        }
    }
}