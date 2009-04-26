// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextManagerAttributeTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextManagerAttributeTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;

using Maverick.Tests.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Data.Tests {
    // Wish there was a way to just re-run the inherited tests without copy-pasting them...
    [TestClass]
    public class DataContextManagerAttributeTests : ComponentMetadataAttributeTestsBase<DataContextManagerAttribute> {
        protected override Func<DataContextManagerAttribute>[] GetConstructors() {
            return new Func<DataContextManagerAttribute>[] {
                () => new DataContextManagerAttribute(TestName),
                () => new DataContextManagerAttribute(TestName, TestVersion),
                () => new DataContextManagerAttribute(TestName, TestVersion, TestDescription),
                () => new DataContextManagerAttribute(TestName, TestVersion, TestDescription, TestVendor),
                () => new DataContextManagerAttribute(TestName, TestVersion, TestDescription, TestVendor, TestLogoUrl)
            };
        }

        [TestMethod]
        public void Constructor_Requires_Non_NullOrEmpty_Name() {
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextManagerAttribute(m));
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextManagerAttribute(m, TestVersion));
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextManagerAttribute(m, TestVersion, TestDescription));
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextManagerAttribute(m, TestVersion, TestDescription, TestVendor));
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextManagerAttribute(m, TestVersion, TestDescription, TestVendor, TestLogoUrl));
        }

        [TestMethod]
        public override void Constructor_Sets_Description_Property() {
            base.Constructor_Sets_Description_Property();
        }

        [TestMethod]
        public override void Constructor_Sets_LogoUrl_Property() {
            base.Constructor_Sets_LogoUrl_Property();
        }

        [TestMethod]
        public override void Constructor_Sets_Name_Property() {
            base.Constructor_Sets_Name_Property();
        }

        [TestMethod]
        public override void Constructor_Sets_Vendor_Property() {
            base.Constructor_Sets_Vendor_Property();
        }

        [TestMethod]
        public override void Constructor_Sets_Version_Property() {
            base.Constructor_Sets_Version_Property();
        }
    }
}
