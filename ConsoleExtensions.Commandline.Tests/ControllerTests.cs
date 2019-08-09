// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ControllerTests.cs" company="Lasse Sjørup">
// //   Copyright (c)  Lasse Sjørup
// //   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests
{
    using ConsoleExtensions.Proxy.TestHelpers;

    using Xunit;

    /// <summary>
    ///     Class ControllerTests.
    /// </summary>
    public class ControllerTests
    {
        /// <summary>
        ///     Given a controller when calling a method then standard converters are used.
        /// </summary>
        [Fact]
        public void GivenAController_WhenCallingAMethod_ThenStandardConvertersAreUsed()
        {
            // Arrange
            var consoleProxy = new TestProxy();
            var setupCalled = false;
            var controller = new Controller(new MyClass(), consoleProxy, setup => setupCalled = true);
            controller.Run("Render", "true", "Some string", "12", "-Uppercase");

            // Act
            var actual = consoleProxy.ToString();

            // Assert
            Assert.Equal("TRUE : SOME STRING : 12", actual);
            Assert.True(setupCalled);
        }

        /// <summary>
        ///     Class MyClass.
        /// </summary>
        public class MyClass
        {
            /// <summary>
            ///     Gets or sets a value indicating whether this <see cref="MyClass" /> is uppercase.
            /// </summary>
            /// <value><c>true</c> if uppercase; otherwise, <c>false</c>.</value>
            public bool Uppercase { get; set; }

            /// <summary>
            ///     Testings the specified bool value.
            /// </summary>
            /// <param name="boolValue">if set to <c>true</c> [bool value].</param>
            /// <param name="stringValue">The string value.</param>
            /// <param name="intValue">The int value.</param>
            /// <returns>A string containing the values rendered as specified.</returns>
            public string Render(bool boolValue, string stringValue, int intValue)
            {
                if (this.Uppercase)
                {
                    return $"{boolValue} : {stringValue} : {intValue}".ToUpper();
                }

                return $"{boolValue} : {stringValue} : {intValue}";
            }
        }
    }
}