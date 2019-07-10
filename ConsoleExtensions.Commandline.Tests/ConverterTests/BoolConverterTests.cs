namespace ConsoleExtensions.Commandline.Tests.ConverterTests
{
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
        [InlineData("nop", false)]
        [InlineData("Nop", false)]
        [InlineData("NOP", false)]
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

        public class Mock
        {
            public bool BoolValue { get; set; }

            [BoolValueAnnotation("yeps", "nop")]
            public bool CustomBoolValue { get; set; }
        }
    }
}