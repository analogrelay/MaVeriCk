// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="EntityQueryAdapterTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the EntityQueryAdapterTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Maverick.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Data.Tests {
    [TestClass]
    public class EntityQueryAdapterTests {
        [TestMethod]
        public void Constructor_Requires_Non_Null_Query() {
            AutoTester.ArgumentNull<IQueryable<object>>(marker => new EntityQueryAdapter<object>(marker));
        }

        [TestMethod]
        public void Include_Requires_Non_Null_Or_Empty_Property() {
            EntityQueryAdapter<object> adapter = CreateEntityQueryAdapter();
            AutoTester.StringArgumentNullOrEmpty(marker => adapter.Include(marker));
        }

        [TestMethod]
        public void Include_Requires_Non_Null_Selector() {
            EntityQueryAdapter<object> adapter = CreateEntityQueryAdapter();
            AutoTester.ArgumentNull<Expression<Func<object, object>>>(marker => adapter.Include(marker));
        }

        [TestMethod]
        public void Include_With_Property_Name_Returns_Adapter() {
            EntityQueryAdapter<object> adapter = CreateEntityQueryAdapter();
            Assert.AreSame(adapter, adapter.Include("Foo"));
        }

        [TestMethod]
        public void Include_With_Selector_Returns_Adapter() {
            EntityQueryAdapter<object> adapter = CreateEntityQueryAdapter();
            Assert.AreSame(adapter, adapter.Include(o => o));
        }

        [TestMethod]
        public void GetEnumerator_Calls_Adapted_Query() {
            RunAdapteeCallTest(q => q.GetEnumerator(), new Mock<IEnumerator<object>>().Object);
        }

        [TestMethod]
        public void NonGeneric_GetEnumerator_Calls_Adapted_Query() {
            RunAdapteeCallTest(q => q.GetEnumerator(),
                               a => ((IEnumerable)a).GetEnumerator(),
                               new Mock<IEnumerator<object>>().Object);
        }

        [TestMethod]
        public void ElementType_Returns_ElementType_Of_Adapted_Query() {
            RunAdapteeCallTest(q => q.ElementType, typeof(string));
        }

        [TestMethod]
        public void Expression_Returns_Expression_Of_Adapted_Query() {
            RunAdapteeCallTest(q => q.Expression, Expression.Constant(42));
        }

        [TestMethod]
        public void Provider_Returns_Provider_Of_Adapted_Query() {
            RunAdapteeCallTest(q => q.Provider, new Mock<IQueryProvider>().Object);
        }

        [TestMethod]
        public void InnerQuery_Contains_Adapted_Query() {
            // Arrange
            var expected = new Mock<IQueryable<object>>().Object;
            EntityQueryAdapter<object> adapter = new EntityQueryAdapter<object>(expected);

            // Act
            IQueryable<object> actual = adapter.InnerQuery;

            // Assert
            Assert.AreSame(expected, actual);
        }

        private static void RunAdapteeCallTest<TOutput>(Expression<Func<IQueryable<object>, TOutput>> call, TOutput expected) {
            RunAdapteeCallTest(call, a => call.Compile()(a), expected);
        }

        private static void RunAdapteeCallTest<TOutput>(Expression<Func<IQueryable<object>, TOutput>> adapteeCall, Func<EntityQueryAdapter<object>, TOutput> call, TOutput expected) {
            RunAdapteeCallTest(adapteeCall, expected, call, expected);
        }

        private static void RunAdapteeCallTest<TAdapteeOutput, TOutput>(Expression<Func<IQueryable<object>, TAdapteeOutput>> adapteeCall,
                                                                        TAdapteeOutput adapteeOutput,
                                                                        Func<EntityQueryAdapter<object>, TOutput> call,
                                                                        TOutput expected) {
            // Arrange
            var mockQuery = new Mock<IQueryable<object>>();
            mockQuery.Setup(adapteeCall).Returns(adapteeOutput);
            EntityQueryAdapter<object> adapter = new EntityQueryAdapter<object>(mockQuery.Object);

            // Act
            TOutput actual = call(adapter);

            // Assert
            Assert.AreSame(expected, actual);
        }

        private EntityQueryAdapter<object> CreateEntityQueryAdapter() {
            var mockQuery = new Mock<IQueryable<object>>();
            return new EntityQueryAdapter<object>(mockQuery.Object);
        }
    }
}
