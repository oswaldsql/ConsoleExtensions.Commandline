// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddHelpExtensionTests.cs" company="Lasse Sjørup">
//   Copyright (c) 2019 Lasse Sjørup
//   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests
{
    using System.ComponentModel;
    using System.Linq;

    using ConsoleExtensions.Commandline.Help;

    using Xunit;

    /// <summary>
    ///     Class AddHelpExtensionTests.
    /// </summary>
    public class AddHelpExtensionTests
    {
        /// <summary>
        ///     Given a model when a help method exists then add help dos not
        ///     add help.
        /// </summary>
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

        /// <summary>
        ///     Given a model when calling help with command topic then help
        ///     should contain model metadata.
        /// </summary>
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

        /// <summary>
        ///     Given a model when calling help with command topic then help
        ///     should contain the right usage information.
        /// </summary>
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
                                         DisplayName = null,
                                         Description = "Some description",
                                         Optional = true,
                                         DefaultValue = "default"
                                     };

            Assert.Equal(secondExpected, second, new DetailsComparer());
        }

        /// <summary>
        ///     Given a model when calling help with option topic then help for
        ///     that option should be returned.
        /// </summary>
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

        /// <summary>
        ///     Given a model when calling help with option topic then help
        ///     should contain model metadata.
        /// </summary>
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

        /// <summary>
        ///     Given a model when calling help with option topic then help
        ///     should contain the right usage information.
        /// </summary>
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

        /// <summary>
        ///     Given a model when calling help without topic then help for full
        ///     model should be returned.
        /// </summary>
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

        /// <summary>
        ///     Class ClassWithExistingHelp.
        /// </summary>
        public class ClassWithExistingHelp
        {
            /// <summary>
            ///     Helps the specified topic.
            /// </summary>
            /// <param name="topic">The topic.</param>
            /// <returns>
            ///     The custom help message.
            /// </returns>
            public string Help(string topic)
            {
                return "Custom help";
            }
        }

        /// <summary>
        ///     Class Mock.
        /// </summary>
        [DisplayName("MockModel")]
        [Description("Describe Mock Model")]
        public class Mock
        {
            /// <summary>
            ///     Gets or sets the option.
            /// </summary>
            /// <value>
            ///     The option.
            /// </value>
            [DisplayName("First Option")]
            [Description("The first option.")]
            public string Option { get; set; }

            /// <summary>
            ///     Commands the specified argument.
            /// </summary>
            /// <param name="argument">The argument.</param>
            /// <param name="optional">The optional.</param>
            /// <returns>
            ///     A mock text string.
            /// </returns>
            public string Command(
                string argument,
                [Description("Some description")]
                string optional = "default")
            {
                return argument;
            }
        }
    }
}