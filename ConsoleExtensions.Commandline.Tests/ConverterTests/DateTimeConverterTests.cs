// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeConverterTests.cs" company="Lasse Sjørup">
//   Copyright (c)  Lasse Sjørup
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

    public class DateTimeConverterTests
    {
        [Fact]
        public void GivenAModelWithADateTimeProperty_WhenSettingTheValue_ThenExpectedValueIsSet()
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            sut.SetOption("DateTimeValue", "2019-01-01");
            var actual = sut.GetOption("DateTimeValue").First();

            // Assert
            Assert.Equal(new DateTime(2019,1,1), model.DateTimeValue);
            Assert.Equal("", actual);
        }

        [Fact]
        public void GivenAModelWithATimeSpanProperty_WhenSettingTheValue_ThenExpectedValueIsSet()
        {
            // Arrange
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            sut.SetOption("TimeSpanValue", "01:02:03");
            var actual = sut.GetOption("TimeSpanValue").First();

            // Assert
            Assert.Equal(3723, model.TimeSpanValue.TotalSeconds);
            Assert.Equal("01:02:03", actual);
        }


        public class Mock
        {
            public DateTime DateTimeValue { get; set; }

            public TimeSpan TimeSpanValue { get; set; }
        }
    }
}