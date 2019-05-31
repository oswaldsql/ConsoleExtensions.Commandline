namespace ConsoleExtensions.Commandline.Tests
{
    using ConsoleExtensions.Commandline.Exceptions;
    using ConsoleExtensions.Commandline.Parser;

    using Xunit;

    public class ModelMapInvokeTests
    {
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


        [Fact]
        public void GivenAMethodWithDefaultValueParameter_WhenInvokingWithValue_ThenTheValueIsUsed()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Equal("OtherResult", actual.Invoke("MethodWithDefaultValue","OtherResult"));
        }

        [Fact]
        public void GivenAModel_WhenSettingAUnknownOption_ThenTheCorrectExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            var actualException = Record.Exception(() => sut["UnknownOption"] == "test");

            // Assert
            Assert.IsType<UnknownOptionException>(actualException);
            var actual = actualException as UnknownOptionException;
            Assert.Equal("UnknownOption", actual.Option);
        }

        [Fact]
        public void GivenAModel_WhenGettingAUnknownOption_ThenTheCorrectExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            var dummy = "PrevValue";
            var actualException = Record.Exception(() => dummy = sut["UnknownOption"]);

            // Assert
            Assert.IsType<UnknownOptionException>(actualException);
            var actual = actualException as UnknownOptionException;
            Assert.Equal("UnknownOption", actual.Option);
        }

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
            Assert.Equal("UnknownMethod", actual.Command);
        }

        [Fact]
        public void GivenAOption_WhenOptionIsSet_ThenThePropertyChanges()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);
            actual["Option"] = "OptionValue";

            // Assert
            Assert.Equal("OptionValue", actual["Option"]);
        }

        [Fact]
        public void GivenAMethodWithTwoArguments_WhenOnlySettingOne_ThenTheCorrectExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            var actualException = Record.Exception(() => sut.Invoke("MethodWithTwoArguments","value1"));

            // Assert
            Assert.IsType<MissingArgumentException>(actualException);
            var actual = actualException as MissingArgumentException;
            Assert.Equal("value2", actual.Argument);
        }

        [Fact]
        public void GivenAMethodWithTwoArguments_WhenTryingToSet3_ThenTheCorrectExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var sut = ModelParser.Parse(model);
            var actualException = Record.Exception(() => sut.Invoke("MethodWithTwoArguments","value1", "value2", "value3"));

            // Assert
            Assert.IsType<TooManyArgumentsException>(actualException);
            var actual = actualException as TooManyArgumentsException;
            Assert.Equal(2, actual.Arguments.Length);
        }

        public class Mock
        {
            public string Option { get; set; }

            public string SimpleMethod()
            {
                return "SimpleMethodResult";
            }

            public string MethodWithDefaultValue(string value = "Result")
            {
                return value;
            }

            public string MethodWithTwoArguments(string value1, string value2)
            {
                return $"{value1} : {value2}";
            }
        }
    }
}