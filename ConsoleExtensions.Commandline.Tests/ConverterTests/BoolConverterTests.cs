namespace ConsoleExtensions.Commandline.Tests.ConverterTests
{
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

        public class Mock
        {
            public bool BoolValue { get; set; }
        }

    }
}