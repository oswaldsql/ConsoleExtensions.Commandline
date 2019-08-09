// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelMapInvokeTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests
{
    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    /// <summary>
    ///     Class ModelMapInvokeTests.
    /// </summary>
    public class ModelMapInvokeTests
    {
        /// <summary>
        ///     Given a method with default value parameter when invoking
        ///     without value then the default value is used.
        /// </summary>
        [Fact]
        public void GivenAMethodWithDefaultValueParameter_WhenInvokingWithoutValue_ThenTheDefaultValueIsUsed()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Equal("Result", actual.Invoke("MethodWithDefaultValue"));
        }

        /// <summary>
        ///     Given a method with default value parameter when invoking with
        ///     value then the value is used.
        /// </summary>
        [Fact]
        public void GivenAMethodWithDefaultValueParameter_WhenInvokingWithValue_ThenTheValueIsUsed()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Equal("OtherResult", actual.Invoke("MethodWithDefaultValue", "OtherResult"));
        }

        /// <summary>
        ///     Given a method without parameters when invoking without values
        ///     then result is returned.
        /// </summary>
        [Fact]
        public void GivenAMethodWithoutParameters_WhenInvokingWithoutValues_ThenResultIsReturned()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Equal("SimpleMethodResult", actual.Invoke("SimpleMethod"));
        }

        /// <summary>
        ///     Given a method with two arguments when only setting one then the
        ///     correct exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenAMethodWithTwoArguments_WhenOnlySettingOne_ThenTheCorrectExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            var actualException = Record.Exception(() => sut.Invoke("MethodWithTwoArguments", "value1"));

            // Assert
            Assert.IsType<MissingArgumentException>(actualException);
            var actual = actualException as MissingArgumentException;
            Assert.NotNull(actual);
            Assert.Equal("value2", actual.Argument);
        }

        /// <summary>
        ///     Given a method with two arguments when trying to set3 then the
        ///     correct exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenAMethodWithTwoArguments_WhenTryingToSet3_ThenTheCorrectExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            var actualException =
                Record.Exception(() => sut.Invoke("MethodWithTwoArguments", "value1", "value2", "value3"));

            // Assert
            Assert.IsType<TooManyArgumentsException>(actualException);
            var actual = actualException as TooManyArgumentsException;
            Assert.NotNull(actual);
            Assert.Equal(2, actual.Arguments.Length);
        }

        /// <summary>
        ///     Given a model when getting a unknown option then the correct
        ///     exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenAModel_WhenGettingAUnknownOption_ThenTheCorrectExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            var dummy = "PrevValue";
            var actualException = Record.Exception(() => dummy = sut.GetOption("UnknownOption")[0]);

            // Assert
            Assert.IsType<UnknownOptionException>(actualException);
            var actual = actualException as UnknownOptionException;
            Assert.NotNull(actual);
            Assert.Equal("UnknownOption", actual.Option);
        }

        /// <summary>
        ///     Given a model when invoking a unknown command then the correct
        ///     exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenAModel_WhenInvokingAUnknownCommand_ThenTheCorrectExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            var actualException = Record.Exception(() => sut.Invoke("UnknownMethod"));

            // Assert
            Assert.IsType<UnknownCommandException>(actualException);
            var actual = actualException as UnknownCommandException;
            Assert.NotNull(actual);
            Assert.Equal("UnknownMethod", actual.Command);
        }

        /// <summary>
        ///     Given a model when setting a unknown option then the correct
        ///     exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenAModel_WhenSettingAUnknownOption_ThenTheCorrectExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            var actualException = Record.Exception(() => sut.GetOption("UnknownOption")[0] == "test");

            // Assert
            Assert.IsType<UnknownOptionException>(actualException);
            var actual = actualException as UnknownOptionException;
            Assert.NotNull(actual);
            Assert.Equal("UnknownOption", actual.Option);
        }

        /// <summary>
        ///     Given a option when option is set then the property changes.
        /// </summary>
        [Fact]
        public void GivenAOption_WhenOptionIsSet_ThenThePropertyChanges()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);
            actual.SetOption("Option", "OptionValue");

            // Assert
            Assert.Equal("OptionValue", actual.GetOption("Option")[0]);
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
            ///     Method with default value.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>
            ///     The value.
            /// </returns>
            public string MethodWithDefaultValue(string value = "Result")
            {
                return value;
            }

            /// <summary>
            ///     Method with two arguments.
            /// </summary>
            /// <param name="value1">The value1.</param>
            /// <param name="value2">The value2.</param>
            /// <returns>
            ///     The two values concatenated.
            /// </returns>
            public string MethodWithTwoArguments(string value1, string value2)
            {
                return $"{value1} : {value2}";
            }

            /// <summary>
            ///     Simple method.
            /// </summary>
            /// <returns>
            ///     Dummy string.
            /// </returns>
            public string SimpleMethod()
            {
                return "SimpleMethodResult";
            }
        }
    }
}