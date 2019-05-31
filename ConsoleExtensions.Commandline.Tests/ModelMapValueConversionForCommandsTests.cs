namespace ConsoleExtensions.Commandline.Tests
{
    using System;

    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    public class ModelMapValueConversionForCommandsTests
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
            var actual = modelMap.Invoke("BoolMethod", value);

            // Assert
            Assert.Equal(expected.ToString(), actual);
        }

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
            Assert.Equal("Invalid", actualException.Value);
            Assert.Equal("Boolean", actualException.ParameterInfo.ParameterType.Name);
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
            var actual = modelMap.Invoke("DayOfWeekMethod", value);

            // Assert
            Assert.Equal(expected.ToString(), actual);
        }

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
            Assert.Equal("Invalid", actualException.Value);
            Assert.Equal("DayOfWeek", actualException.ParameterInfo.ParameterType.Name);
        }

        [Fact]
        public void GivenAIntOption_WhenSettingOptionToString_ThenTheCorrectExceptionIsThrownValueIsSet()
        {
            // Arrange
            var modelMap = ModelParser.Parse(new Mock());

            // Act
            var actual = Record.Exception(() => modelMap.Invoke("IntMethod",  "abc"));

            // Assert
            Assert.IsType<InvalidParameterFormatException>(actual);
            var actualException = actual as InvalidParameterFormatException;
            Assert.Equal("abc", actualException.Value);
            Assert.Equal("Int32", actualException.ParameterInfo.ParameterType.Name);
        }

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
            Assert.Equal("1.35", actualException.Value);
            Assert.Equal("Int32", actualException.ParameterInfo.ParameterType.Name);
        }

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
            Assert.Equal("2147483648", actualException.Value);
            Assert.Equal("Int32", actualException.ParameterInfo.ParameterType.Name);
        }

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

        public class Mock
        {
            public string IntMethod(int value)
            {
                return value.ToString();
            }

            public string BoolMethod(bool value)
            {
                return value.ToString();
            }

            public string DayOfWeekMethod(DayOfWeek value)
            {
                return ((int)value).ToString();
            }
        }
    }
}