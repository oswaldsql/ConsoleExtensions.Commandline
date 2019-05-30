// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArgumentParserTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests
{
    using System.Linq;

    using ConsoleExtensions.Commandline.Arguments;

    using Xunit;

    /// <summary>
    ///     Class ArgumentParserTests. Tests the argument parser.
    /// </summary>
    public class ArgumentParserTests
    {
        /// <summary>
        ///     Given a argument array
        ///     when parsing
        ///     then the values should be returned.
        /// </summary>
        [Fact]
        public void GivenAArgumentArray_WhenParsing_ThenTheValuesShouldBeReturned()
        {
            // Arrange

            // Act
            var actual = ArgumentParser.Parse(
                "command",
                "arg1",
                "arg2",
                "arg3",
                "-param1",
                "-param2",
                "singleparamvalue",
                "-param3",
                "value1",
                "value2",
                "value3");

            // Assert
            Assert.Equal("command", actual.Command);
            Assert.Equal(3, actual.Arguments.Length);
            Assert.Equal(3, actual.Properties.Count);

            Assert.Equal("arg1|arg2|arg3", string.Join('|', actual.Arguments));

            Assert.Contains("param1", actual.Properties.Keys);
            Assert.Contains("param2", actual.Properties.Keys);
            Assert.Contains("param3", actual.Properties.Keys);
            Assert.DoesNotContain("param4", actual.Properties.Keys);

            Assert.Empty(actual.Properties["param1"]);
            Assert.Single(actual.Properties["param2"]);
            Assert.Equal("singleparamvalue", actual.Properties["param2"].First());
            Assert.Equal(3, actual.Properties["param3"].Count);
            Assert.Equal("value1|value2|value3", string.Join('|', actual.Properties["param3"]));
        }
    }
}