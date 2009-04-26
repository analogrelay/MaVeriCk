// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextFactoryAttributeTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextFactoryAttributeTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;

using Maverick.Tests.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Data.Tests {
    // Wish there was a way to just re-run the inherited tests without copy-pasting them...
    [TestClass]
    public class DataContextFactoryAttributeTests : ComponentMetadataAttributeTestsBase<DataContextFactoryAttribute> {
        protected override Func<DataContextFactoryAttribute>[] GetConstructors() {
            return new Func<DataContextFactoryAttribute>[] {
                () => new DataContextFactoryAttribute(TestName),
                () => new DataContextFactoryAttribute(TestName, TestVersion),
                () => new DataContextFactoryAttribute(TestName, TestVersion, TestDescription),
                () => new DataContextFactoryAttribute(TestName, TestVersion, TestDescription, TestVendor),
                () => new DataContextFactoryAttribute(TestName, TestVersion, TestDescription, TestVendor, TestLogoUrl)
            };
        }

        [TestMethod]
        public void Constructor_Requires_Non_NullOrEmpty_Name() {
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextFactoryAttribute(m));
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextFactoryAttribute(m, TestVersion));
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextFactoryAttribute(m, TestVersion, TestDescription));
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextFactoryAttribute(m, TestVersion, TestDescription, TestVendor));
            AutoTester.StringArgumentNullOrEmpty(m => new DataContextFactoryAttribute(m, TestVersion, TestDescription, TestVendor, TestLogoUrl));
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
