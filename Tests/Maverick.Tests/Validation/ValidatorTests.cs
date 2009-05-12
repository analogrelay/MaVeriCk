// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ValidatorTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ValidatorTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Maverick.Validation;
using TestUtilities;

namespace Maverick.Tests.Validation {
    [TestClass]
    public class ValidatorTests {
        [TestMethod]
        public void ValidateAndThrowIfInvalid_Throws_ValidationFailed_If_Validate_Returns_Non_Empty_List() {
            // Arrange
            object target = new object();
            var mockValidator = new Mock<Validator>() {CallBase = true};
            IEnumerable<ValidationError> expectedErrors = new[] {new ValidationError("Foo", "Bar")};
            mockValidator.Setup(v => v.Validate(target))
                         .Returns(expectedErrors);

            // Act
            ValidationFailedException thrown = ExceptionAssert.Capture(() => mockValidator.Object
                                                              .ValidateAndThrowIfInvalid(target))
                                                              .AssertCast<ValidationFailedException>();

            // Assert
            Assert.IsNotNull(thrown);
            EnumerableAssert.ElementsAreEqual(thrown.Errors, expectedErrors);
        }
    }
}
