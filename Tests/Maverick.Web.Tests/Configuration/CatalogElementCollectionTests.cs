// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CatalogElementCollectionTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CatalogElementCollectionTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Maverick.Web.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using System.Configuration;

namespace Maverick.Web.Tests.Configuration {
    [TestClass]
    public class CatalogElementCollectionTests : ConfigurationElementCollectionTestsBase<CatalogElement> {
        private const string TestName = "Foo";

        public class CatalogElementCollectionAccessor : CatalogElementCollection, IConfigurationElementCollectionAccessor {
            public ConfigurationElement AccessCreateNewElement() {
                return CreateNewElement();
            }

            public object AccessGetElementKey(ConfigurationElement element) {
                return GetElementKey(element);
            }
        }

        [TestMethod]
        public void CreateNewElement_Returns_New_CatalogElement() {
            RunCreateNewElementTest();
        }

        [TestMethod]
        public void GetElementKey_Returns_CatalogElement_Name() {
            RunGetElementTest(new CatalogElement() {
                Name = TestName
            }, TestName);
        }

        protected override IConfigurationElementCollectionAccessor CreateCollectionAccessor() {
            return new CatalogElementCollectionAccessor();
        }
    }
}
