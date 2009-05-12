// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="EntityBaseTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the EntityBaseTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Maverick.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Maverick.Tests.Models {
    [TestClass]
    public class EntityBaseTests {
        [TestMethod]
        public void IsNew_Returns_True_If_Id_Null() {
            // Arrange
            var mockEntity = new Mock<EntityBase>() {CallBase = true};
            mockEntity.SetupGet(e => e.IdValue)
                      .Returns(() => null);

            // Act
            bool isNew = mockEntity.Object.IsNew;

            // Assert
            Assert.IsTrue(isNew);
        }

        [TestMethod]
        public void IsNew_Returns_False_If_Id_NotNull() {
            // Arrange
            var mockEntity = new Mock<EntityBase>() { CallBase = true };
            mockEntity.SetupGet(e => e.IdValue)
                      .Returns(42);

            // Act
            bool isNew = mockEntity.Object.IsNew;

            // Assert
            Assert.IsFalse(isNew);
        }
    }
}
