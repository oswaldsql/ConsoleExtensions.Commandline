namespace ConsoleExtensions.Commandline.Tests
{
    using System;

    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    public class ModelMapValueConversionForOptionsTests
    {
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
            modelMap["BoolOption"] = value;

            // Assert
            Assert.Equal(expected, model.BoolOption);
        }

        [Fact]
        public void GivenABoolOption_WhenSettingToInvalidValue_ThenExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            var actual = Record.Exception(() => modelMap["BoolOption"] = "Invalid");

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.Equal("Invalid", actualException.StringValue);
            Assert.Equal("Boolean", actualException.Property.PropertyType.Name);
        }

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
            modelMap["DayOfWeek"] = value;

            // Assert
            Assert.Equal(expected, (int)model.DayOfWeek);
        }

        [Fact]
        public void GivenAEnumOption_WhenSettingToInvalidValue_ThenExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            var actual = Record.Exception(() => modelMap["DayOfWeek"] = "Invalid");

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.Equal("Invalid", actualException.StringValue);
            Assert.Equal("DayOfWeek", actualException.Property.PropertyType.Name);
        }

        [Fact]
        public void GivenAIntOption_WhenSettingOptionToString_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var actual = Record.Exception(() => modelMap["IntOption"] = "abc");

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.Equal("abc", actualException.StringValue);
            Assert.Equal("Int32", actualException.Property.PropertyType.Name);
        }

        [Fact]
        public void GivenAIntOption_WhenSettingOptionToToLargeANumber_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var actual = Record.Exception(() => modelMap["IntOption"] = "1.35");

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.Equal("1.35", actualException.StringValue);
            Assert.Equal("Int32", actualException.Property.PropertyType.Name);
        }

        [Fact]
        public void GivenAIntOption_WhenSettingOptionToTooLargeANumber_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var large = (long)int.MaxValue + 1;
            var actual = Record.Exception(() => modelMap["IntOption"] = large.ToString());

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.Equal("2147483648", actualException.StringValue);
            Assert.Equal("Int32", actualException.Property.PropertyType.Name);
        }

        [Fact]
        public void GivenAIntValue_WhenSettingOption_ThenValueIsSet()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            // Act
            modelMap["IntOption"] = "123";

            // Assert
            Assert.Equal("123", modelMap["IntOption"]);
            Assert.Equal(123, model.IntOption);
        }

        public class Mock
        {
            public bool BoolOption { get; set; }

            public DayOfWeek DayOfWeek { get; set; }

            public int IntOption { get; set; }
        }
    }
}