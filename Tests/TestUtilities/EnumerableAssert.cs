// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="EnumerableAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the EnumerableAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public static class EnumerableAssert {
        public static void ElementsAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual) {
            ElementsMatch(expected, actual, (e, a) => Equals(e, a));
        }

        public static void ElementsAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message) {
            ElementsMatch(expected, actual, (e, a) => Equals(e, a), message);
        }

        public static void ElementsAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message, params object[] args) {
            ElementsMatch(expected, actual, (e, a) => Equals(e, a), message, args);
        }

        public static void ElementsMatch<TExpected, TActual>(IEnumerable<TExpected> expected, IEnumerable<TActual> actual, Func<TExpected, TActual, bool> matcher) {
            ElementsMatch(expected, actual, matcher, String.Empty, new string[0]);
        }

        public static void ElementsMatch<TExpected, TActual>(IEnumerable<TExpected> expected, IEnumerable<TActual> actual, Func<TExpected, TActual, bool> matcher, string message) {
            ElementsMatch(expected, actual, matcher, message, new string[0]);    
        }

        public static void ElementsMatch<TExpected, TActual>(IEnumerable<TExpected> expected, IEnumerable<TActual> actual, Func<TExpected, TActual, bool> matcher, string message, params object[] args) {
            IEnumerator<TExpected> expectedEnumerator = expected.GetEnumerator();
            IEnumerator<TActual> actualEnumerator = actual.GetEnumerator();

            while (actualEnumerator.MoveNext()) {
                Assert.IsTrue(expectedEnumerator.MoveNext());
                Assert.IsTrue(matcher(expectedEnumerator.Current, actualEnumerator.Current), message, args);
            }
        }
    }
}
