// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="StackHelper.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the StackHelper type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public static class StackHelper {
        public static StackFrame GetCaller(int count) {
            // Increase the count, since it is supposed to be relative to the previous method, and this method has
            // added a frame to the stack
            return new StackTrace(false).GetFrame(count + 1);
        }

        public static StackFrame FindTestMethodCaller() {
            return FindCaller(frame => frame.GetMethod().HasAttribute<TestMethodAttribute>());
        }

        public static StackFrame FindTestCallerInTestClass() {
            return FindCaller(frame => frame.GetMethod().HasAttribute<TestMethodAttribute>() ||
                                       frame.GetMethod().HasAttribute<TestInitializeAttribute>() ||
                                       frame.GetMethod().HasAttribute<TestCleanupAttribute>() ||
                                       frame.GetMethod().DeclaringType.HasAttribute<TestClassAttribute>());
        }

        public static StackFrame FindCaller(Predicate<StackFrame> criteria) {
            StackFrame[] frames = new StackTrace(false).GetFrames();
            StackFrame testMethodFrame = null;
            for (int i = 0; testMethodFrame == null && i < frames.Length; i++) {
                if (criteria(frames[i])) {
                    testMethodFrame = frames[i];
                }
            }
            return testMethodFrame;
        }

        private static bool HasAttribute<TAttr>(this ICustomAttributeProvider attributeProvider) where TAttr : Attribute {
            return attributeProvider.GetCustomAttributes(typeof(TAttr), true).Length > 0;
        }
    }
}