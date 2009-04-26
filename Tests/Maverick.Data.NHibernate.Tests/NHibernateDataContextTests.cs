// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="NHibernateDataContextTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the NHibernateDataContextTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate;
using TestUtilities;

namespace Maverick.Data.NHibernate.Tests {
    [TestClass]
    public class NHibernateDataContextTests {
        [TestMethod]
        public void Constructor_Requires_Non_Null_Session() {
            AutoTester.ArgumentNull<ISession>(marker => new NHibernateDataContext(marker));
        }

        [TestMethod]
        public void GetEntitySet_Returns_NHibernateEntitySet() {
            // Arrange
            ISession session = new Mock<ISession>().Object;
            NHibernateDataContext context = new NHibernateDataContext(session);

            // Act
            IEntitySet<Object> entitySet = context.GetEntitySet<Object>();

            // Assert
            Assert.IsInstanceOfType(entitySet, typeof(NHibernateEntitySet<Object>));
        }

        [TestMethod]
        public void GetEntitySet_Returns_NHibernateEntitySet_With_Session() {
            // Arrange
            ISession session = new Mock<ISession>().Object;
            NHibernateDataContext context = new NHibernateDataContext(session);

            // Act
            NHibernateEntitySet<Object> entitySet = context.GetEntitySet<Object>().AssertCast<NHibernateEntitySet<Object>>();

            // Assert
            Assert.AreSame(session, entitySet.Session);
        }

        [TestMethod]
        public void GetEntitySet_Returns_NHibernateEntitySet_With_NHibernateQueryable() {
            // Arrange
            ISession session = new Mock<ISession>().Object;
            NHibernateDataContext context = new NHibernateDataContext(session);

            // Act
            NHibernateEntitySet<Object> entitySet = context.GetEntitySet<Object>().AssertCast<NHibernateEntitySet<Object>>();

            // Assert
            Assert.IsNotNull(entitySet.Query);
        }

        [TestMethod]
        public void SaveChanges_Flushes_Session() {
            // Arrange
            var mockSession = new Mock<ISession>();
            NHibernateDataContext context = new NHibernateDataContext(mockSession.Object);

            // Act
            context.SaveChanges();

            // Assert
            mockSession.Verify(s => s.Flush());
        }

        [TestMethod]
        public void Dispose_Disposes_Session() {
            // Arrange
            var mockSession = new Mock<ISession>();
            NHibernateDataContext context = new NHibernateDataContext(mockSession.Object);

            // Act
            context.Dispose();

            // Assert
            mockSession.Verify(s => s.Dispose());
        }
    }
}
