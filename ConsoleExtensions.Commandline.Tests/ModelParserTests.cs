// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelParserTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests
{
    using System.ComponentModel;

    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    /// <summary>
    ///     Class ModelParserTests. Tests the model parser.
    /// </summary>
    public class ModelParserTests
    {
        /// <summary>
        ///     Given a object with a method with metadata when parsing then the
        ///     matching command should be populated with metadata.
        /// </summary>
        [Fact]
        public void
            GivenAObjectWithAMethodWithMetadata_WhenParsing_ThenTheMatchingCommandShouldBePopulatedWithMetadata()
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

        /// <summary>
        ///     Given a object with a simple method when parsing then the
        ///     matching command should be populated with metadata.
        /// </summary>
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

        /// <summary>
        ///     Given a object with a simple property when parsing then the
        ///     matching option should be populated with metadata.
        /// </summary>
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

        /// <summary>
        ///     Given a object with metadata when parsing then the matching
        ///     option should be populated with metadata.
        /// </summary>
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

        /// <summary>
        ///     Class Mock.
        /// </summary>
        public class Mock
        {
            /// <summary>
            ///     Gets or sets the option.
            /// </summary>
            public string Option { get; set; }

            /// <summary>
            ///     Gets or sets the option with metadata.
            /// </summary>
            [DisplayName("WithMetadata")]
            [Description("Option with metadata.")]
            public string OptionWithMetadata { get; set; }

            /// <summary>
            ///     Method with metadata.
            /// </summary>
            /// <returns>
            ///     Mock data.
            /// </returns>
            [DisplayName("MetadataDisplayName")]
            [Description("MetadataDescription")]
            public string MethodWithMetadata()
            {
                return "MethodWithMetadataResult";
            }

            /// <summary>
            ///     Simple method.
            /// </summary>
            /// <returns>
            ///     Mock data.
            /// </returns>
            public string SimpleMethod()
            {
                return "SimpleMethodResult";
            }
        }
    }
}