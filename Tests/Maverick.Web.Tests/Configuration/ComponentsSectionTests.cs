// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ComponentsSectionTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ComponentsSectionTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using Maverick.Web.Configuration;
using System.Web.Configuration;

namespace Maverick.Web.Tests.Configuration {
    [TestClass]
    public class ComponentsSectionTests : ConfigurationElementTestsBase<ComponentsSectionTests.ComponentsSectionAccessor> {
        public class ComponentsSectionAccessor : ComponentsSection, IConfigurationElementAccessor {
            // Mixins would be soooo handy here :)

            public object GetProperty(string name) {
                return base[name];
            }

            public void SetProperty(string name, object value) {
                base[name] = value;
            }
        }

        [TestMethod]
        public void Catalogs_Getter_Retrieves_Value_From_Properties_Dictionary() {
            RunPropertyGetterTest("catalogs", new CatalogElementCollection(), c => c.Catalogs);
        }

        [TestMethod]
        public void Catalogs_Setter_Retrieves_Value_From_Properties_Dictionary() {
            RunPropertySetterTest("catalogs", new CatalogElementCollection(), c => c.Catalogs);
        }

        [TestMethod]
        public void Directories_Getter_Retrieves_Value_From_Properties_Dictionary() {
            RunPropertyGetterTest("directories", new DirectoryElementCollection(), c => c.Directories);
        }

        [TestMethod]
        public void Directories_Setter_Retrieves_Value_From_Properties_Dictionary() {
            RunPropertySetterTest("directories", new DirectoryElementCollection(), c => c.Directories);
        }

        [TestMethod]
        public void Assemblies_Getter_Retrieves_Value_From_Properties_Dictionary() {
            RunPropertyGetterTest("assemblies", new AssemblyCollection(), c => c.Assemblies);
        }

        [TestMethod]
        public void Assemblies_Setter_Retrieves_Value_From_Properties_Dictionary() {
            RunPropertySetterTest("assemblies", new AssemblyCollection(), c => c.Assemblies);
        }

        [TestMethod]
        public void Types_Getter_Retrieves_Value_From_Properties_Dictionary() {
            RunPropertyGetterTest("types", new TypeElementCollection(), c => c.Types);
        }

        [TestMethod]
        public void Types_Setter_Retrieves_Value_From_Properties_Dictionary() {
            RunPropertySetterTest("types", new TypeElementCollection(), c => c.Types);
        }

        protected override ComponentsSectionAccessor CreateAccessor() {
            return new ComponentsSectionAccessor();
        }
    }
}
