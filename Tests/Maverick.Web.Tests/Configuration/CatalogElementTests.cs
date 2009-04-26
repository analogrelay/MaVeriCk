// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CatalogElementTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CatalogElementTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maverick.Web.Configuration;

namespace Maverick.Web.Tests.Configuration {
    [TestClass]
    public class CatalogElementTests : ConfigurationElementTestsBase<CatalogElementTests.CatalogElementAccessor> {
        private const string TestName = "Foo";

        public class CatalogElementAccessor : CatalogElement, IConfigurationElementAccessor {
            public object GetProperty(string name) {
                return base[name];
            }

            public void SetProperty(string name, object value) {
                base[name] = value;
            }
        }

        [TestMethod]
        public void Name_Getter_Retrieves_Value_From_Properties_Dictionary() {
            RunPropertySetterTest("name", TestName, e => e.Name);
        }

        [TestMethod]
        public void Name_Setter_Stores_Value_In_Properties_Dictionary() {
            RunPropertyGetterTest("name", TestName, e => e.Name);
        }

        protected override CatalogElementAccessor CreateAccessor() {
            return new CatalogElementAccessor();
        }

    }
}
