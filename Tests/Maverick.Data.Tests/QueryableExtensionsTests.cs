// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="QueryableExtensionsTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the QueryableExtensionsTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Maverick.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Data.Tests {
    [TestClass]
    public class QueryableExtensionsTests {
        [TestMethod]
        public void AsEntityQuery_Requires_Non_Null_Queryable() {
            AutoTester.ArgumentNull<IQueryable<object>>(marker => marker.AsEntityQuery());
        }

        [TestMethod]
        public void AsEntityQuery_Requires_Non_Null_Enumerable() {
            AutoTester.ArgumentNull<IEnumerable<object>>(marker => marker.AsEntityQuery());
        }

        [TestMethod]
        public void AsEntityQuery_Wraps_Queryable_In_EntityQueryAdapter() {
            // Arrange
            IQueryable<object> expected = new Mock<IQueryable<object>>().Object;

            // Act
            EntityQueryAdapter<object> adapter = expected.AsEntityQuery() as EntityQueryAdapter<object>;

            // Assert
            Assert.IsNotNull(adapter);
            Assert.AreSame(expected, adapter.InnerQuery);
        }

        [TestMethod]
        public void AsEntityQuery_Wraps_Enumerable_In_EntityQueryAdapter() {
            // Arrange
            IEnumerator<object> expected = new Mock<IEnumerator<object>>().Object;
            var mockEnumerable = new Mock<IEnumerable<object>>();
            mockEnumerable.Setup(e => e.GetEnumerator()).Returns(expected);
            
            // Act
            EntityQueryAdapter<object> adapter = mockEnumerable.Object.AsEntityQuery() as EntityQueryAdapter<object>;

            // Assert
            Assert.IsNotNull(adapter);
            Assert.AreSame(expected, adapter.InnerQuery.GetEnumerator());
        }
    }
}
