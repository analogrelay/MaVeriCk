// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ObjectExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ObjectExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public static class ObjectExtensions {
        public static T AssertCast<T>(this object value) where T : class {
            Assert.IsInstanceOfType(value, typeof(T));
            return value as T;
        }
    }
}
