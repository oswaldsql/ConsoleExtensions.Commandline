// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueConverterTests.cs" company="Lasse Sjørup">
//   Copyright (c)  Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests
{
    using System;

    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    /// <summary>
    ///     Class ValueConverter.
    /// </summary>
    public class ValueConverterTests
    {
        /// <summary>
        ///     Given a custom option when setting option with value converter then value is set.
        /// </summary>
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

        /// <summary>
        ///     Given a existing converter when overwriting then the new
        ///     converter is used.
        /// </summary>
        [Fact]
        public void GivenAExistingConverter_WhenOverwriting_ThenTheNewConverterIsUsed()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            var toObjCalled = false;
            var toStringCalled = false;

            modelMap.AddValueConverter(
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
            modelMap.SetOption("IntOption", "123");

            // Assert
            Assert.Equal("123", modelMap.GetOption("IntOption")[0]);
            Assert.Equal(123, model.IntOption);
            Assert.True(toObjCalled);
            Assert.True(toStringCalled);
        }

        /// <summary>
        ///     Given the two identical converters when converting then the last
        ///     added converter should be used.
        /// </summary>
        [Fact]
        public void GivenTwoIdenticalConverters_WhenConverting_ThenTheLastAddedConverterShouldBeUsed()
        {
            // Arrange
            var model = new Mock();
            var modelMap = ModelParser.Parse(model);

            var lastConverterCalled = false;
            var firstConverterCalled = false;
            modelMap.AddValueConverter(
                s =>
                    {
                        firstConverterCalled = true;
                        return new CustomType(s);
                    },
                uri => throw new NotImplementedException());
            modelMap.AddValueConverter(
                s =>
                    {
                        lastConverterCalled = true;
                        return new CustomType(s);
                    },
                uri => throw new NotImplementedException());

            // Act
            modelMap.SetOption("CustomTypeOption", "CustomValue");

            // Assert
            Assert.False(firstConverterCalled);
            Assert.True(lastConverterCalled);
        }

        /// <summary>
        ///     Class CustomType.
        /// </summary>
        public class CustomType
        {
            /// <summary>
            ///     Initializes a new instance of the
            ///     <see cref="ValueConverterTests.CustomType" /> class.
            /// </summary>
            /// <param name="internalValue">
            ///     The <see langword="internal" /> value.
            /// </param>
            public CustomType(string internalValue)
            {
                this.InternalValue = internalValue.ToLower();
            }

            /// <summary>
            ///     Gets the <see langword="internal" /> value.
            /// </summary>
            /// <value>
            ///     The <see langword="internal" /> value.
            /// </value>
            public string InternalValue { get; }
        }

        /// <summary>
        ///     Class Mock.
        /// </summary>
        public class Mock
        {
            /// <summary>
            ///     Gets or sets the custom type option.
            /// </summary>
            /// <value>
            ///     The custom type option.
            /// </value>
            public CustomType CustomTypeOption { get; set; }

            /// <summary>
            ///     Gets or sets the <see langword="int" /> option.
            /// </summary>
            /// <value>
            ///     The <see langword="int" /> option.
            /// </value>
            public int IntOption { get; set; }
        }
    }
}