using ConsoleExtensions.Commandline.Exceptions;
using ConsoleExtensions.Commandline.Parser;
using Xunit;

namespace ConsoleExtensions.Commandline.Tests.ValidatorTests
{
    using ConsoleExtensions.Commandline.Validators;

    public class MinMaxValidatorTests
    {
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

        public class Mock
        {
            [MinMaxValidator(10, 23)] public int IntValue { get; set; }
        }
    }
}