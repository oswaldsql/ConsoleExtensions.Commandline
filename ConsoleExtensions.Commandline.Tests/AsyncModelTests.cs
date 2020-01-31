// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="AsyncModelTests.cs" company="Lasse Sjørup">
// //   Copyright (c)  Lasse Sjørup
// //   Licensed under the MIT license. See LICENSE file in the solution root for full license information.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace ConsoleExtensions.Commandline.Tests
{
    using System;
    using System.Threading.Tasks;

    using ConsoleExtensions.Commandline.Parser;

    using JetBrains.Annotations;

    using Xunit;

    /// <summary>
    /// Tests for models with async methods.
    /// </summary>
    public class AsyncModelTests
    {
        /// <summary>
        /// Given a method with asynchronous suffix
        /// When return value is task
        /// Then method name dos not contain asynchronous.
        /// </summary>
        [Fact]
        public void GivenAMethodWithAsyncSuffix_WhenReturnValueIsTask_ThenMethodNameDosNotContainAsync()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Contains(actual.Commands, pair => pair.Key == "VoidTask");
            var command = actual.Commands["VoidTask"];
            Assert.Equal("VoidTaskAsync", command.Name);
            Assert.Equal("VoidTaskAsync", command.Method.Name);
        }

        /// <summary>
        /// Given a method with asynchronous suffix
        /// When return value is task of string
        /// Then method name dos not contain asynchronous.
        /// </summary>
        [Fact]
        public void GivenAMethodWithAsyncSuffix_WhenReturnValueIsTaskOfString_ThenMethodNameDosNotContainAsync()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Contains(actual.Commands, pair => pair.Key == "StringTask");
            var command = actual.Commands["StringTask"];
            Assert.Equal("StringTaskAsync", command.Name);
            Assert.Equal("StringTaskAsync", command.Method.Name);
        }

        /// <summary>
        /// Given a asynchronous void method
        /// When executing
        /// Then the task is awaited and null is returned.
        /// </summary>
        [Fact]
        public void GivenAAsyncVoidMethod_WhenExecuting_ThenTheTaskIsAwaitedAndNullIsReturned()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.False(model.VoidTaskHasBeenCalled);
            Assert.Null(actual.Invoke("VoidTask"));
            Assert.True(model.VoidTaskHasBeenCalled);
        }

        /// <summary>
        /// Given a asynchronous method with result
        /// When executing
        /// Then the task is awaited and the result is returned.
        /// </summary>
        [Fact]
        public void GivenAAsyncMethodWithResult_WhenExecuting_ThenTheTaskIsAwaitedAndTheResultIsReturned()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            Assert.Contains(actual.Commands, pair => pair.Key == "StringTask");
            Assert.Equal("Some string", actual.Invoke("StringTask"));
        }

        /// <summary>
        /// Given a asynchronous method
        /// When method fails
        /// Then the exception should be thrown.
        /// </summary>
        [Fact]
        public void GivenAAsyncMethod_WhenMethodFails_ThenTheExceptionShouldBeThrown()
        {
            // Arrange
            var model = new Mock();

            // Act
            var actual = ModelParser.Parse(model);

            // Assert
            var exception = Record.Exception(() => actual.Invoke("FailingTask"));
            var expected = exception as ArgumentException;
            Assert.NotNull(expected);
            Assert.Equal("Some argument was wrong", expected.Message);
        }

        /// <summary>
        /// Mock class for testing Async calling.
        /// </summary>
        internal class Mock
        {
            /// <summary>
            /// Signals that the void task has been called.
            /// </summary>
            internal bool VoidTaskHasBeenCalled { get; private set; }

            /// <summary>
            /// After a small delay VoidTaskHasBeenCalled is set to true.
            /// </summary>
            [UsedImplicitly]
            public async Task VoidTaskAsync()
            {
                await Task.Delay(100).ConfigureAwait(false);

                this.VoidTaskHasBeenCalled = true;
            }

            /// <summary>
            /// After a small delay a string is returned.
            /// </summary>
            /// <returns></returns>
            public async Task<string> StringTaskAsync()
            {
                await Task.Delay(100).ConfigureAwait(false);

                return await Task.FromResult("Some string").ConfigureAwait(false);
            }

            /// <summary>
            /// After a small delay a argument exception is thrown.
            /// </summary>
            /// <returns></returns>
            /// <exception cref="ArgumentException">Some argument was wrong</exception>
            public async Task<int> FailingTaskAsync()
            {
                await Task.Delay(100).ConfigureAwait(false);

                throw new ArgumentException("Some argument was wrong");
            }
        }

    }
}