// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DateTimeAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DateTimeAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public static class DateTimeAssert {
        public static void IsNear(DateTimeOffset expected, DateTimeOffset actual, long thresholdTicks) {
            IsNear(expected, actual, new TimeSpan(thresholdTicks));
        }

        public static void IsNear(DateTimeOffset expected, DateTimeOffset actual, TimeSpan threshold) {
            IsWithinThreshold(expected.Subtract(actual), threshold);
        }

        public static void IsNear(DateTime expected, DateTime actual, long thresholdTicks) {
            IsNear(expected, actual, new TimeSpan(thresholdTicks));
        }

        public static void IsNear(DateTime expected, DateTime actual, TimeSpan threshold) {
            IsWithinThreshold(expected.Subtract(actual), threshold);
        }

        public static void IsAfter(DateTimeOffset expected, DateTime actual) {
            TimeSpan difference = actual.Subtract(expected.DateTime);
            Assert.IsTrue(difference.Ticks > 0);
        }

        public static void IsAfter(DateTime expected, DateTimeOffset actual) {
            TimeSpan difference = actual.Subtract(expected);
            Assert.IsTrue(difference.Ticks > 0);
        }

        public static void IsAfter(DateTimeOffset expected, DateTimeOffset actual) {
            TimeSpan difference = actual.Subtract(expected);
            Assert.IsTrue(difference.Ticks > 0);
        }

        public static void IsAfter(DateTime expected, DateTime actual) {
            TimeSpan difference = actual.Subtract(expected);
            Assert.IsTrue(difference.Ticks > 0);
        }

        private static void IsWithinThreshold(TimeSpan difference, TimeSpan threshold) {
            Assert.IsFalse(Math.Abs(difference.Ticks) > Math.Abs(threshold.Ticks));
        }
    }
}
