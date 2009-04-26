// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="NHibernateEntitySetTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the NHibernateEntitySetTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using Maverick.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate;
using NHibernate.Linq;
using TestUtilities;
using TargetResources = Maverick.Data.NHibernate.Properties.Resources;

namespace Maverick.Data.NHibernate.Tests {
    [TestClass]
    public class NHibernateEntitySetTests {
        [TestMethod]
        public void Constructor_Requires_Non_Null_Session() {
            AutoTester.ArgumentNull<ISession>(
                marker => new NHibernateEntitySet<Portal>(marker, new Mock<INHibernateQueryable<Portal>>().Object));
        }

        [TestMethod]
        public void Constructor_Requires_Non_Null_Query() {
            AutoTester.ArgumentNull<INHibernateQueryable<Portal>>(
                marker => new NHibernateEntitySet<Portal>(new Mock<ISession>().Object, marker));
        }

        [TestMethod]
        public void Include_Returns_This() {
            // Arrange
            var mockQuery = new Mock<INHibernateQueryable<Portal>>();
            NHibernateEntitySet<Portal> entitySet = new NHibernateEntitySet<Portal>(new Mock<ISession>().Object,
                                                                                    mockQuery.Object);

            // Act
            IEntityQuery<Portal> returned = entitySet.Include("Foo");

            // Assert
            Assert.AreSame(entitySet, returned);
        }

        [TestMethod]
        public void Include_Calls_Expand_On_Query() {
            // Arrange
            var mockQuery = new Mock<INHibernateQueryable<Portal>>();
            NHibernateEntitySet<Portal> entitySet = new NHibernateEntitySet<Portal>(new Mock<ISession>().Object,
                                                                                    mockQuery.Object);

            // Act
            entitySet.Include("Foo");

            // Assert
            mockQuery.Verify(q => q.Expand("Foo"));
        }

        [TestMethod]
        public void Include_Determines_Property_Name_From_Expression() {
            // Arrange
            var mockQuery = new Mock<INHibernateQueryable<Portal>>();
            NHibernateEntitySet<Portal> entitySet = new NHibernateEntitySet<Portal>(new Mock<ISession>().Object,
                                                                                    mockQuery.Object);

            // Act
            entitySet.Include(p => p.Pages);

            // Assert
            mockQuery.Verify(q => q.Expand("Pages"));
        }

        [TestMethod]
        public void Include_Guards_Against_Non_MemberAccessExpression() {
            // Arrange
            var mockQuery = new Mock<INHibernateQueryable<Portal>>();
            NHibernateEntitySet<Portal> entitySet = new NHibernateEntitySet<Portal>(new Mock<ISession>().Object,
                                                                                    mockQuery.Object);

            // Act and Assert
            ExceptionAssert.Guards(() => entitySet.Include(p => new Portal()),
                                   TargetResources.Error_ExpressionWasNotMemberAccess);
        }

        [TestMethod]
        public void GetEnumeratorOfT_Calls_GetEnumerator_On_Query() {
            TestQueryDelegatedCall(e => ((IEnumerable<Portal>)e).GetEnumerator(), q => q.GetEnumerator());
        }

        [TestMethod]
        public void GetEnumerator_Calls_GetEnumerator_On_Query() {
            TestQueryDelegatedCall(e => ((IEnumerable)e).GetEnumerator(), q => q.GetEnumerator());
        }

        [TestMethod]
        public void Expression_Returns_Expression_From_Query() {
            TestQueryDelegatedCall(e => e.Expression, q => q.Expression);
        }

        [TestMethod]
        public void ElementType_Returns_ElementType_From_Query() {
            TestQueryDelegatedCall(e => e.ElementType, q => q.ElementType);
        }

        [TestMethod]
        public void Provider_Returns_Provider_From_Query() {
            TestQueryDelegatedCall(e => e.Provider, q => q.Provider);
        }

        [TestMethod]
        public void InsertOnSave_Calls_Save_On_Session() {
            Portal p = new Portal();
            TestSessionDelegatedCall(e => e.InsertOnSave(p), s => s.Save(p));
        }

        [TestMethod]
        public void DeleteOnSave_Calls_Delete_On_Session() {
            Portal p = new Portal();
            TestSessionDelegatedCall(e => e.DeleteOnSave(p), s => s.Delete(p));
        }

        [TestMethod]
        public void UpdateOnSave_With_Modified_Calls_Update_On_Session() {
            Portal p = new Portal();
            TestSessionDelegatedCall(e => e.UpdateOnSave(p), s => s.Update(p));
        }

        [TestMethod]
        public void UpdateOnSave_With_Original_And_Modified_Calls_Update_On_Session_With_Modified() {
            Portal p = new Portal();
            TestSessionDelegatedCall(e => e.UpdateOnSave(new Portal(), p), s => s.Update(p));
        }

        [TestMethod]
        public void Attach_Calls_Lock_On_Session_With_LockMode_None() {
            Portal p = new Portal();
            TestSessionDelegatedCall(e => e.Attach(p), s => s.Lock(p, LockMode.None));
        }

        [TestMethod]
        public void Detach_Calls_Evict_On_Session() {
            Portal p = new Portal();
            TestSessionDelegatedCall(e => e.Detach(p), s => s.Evict(p));
        }

        [TestMethod]
        public void Query_Returns_NHibernateQueryable_Provided_In_Constructor() {
            // Arrange
            INHibernateQueryable<Portal> expected = new Mock<INHibernateQueryable<Portal>>().Object;
            NHibernateEntitySet<Portal> entitySet = new NHibernateEntitySet<Portal>(new Mock<ISession>().Object,
                                                                                    expected);

            // Act
            INHibernateQueryable<Portal> actual = entitySet.Query;

            // Assert
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void Session_Returns_Session_Provided_In_Constructor() {
            // Arrange
            ISession expected = new Mock<ISession>().Object;
            NHibernateEntitySet<Portal> entitySet = new NHibernateEntitySet<Portal>(expected,
                                                                                    new Mock<INHibernateQueryable<Portal>>().Object);

            // Act
            ISession actual = entitySet.Session;

            // Assert
            Assert.AreSame(expected, actual);
        }

        private static void TestQueryDelegatedCall<T>(Func<NHibernateEntitySet<Portal>, T> entitySetCall, Expression<Func<INHibernateQueryable<Portal>, T>> queryCall) {
            // Arrange
            var mockQuery = new Mock<INHibernateQueryable<Portal>>();
            NHibernateEntitySet<Portal> entitySet = new NHibernateEntitySet<Portal>(new Mock<ISession>().Object,
                                                                                    mockQuery.Object);

            // Act
            T expr = entitySetCall(entitySet);

            // Assert
            mockQuery.Verify(queryCall);
        }

        private static void TestSessionDelegatedCall(Action<NHibernateEntitySet<Portal>> entitySetCall, Expression<Action<ISession>> sessionCall) {
            // Arrange
            var mockSession = new Mock<ISession>();
            NHibernateEntitySet<Portal> entitySet = new NHibernateEntitySet<Portal>(mockSession.Object,
                                                                                    new Mock<INHibernateQueryable<Portal>>().Object);

            // Act
            entitySetCall(entitySet);

            // Assert
            mockSession.Verify(sessionCall);
        }
    }
}
