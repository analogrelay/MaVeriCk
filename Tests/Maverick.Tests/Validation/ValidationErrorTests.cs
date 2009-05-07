// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ValidationErrorTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ValidationErrorTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maverick.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Tests.Validation {
    [TestClass]
    public class ValidationErrorTests {
        [TestMethod]
        public void ValidationError_Constructor_Requires_NonNullOrEmpty_PropertyName() {
            AutoTester.StringArgumentNullOrEmpty(marker => new ValidationError(marker, "Foo"));
        }

        [TestMethod]
        public void ValidationError_Constructor_Requires_NonNullOrEmtpy_ErrorMessage() {
            AutoTester.StringArgumentNullOrEmpty(marker => new ValidationError("Bar", marker));
        }

        [TestMethod]
        public void ValidationError_Constructor_Requires_NonNull_TargetObject_If_Provided() {
            AutoTester.ArgumentNull<object>(marker => new ValidationError("Bar", "Foo", marker));
        }

        [TestMethod]
        public void ValidationError_Constructor_With_Name_And_Message_Sets_Properties() {
            // Act
            ValidationError error = new ValidationError("Bar", "Foo");

            // Assert
            Assert.AreEqual("Bar", error.PropertyName);
            Assert.AreEqual("Foo", error.ErrorMessage);
            Assert.IsNull(error.TargetObject);
        }

        [TestMethod]
        public void ValidationError_Constructor_With_Name_Message_And_Target_Sets_Properties() {
            // Act
            object expected = new object();
            ValidationError error = new ValidationError("Bar", "Foo", expected);

            // Assert
            Assert.AreEqual("Bar", error.PropertyName);
            Assert.AreEqual("Foo", error.ErrorMessage);
            Assert.AreSame(expected, error.TargetObject);
        }
    }
}