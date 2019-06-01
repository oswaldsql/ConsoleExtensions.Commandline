// ReSharper disable ExceptionNotDocumented
// ReSharper disable StyleCop.SA1600

namespace ConsoleExtensions.Commandline.Tests
{
    using System.ComponentModel;
    using System.Linq;

    using ConsoleExtensions.Commandline.Help;

    using Xunit;

    public class AddHelpExtensionTests
    {
        [Fact]
        public void GivenAModel_WhenAHelpMethodExists_ThenAddHelpDosNotAddHelp()
        {
            // Arrange
            var controller = new Controller(new ClassWithExistingHelp(), c => c.AddHelp());

            // Act
            var actual = controller.ModelMap.Invoke("Help", "Topic");

            // Assert
            Assert.IsType<string>(actual);
            Assert.Equal("Custom help", actual);
        }

        [Fact]
        public void GivenAModel_WhenCallingHelpWithCommandTopic_ThenHelpShouldContainModelMetadata()
        {
            // Arrange
            var controller = new Controller(new Mock(), c => c.AddHelp());

            // Act
            var actual = controller.ModelMap.Invoke("Help", "Command") as HelpDetails;

            // Assert
            Assert.NotNull(actual?.Usage);

            Assert.Equal("MockModel", actual.ModelName);
            Assert.Equal("1.0.0.0", actual.ModelVersion.ToString());
            Assert.Equal("Describe Mock Model", actual.Description);
            Assert.Null(actual.Commands);
            Assert.Contains(actual.Options, option => option.Name == "Option");
        }

        [Fact]
        public void GivenAModel_WhenCallingHelpWithCommandTopic_ThenHelpShouldContainTheRightUsageInformation()
        {
            // Arrange
            var controller = new Controller(new Mock(), c => c.AddHelp());

            // Act
            var actual = controller.ModelMap.Invoke("Help", "Command") as HelpDetails;

            // Assert
            var usage = actual?.Usage;
            Assert.NotNull(usage);
            Assert.Equal("Command", usage.Name);
            Assert.Equal("String", usage.ReturnType);
            Assert.Equal("Command", usage.DisplayName);
            Assert.Equal(2, usage.Arguments.Length);

            var first = usage.Arguments.First();
            var firstExpected = new ArgumentDetails { Name = "argument", Type = "String" };
            Assert.Equal(firstExpected, first, new DetailsComparer());

            var second = usage.Arguments[1];
            var secondExpected = new ArgumentDetails
                                     {
                                         Name = "optional",
                                         Type = "String",
                                         DisplayName = "Optional value",
                                         Description = "Some description",
                                         Optional = true,
                                         DefaultValue = "default"
                                     };
            Assert.Equal(secondExpected, second, new DetailsComparer());
        }

        [Fact]
        public void GivenAModel_WhenCallingHelpWithOptionTopic_ThenHelpForThatOptionShouldBeReturned()
        {
            // Arrange
            var controller = new Controller(new Mock(), c => c.AddHelp());

            // Act
            var actual = controller.ModelMap.Invoke("Help", "Option") as HelpDetails;

            // Assert
            Assert.NotNull(actual?.Usage);
            Assert.Equal("MockModel", actual.ModelName);
            Assert.Equal("1.0.0.0", actual.ModelVersion.ToString());
            Assert.Equal("Describe Mock Model", actual.Description);
            Assert.Null(actual.Commands);
            Assert.Contains(actual.Options, option => option.Name == "Option");
        }

        [Fact]
        public void GivenAModel_WhenCallingHelpWithOptionTopic_ThenHelpShouldContainModelMetadata()
        {
            // Arrange
            var controller = new Controller(new Mock(), c => c.AddHelp());

            // Act
            var actual = controller.ModelMap.Invoke("Help", "Option") as HelpDetails;

            // Assert
            Assert.NotNull(actual?.Usage);
            Assert.Equal("MockModel", actual.ModelName);
            Assert.Equal("1.0.0.0", actual.ModelVersion.ToString());
            Assert.Equal("Describe Mock Model", actual.Description);
            Assert.Null(actual.Commands);
            Assert.Contains(actual.Options, option => option.Name == "Option");
        }

        [Fact]
        public void GivenAModel_WhenCallingHelpWithOptionTopic_ThenHelpShouldContainTheRightUsageInformation()
        {
            // Arrange
            var controller = new Controller(new Mock(), c => c.AddHelp());

            // Act
            var actual = controller.ModelMap.Invoke("Help", "Option") as HelpDetails;

            // Assert
            var usage = actual?.Usage;
            Assert.NotNull(usage);
            Assert.Equal("Option", usage.Name);
            Assert.Equal("String", usage.ReturnType);
            Assert.Equal("First Option", usage.DisplayName);
            Assert.Equal("The first option.", usage.Description);
            Assert.Null(usage.Arguments);
        }

        [Fact]
        public void GivenAModel_WhenCallingHelpWithoutTopic_ThenHelpForFullModelShouldBeReturned()
        {
            // Arrange
            var controller = new Controller(new Mock(), c => c.AddHelp());

            // Act
            var actual = controller.ModelMap.Invoke("Help") as HelpDetails;

            // Assert
            Assert.NotNull(actual);
            Assert.Null(actual.Usage);
            Assert.Equal("MockModel", actual.ModelName);
            Assert.Equal("1.0.0.0", actual.ModelVersion.ToString());
            Assert.Equal("Describe Mock Model", actual.Description);
            Assert.Contains(actual.Commands, command => command.Name == "Help");
            Assert.Contains(actual.Commands, command => command.Name == "Command");
            Assert.Contains(actual.Options, option => option.Name == "Option");
        }

        public class ClassWithExistingHelp
        {
            public string Help(string topic)
            {
                return "Custom help";
            }
        }

        [DisplayName("MockModel")]
        [Description("Describe Mock Model")]
        public class Mock
        {
            [DisplayName("First Option")]
            [Description("The first option.")]
            public string Option { get; set; }

            public string Command(
                string argument,
                [DisplayName("Optional value")] [Description("Some description")]
                string optional = "default")
            {
                return argument;
            }
        }
    }
}