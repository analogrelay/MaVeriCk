// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ValidationFailedExceptionTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ValidationFailedExceptionTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maverick.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TargetResources = Maverick.Properties.Resources;
using TestUtilities;

namespace Maverick.Tests.Validation {
    [TestClass]
    public class ValidationFailedExceptionTests {
        private const string TestObjectName = "Foo";

        [TestMethod]
        public void Constructor_Requires_NonNullOrEmpty_ObjectName() {
            AutoTester.StringArgumentNullOrEmpty(marker => new ValidationFailedException(marker));
        }

        [TestMethod]
        public void Constructor_With_ObjectName_Only_Initializes_Message_To_Default() {
            // Arrange 
            string expected = String.Format(TargetResources.Error_ValidationFailed, TestObjectName);
                
            // Act
            ValidationFailedException ex = new ValidationFailedException(TestObjectName);

            // Assert
            Assert.AreEqual(expected, ex.Message);
        }

        [TestMethod]
        public void Constructor_With_ObjectName_Only_Initializes_Errors_List_To_Empty_List() {
            // Act
            ValidationFailedException ex = new ValidationFailedException(TestObjectName);

            // Assert
            Assert.IsNotNull(ex.Errors);
            Assert.AreEqual(0, ex.Errors.Count);
        }

        [TestMethod]
        public void Constructor_With_ObjectName_Only_Initializes_ObjectName_Property() {
            // Act
            ValidationFailedException ex = new ValidationFailedException(TestObjectName);

            // Assert
            Assert.AreEqual(TestObjectName, ex.ObjectName);
        }

        [TestMethod]
        public void Constructor_With_ErrorList_Initializes_ObjectName() {
            // Act
            ValidationFailedException ex = new ValidationFailedException(TestObjectName, new List<ValidationError>());

            // Assert
            Assert.AreEqual(TestObjectName, ex.ObjectName);
        }

        [TestMethod]
        public void Constructor_With_ErrorList_Initializes_ErrorList() {
            // Arrange
            List<ValidationError> expected = new List<ValidationError>();

            // Act
            ValidationFailedException ex = new ValidationFailedException(TestObjectName, expected);

            // Assert
            Assert.AreSame(expected, ex.Errors);
        }

        [TestMethod]
        public void Constructor_With_ErrorList_Builds_Message_From_ErrorList() {
            // Arrange
            const string expected = @"There were errors validating the Foo:
* Foo Bar Baz
* Bar Foo Baz
* Baz Bar Foo
";

            List<ValidationError> errors = new List<ValidationError>() {
                new ValidationError("Foo", "Foo Bar Baz"),
                new ValidationError("Bar", "Bar Foo Baz"),
                new ValidationError("Baz", "Baz Bar Foo")
            };

            // Act
            ValidationFailedException ex = new ValidationFailedException(TestObjectName, errors);

            // Assert
            Assert.AreEqual(expected, ex.Message);
        }
    }
}
