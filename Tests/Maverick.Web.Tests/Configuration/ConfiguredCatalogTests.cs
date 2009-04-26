// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ConfiguredCatalogTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ConfiguredCatalogTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using Maverick.Models;
using Maverick.Web.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Web.Tests.Configuration {
    [TestClass]
    public class ConfiguredCatalogTests {
        private static readonly Type[] SpecifiedExportedTypes = new [] {typeof(TestExport1), typeof(TestExport2)};
        private static readonly Type[] DefaultExportedTypes = new[] { typeof(TestExport3), typeof(TestExport2) };
        private static readonly Assembly DefaultAssembly = typeof(MaverickApplication).Assembly;
        private static readonly Assembly SpecifiedAssembly = typeof(Portal).Assembly;
        private const string DefaultDirectory = "Baz\\Bar\\Foo\\";
        private const string SpecifiedDirectory = "Foo\\Bar\\Baz";

        public class TestCatalog : ComposablePartCatalog {
            public override IQueryable<ComposablePartDefinition> Parts {
                get { return Enumerable.Empty<ComposablePartDefinition>().AsQueryable(); }
            }
        }

        [Export]
        public class TestExport1 {}

        [Export]
        public class TestExport2 { }

        [Export]
        public class TestExport3 { }

        [TestMethod]
        public void Constructor_Requires_Non_NullOrEmpty_SectionName() {
            AutoTester.StringArgumentNullOrEmpty(marker => new ConfiguredCatalog(marker));
        }

        [TestMethod]
        public void Parts_Returns_Empty_Catalog_If_No_ConfigurationSection_Loaded() {
            Assert.AreEqual(0, new ConfiguredCatalog((ComponentsSection)null).Parts.Count());
        }

        [TestMethod]
        public void Constructor_With_No_Args_Loads_From_Default_Configuration_Section() {
            // Arrange
            RunLoadConfigurationTest(new ConfiguredCatalog(), 
                                     DefaultDirectory, 
                                     DefaultAssembly, 
                                     DefaultExportedTypes);
        }

        [TestMethod]
        public void Constructor_With_Section_Name_Loads_From_Specified_Configuration_Section() {
            // Arrange
            RunLoadConfigurationTest(new ConfiguredCatalog("components"),
                                     SpecifiedDirectory,
                                     SpecifiedAssembly,
                                     SpecifiedExportedTypes);
        }

        private static void RunLoadConfigurationTest(ConfiguredCatalog catalog, string directoryCatalogPath, Assembly assemblyCatalogAssembly, IEnumerable<Type> typeCatalogTypes) {
            EnsureDirectory(directoryCatalogPath);
            //BuildManagerTypeNameConverter.TypeResolver = name => Type.GetType(name, false, true);

            // Assert
            Assert.AreEqual(1, catalog.Catalogs.OfType<TestCatalog>()
                                   .Count());
            Assert.AreEqual(1, catalog.Catalogs.OfType<DirectoryCatalog>()
                                   .Where(c => c.Path == directoryCatalogPath)
                                   .Count());
            Assert.AreEqual(1, catalog.Catalogs.OfType<AssemblyCatalog>()
                                   .Where(c => c.Assembly == assemblyCatalogAssembly)
                                   .Count());
            EnumerableAssert.ElementsMatch(typeCatalogTypes,
                                           catalog.Catalogs.OfType<TypeCatalog>()
                                               .Single().Parts,
                                           (e, a) => e == a.CreatePart()
                                                              .GetExportedObject(a.ExportDefinitions.Single())
                                                              .GetType());
        }

        private static void EnsureDirectory(string directoryPath) {
            if(!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}