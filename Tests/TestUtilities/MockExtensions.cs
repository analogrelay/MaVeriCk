// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MockExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MockExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestUtilities {
    [CLSCompliant(false)]
    public static class MockExtensions {
        public static void Never<T, TResult>(this Mock<T> mock, Expression<Func<T, TResult>> call) where T : class {
            Never(mock, call, "Expected that the specified invocation would not be called");
        }

        public static void Never<T, TResult>(this Mock<T> mock, Expression<Func<T, TResult>> call, string message) where T : class {
            mock.Setup(call).Callback(() => Assert.Fail(message));
        }

        public static void Never<T>(this Mock<T> mock, Expression<Action<T>> call) where T : class {
            Never(mock, call, "Expected that the specified invocation would not be called");
        }

        public static void Never<T>(this Mock<T> mock, Expression<Action<T>> call, string message) where T : class {
            mock.Setup(call).Callback(() => Assert.Fail(message));
        }

        public static void Verify<T>(this Mock<T> mock, Expression<Action<T>> call, string message) where T : class {
            try {
                mock.Verify(call);
            } catch(MockException) {
                Assert.Fail(message);
            }
        }
    }
}
