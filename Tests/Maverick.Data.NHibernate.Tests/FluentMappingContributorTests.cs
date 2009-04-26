// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="FluentMappingContributorTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the FluentMappingContributorTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Mapping;
using TestUtilities;

namespace Maverick.Data.NHibernate.Tests {
    [TestClass]
    public class FluentMappingContributorTests {
        [CLSCompliant(false)]
        public class TestContributor : FluentMappingContributor {
            public class TestMap : ClassMap<Version> {
                public TestMap() {
                    WithTable("Foo");
                    Id(v => v.Major);
                }
            }

            public override void ContributeMappings(MappingConfiguration configuration) {
                configuration.FluentMappings.Add(typeof(TestMap));
            }
        }

        [TestMethod]
        public void ContributeMappings_Requires_Non_Null_Configuration() {
            AutoTester.ArgumentNull<Configuration>(marker => new TestContributor().ContributeMappings(marker));
        }

        [TestMethod]
        public void ContributeMappings_Applies_FluentMappings_To_NHibernate_Configuration() {
            // Arrange
            Configuration cfg = new Configuration().SetProperty("dialect", "NHibernate.Dialect.SQLiteDialect");
            TestContributor contributor = new TestContributor();

            // Act
            contributor.ContributeMappings(cfg);
            
            // Assert
            PersistentClass mapping = cfg.GetClassMapping(typeof(Version));
            Assert.IsNotNull(mapping);
            Assert.AreEqual("Foo", mapping.Table.Name);
            Assert.AreEqual("Major", mapping.IdentifierProperty.Name);
        }
    }
}
