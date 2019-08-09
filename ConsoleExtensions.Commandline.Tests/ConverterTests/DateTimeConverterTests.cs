// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeConverterTests.cs" company="Lasse Sjørup">
//   Copyright (c)  Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests.ConverterTests
{
    using System;
    using System.Globalization;
    using System.Linq;

    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    /// <summary>
    ///     Class DateTimeConverterTests. Tests the Date Time converter.
    /// </summary>
    public class DateTimeConverterTests
    {
        /// <summary>
        ///     Given a model with a date time property when setting the value
        ///     then expected value is set.
        /// </summary>
        [Fact]
        public void GivenAModelWithADateTimeProperty_WhenSettingTheValue_ThenExpectedValueIsSet()
        {
            // Arrange
            CultureInfo.CurrentCulture = new CultureInfo("en-US")
                                             {
                                                 DateTimeFormat =
                                                     {
                                                         ShortDatePattern = "yyyy-MM-dd",
                                                         LongTimePattern = "HH:mm:ss.FFFFFFFK"
                                                     }
                                             };
            var model = new Mock();
            var sut = ModelParser.Parse(model);

            // Act
            sut.SetOption("DateTimeValue", "2019-01-13");
            var actual = sut.GetOption("DateTimeValue").First();

            // Assert
            Assert.Equal(new DateTime(2019, 1, 13), model.DateTimeValue);
            Assert.Equal("2019-01-13 00:00:00", actual);
        }

        /// <summary>
        ///     Given a model with a time span property when setting the value
        ///     then expected value is set.
        /// </summary>
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

        /// <summary>
        ///     Class Mock.
        /// </summary>
        public class Mock
        {
            /// <summary>
            ///     Gets or sets the date time value.
            /// </summary>
            public DateTime DateTimeValue { get; set; }

            /// <summary>
            ///     Gets or sets the time span value.
            /// </summary>
            public TimeSpan TimeSpanValue { get; set; }
        }
    }
}