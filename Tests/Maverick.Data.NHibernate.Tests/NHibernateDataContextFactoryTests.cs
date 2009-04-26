// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="NHibernateDataContextFactoryTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the NHibernateDataContextFactoryTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using TestUtilities;
using TargetResources = Maverick.Data.NHibernate.Properties.Resources;

namespace Maverick.Data.NHibernate.Tests {
    [TestClass]
    public class NHibernateDataContextFactoryTests {
        private const string NHibernateConfigurationFilePath = @"TestFiles\NHibernate\NHibernate.config";

        [TestMethod]
        public void CreateDataContext_Guards_Against_Null_ConnectionSource() {
            // Arrange
            NHibernateDataContextFactory factory = new NHibernateDataContextFactory();

            // Act and Assert
            ExceptionAssert.Guards(() => factory.CreateDataContext(),
                                   TargetResources.Error_NoConfigurationSource,
                                   NHibernateContractNames.ConfigurationFilePath,
                                   NHibernateContractNames.ConfigurationSource);
        }

        [TestMethod]
        public void CreateDataContext_Guards_Against_Null_ConnectionSource_Result() {
            // Arrange
            NHibernateDataContextFactory factory = new NHibernateDataContextFactory {
                ConfigurationSource = () => null,
            };

            // Act and Assert
            ExceptionAssert.Guards(() => factory.CreateDataContext(),
                                   TargetResources.Error_ConfigurationSourceReturnedNull);
        }

        [TestMethod]
        public void CreateDataContext_Does_Not_Fail_If_MappingContributors_Is_Null() {
            // Arrange
            NHibernateDataContextFactory factory = CreateFactory(new Configuration(), null);
            SetupMockSessionFactory(factory);

            // Act and Assert (by not throwing)
            DataContext context = factory.CreateDataContext();
        }

        [TestMethod]
        public void CreateDataContext_Calls_Each_MappingContributor_With_Configuration() {
            // Arrange
            Mock<MappingContributor> mockContributor1 = new Mock<MappingContributor>();
            Mock<MappingContributor> mockContributor2 = new Mock<MappingContributor>();
            Configuration cfg = new Configuration();

            NHibernateDataContextFactory factory = CreateFactory(cfg,
                                                                 new List<MappingContributor> {
                                                                     mockContributor1.Object,
                                                                     mockContributor2.Object
                                                                 });
            SetupMockSessionFactory(factory);

            
            
            // Act
            DataContext context = factory.CreateDataContext();

            // Assert
            mockContributor1.Verify(c => c.ContributeMappings(cfg));
            mockContributor2.Verify(c => c.ContributeMappings(cfg));
        }

        [TestMethod]
        public void CreateDataContext_Returns_NHibernateDataContext() {
            // Arrange
            ISessionFactory expected = SetupMockSessionFactory();
            NHibernateDataContextFactory factory = CreateFactory();
            SetupMockSessionFactory(factory, expected);

            // Act
            DataContext context = factory.CreateDataContext();

            // Assert
            Assert.IsInstanceOfType(context, typeof(NHibernateDataContext));
        }

        [TestMethod]
        public void CreateSessionFactory_Correctly_Configures_NHibernate_SessionFactory() {
            // Arrange
            NHibernateDataContextFactory factory = CreateFactory(null);
            factory.ConfigurationFilePath = NHibernateConfigurationFilePath;

            // Act
            ISessionFactory context = factory.CreateSessionFactory();
            
            // Assert
            Assert.IsInstanceOfType(context.Dialect, typeof(SQLiteDialect));
            Assert.IsInstanceOfType(context.ConnectionProvider, typeof(DriverConnectionProvider));
        }

        [TestMethod]
        public void ConfigurationSource_Uses_Configuration_File_Path_If_Provided() {
            // Arrange
            Configuration cfg = null;
            NHibernateDataContextFactory factory = CreateFactory(null);
            factory.ConfigurationFilePath = NHibernateConfigurationFilePath;
            Mock.Get(factory)
                .Setup(f => f.CreateSessionFactory(It.IsAny<Configuration>()))
                .Callback<Configuration>(c => cfg = c)
                .Returns(SetupMockSessionFactory());

            // Act
            factory.CreateDataContext();

            // Assert
            Assert.AreEqual("NHibernate.Connection.DriverConnectionProvider", cfg.Properties["connection.provider"]);
            Assert.AreEqual("NHibernate.Driver.SQLite20Driver", cfg.Properties["connection.driver_class"]);
            Assert.AreEqual("TestConnectionSource", cfg.Properties["connection.connection_string_name"]);
            Assert.AreEqual("NHibernate.Dialect.SQLiteDialect", cfg.Properties["dialect"]);
        }

        private static void SetupMockSessionFactory(NHibernateDataContextFactory factory) {
            SetupMockSessionFactory(factory, SetupMockSessionFactory());
        }

        private static ISessionFactory SetupMockSessionFactory() {
            var mockFactory = new Mock<ISessionFactory>();
            mockFactory.Setup(f => f.OpenSession()).Returns(new Mock<ISession>().Object);
            return mockFactory.Object;
        }

        private static void SetupMockSessionFactory(NHibernateDataContextFactory factory, ISessionFactory sessionFactory) {
            Mock.Get(factory)
                .Setup(f => f.CreateSessionFactory(It.IsAny<Configuration>()))
                .Returns(() => sessionFactory);
        }

        private static NHibernateDataContextFactory CreateFactory() {
            return CreateFactory(new Configuration(), new List<MappingContributor>());
        }

        private static NHibernateDataContextFactory CreateFactory(Func<Configuration> configurationSource) {
            return CreateFactory(configurationSource, new List<MappingContributor>());
        }

        private static NHibernateDataContextFactory CreateFactory(Configuration configuration, IList<MappingContributor> contributors) {
            return CreateFactory(() => configuration, contributors);
        }

        private static NHibernateDataContextFactory CreateFactory(Func<Configuration> configurationSource, IList<MappingContributor> contributors) {
            NHibernateDataContextFactory factory = new Mock<NHibernateDataContextFactory> {CallBase = true}.Object;
            factory.ConfigurationSource = configurationSource;
            factory.MappingContributors = contributors;

            return factory;
        }
    }
}
