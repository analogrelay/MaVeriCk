// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="AppSettingsCatalogTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the AppSettingsCatalogTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Linq;
using Maverick.Web.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Web.Tests.Configuration {
    [TestClass]
    public class AppSettingsCatalogTests {
        [TestMethod]
        public void Parts_Should_Contain_One_Part_Per_AppSetting() {
            // Arrange
            AppSettingsCatalog catalog = new AppSettingsCatalog();

            // Act
            int partCount = catalog.Parts.Count();

            // Assert
            Assert.AreEqual(ConfigurationManager.AppSettings.Count, partCount);
        }

        [TestMethod]
        public void AppSettingPart_Should_Export_KeyName_As_Contract() {
            // Arrange
            AppSettingsCatalog catalog = new AppSettingsCatalog();

            // Act
            IQueryable<ExportDefinition> exports = catalog.Parts.Select(p => p.ExportDefinitions.Single());


            // Assert
            EnumerableAssert.ElementsMatch(exports,
                                           ConfigurationManager.AppSettings.Keys.Cast<string>(),
                                           (export, setting) => String.Equals(export.ContractName,
                                                                              setting,
                                                                              StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void AppSettingPart_Should_Return_SettingValue_On_Create() {
            // Arrange
            AppSettingsCatalog catalog = new AppSettingsCatalog();

            // Act
            IQueryable<string> parts = catalog.Parts.Select(p => p.CreatePart()
                                                                     .GetExportedObject(p.ExportDefinitions.Single()))
                .Cast<string>();


            // Assert
            EnumerableAssert.ElementsMatch(parts,
                                           ConfigurationManager.AppSettings.Keys.Cast<string>(),
                                           (part, setting) => String.Equals(part,
                                                                            ConfigurationManager.AppSettings[setting],
                                                                            StringComparison.OrdinalIgnoreCase));
        }
    }
}