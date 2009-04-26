// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ConnectionStringsCatalogTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ConnectionStringsCatalogTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using Maverick.Web.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using System.Data.SqlClient;

namespace Maverick.Web.Tests.Configuration {
    [TestClass]
    public class ConnectionStringsCatalogTests {
        private const string TestName = "Foo";
        private const string TestConnectionString = "Server=(local);Database=Foo;Integrated Security=True";
        private const string TestProviderName = "System.Data.SqlClient";

        [TestMethod]
        public void Parts_Should_Contain_One_Part_Per_ConnectionString() {
            // Arrange
            ConnectionStringsCatalog catalog = new ConnectionStringsCatalog();

            // Act
            int partCount = catalog.Parts.Count();

            // Assert
            Assert.AreEqual(ConfigurationManager.ConnectionStrings.Count, partCount);
        }

        [TestMethod]
        public void ConnectionStringPart_Should_Export_ConnectionStringName_As_Contract() {
            // Arrange
            ConnectionStringsCatalog catalog = new ConnectionStringsCatalog();

            // Act
            IQueryable<ExportDefinition> exports = catalog.Parts.Select(p => p.ExportDefinitions.Single());
            IEnumerable<ConnectionStringSettings> connectionStrings = ConfigurationManager.ConnectionStrings
                .Cast<ConnectionStringSettings>();


            // Assert
            EnumerableAssert.ElementsMatch(exports,
                                           connectionStrings,
                                           (export, setting) => String.Equals(export.ContractName,
                                                                              setting.Name,
                                                                              StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ConnectionStringPart_Should_Return_DbConnectionFactory_On_Create() {
            // Arrange
            ConnectionStringsCatalog catalog = new ConnectionStringsCatalog();

            // Act
            IQueryable<Func<DbConnection>> parts = catalog.Parts.Select(p => p.CreatePart()
                                                                                 .GetExportedObject(p.ExportDefinitions.Single()))
                .Select(p => p as Func<DbConnection>);
            IEnumerable<ConnectionStringSettings> connectionStrings = ConfigurationManager.ConnectionStrings
                .Cast<ConnectionStringSettings>();


            // Assert
            EnumerableAssert.ElementsMatch(parts,
                                           connectionStrings,
                                           (part, setting) => part != null);
        }

        [TestMethod]
        public void OpenConnection_Uses_Provider_And_ConnectionString_To_Create_Connection() {
            // Arrange
            ConnectionStringSettings settings = new ConnectionStringSettings(TestName, TestConnectionString, TestProviderName);

            // Act
            Func<DbConnection> connectionSource =
                ConnectionStringsCatalog.ConnectionStringComposablePartDefinition.CreateConnection(settings);
            DbConnection connection = connectionSource();

            // Assert
            Assert.IsInstanceOfType(connection, typeof(SqlConnection));
            Assert.AreEqual(TestConnectionString, connection.ConnectionString);
        }
    }
}