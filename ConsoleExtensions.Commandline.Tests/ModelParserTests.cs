// ReSharper disable StyleCop.SA1600
// ReSharper disable ExceptionNotDocumented
namespace ConsoleExtensions.Commandline.Tests
{
    using System.ComponentModel;

    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    public class ModelParserTests
    {
        [Fact]
        public void GivenAObjectWithASimpleMethod_WhenParsing_ThenTheMatchingCommandShouldBePopulatedWithMetadata()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Contains(actual.Commands, pair => pair.Key == "SimpleMethod");
            var command = actual.Commands["SimpleMethod"];
            Assert.Equal("SimpleMethod", command.Name);
            Assert.Equal("Simple Method", command.DisplayName);
            Assert.Null(command.Description);
            Assert.Equal(model, command.Source);
            Assert.Equal("SimpleMethod", command.Method.Name);
        }

        [Fact]
        public void GivenAObjectWithASimpleProperty_WhenParsing_ThenTheMatchingOptionShouldBePopulatedWithMetadata()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Contains(actual.Options, pair => pair.Key == "Option");
            var actualOption = actual.Options["Option"];
            Assert.Equal("Option", actualOption.Name);
            Assert.Equal("Option", actualOption.DisplayName);
            Assert.Null(actualOption.Description);
            Assert.Equal(model, actualOption.Source);
            Assert.Equal("Option", actualOption.Property.Name);
        }

        [Fact]
        public void GivenAObjectWithMetadata_WhenParsing_ThenTheMatchingOptionShouldBePopulatedWithMetadata()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Contains(actual.Options, pair => pair.Key == "OptionWithMetadata");
            var actualOption = actual.Options["OptionWithMetadata"];
            Assert.Equal("OptionWithMetadata", actualOption.Name);
            Assert.Equal("WithMetadata", actualOption.DisplayName);
            Assert.Equal("Option with metadata.", actualOption.Description);
            Assert.Equal(model, actualOption.Source);
            Assert.Equal("OptionWithMetadata", actualOption.Property.Name);
        }

        [Fact]
        public void GivenAObjectWithAMethodWithMetadata_WhenParsing_ThenTheMatchingCommandShouldBePopulatedWithMetadata()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Contains(actual.Commands, pair => pair.Key == "MethodWithMetadata");
            var command = actual.Commands["MethodWithMetadata"];
            Assert.Equal("MethodWithMetadata", command.Name);
            Assert.Equal("MetadataDisplayName", command.DisplayName);
            Assert.Equal("MetadataDescription", command.Description);
            Assert.Equal(model, command.Source);
            Assert.Equal("MethodWithMetadata", command.Method.Name);
            Assert.Equal("MethodWithMetadataResult", actual.Invoke("MethodWithMetadata"));
        }

        public class Mock
        {
            public string Option { get; set; }

            [DisplayName("WithMetadata")]
            [Description("Option with metadata.")]
            public string OptionWithMetadata { get; set; }

            public string SimpleMethod()
            {
                return "SimpleMethodResult";
            }

            [DisplayName("MetadataDisplayName")]
            [Description("MetadataDescription")]
            public string MethodWithMetadata()
            {
                return "MethodWithMetadataResult";
            }
        }
    }
}