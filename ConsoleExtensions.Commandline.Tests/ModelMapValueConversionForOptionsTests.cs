// ReSharper disable StyleCop.SA1600
// ReSharper disable ExceptionNotDocumented
namespace ConsoleExtensions.Commandline.Tests
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    using ConsoleExtensions.Commandline.Converters;
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
            modelMap.SetOption("BoolOption", value);

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
            var actual = Record.Exception(() => modelMap.SetOption("BoolOption", "Invalid"));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("Invalid", actualException.Value);
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
            modelMap.SetOption("DayOfWeek", value);

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
            var actual = Record.Exception(() => modelMap.SetOption("DayOfWeek", "Invalid"));

            // Assert
            Assert.IsType<InvalidArgumentFormatException>(actual);
            var actualException = actual as InvalidArgumentFormatException;
            Assert.NotNull(actualException);
            Assert.Equal("Invalid", actualException.Value);
            Assert.Equal("DayOfWeek", actualException.Property.PropertyType.Name);
        }

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

        public class Mock
        {
            public bool BoolOption { get; set; }

            public DayOfWeek DayOfWeek { get; set; }

            public int IntOption { get; set; }

            public string StringOption { get; set; }
        }

        public class CustomType
        {
            public CustomType(string internalValue)
            {
                this.InternalValue = internalValue.ToLower();
            }

            public string InternalValue { get;  }
        }
    }

    public class ValueConverter
    {
        // Tests
        // Can overwrite existing converters
        // Can add custom converter

        [Fact]
        public void GivenACustomOption_WhenSettingOptionWithValueConverter_ThenValueIsSet()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            modelMap.AddValueConverter(s => new CustomType(s), uri => uri.InternalValue.ToUpper());

            // Act
            modelMap.SetOption("CustomTypeOption", "CustomValue");

            // Assert
            Assert.Equal("CUSTOMVALUE", modelMap.GetOption("CustomTypeOption")[0]);
            Assert.Equal("customvalue", model.CustomTypeOption.InternalValue);
        }

        [Fact]
        public void GivenAExistingConverter_WhenOverwriting_ThenTheNewConverterIsUsed()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            bool toObjCalled = false;
            bool toStringCalled = false;

            modelMap.AddValueConverter<int>(
                s =>
                    {
                        toObjCalled = true;
                        return int.Parse(s);
                    },
                o =>
                    {
                        toStringCalled = true;
                        return o.ToString();
                    });

            // Act
            modelMap.SetOption("IntOption",  "123");

            // Assert
            Assert.Equal("123", modelMap.GetOption("IntOption")[0]);
            Assert.Equal(123, model.IntOption);
            Assert.True(toObjCalled);
            Assert.True(toStringCalled);
        }

        [Fact]
        public void GivenTwoIdenticalConverters_WhenConverting_ThenTheLastAddedConverterShouldBeUsed()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            bool lastConverterCalled = false;
            bool firstConverterCalled = false;
            modelMap.AddValueConverter(s =>
                {
                    firstConverterCalled = true;
                    return new CustomType(s);
                }, uri => throw new NotImplementedException());
            modelMap.AddValueConverter(
                s =>
                    {
                        lastConverterCalled = true;
                        return new CustomType(s);
                    }, uri => throw new NotImplementedException());

            // Act
            modelMap.SetOption("CustomTypeOption", "CustomValue");

            // Assert
            Assert.False(firstConverterCalled);
            Assert.True(lastConverterCalled);
        }



        public class Mock
        {
            public int IntOption { get; set; }

            public CustomType CustomTypeOption { get; set; }
        }

        public class CustomType
        {
            public CustomType(string internalValue)
            {
                this.InternalValue = internalValue.ToLower();
            }

            public string InternalValue { get; }
        }
    }
}