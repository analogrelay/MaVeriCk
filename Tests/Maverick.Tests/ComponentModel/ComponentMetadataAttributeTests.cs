// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ComponentMetadataAttributeTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ComponentMetadataAttributeTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Maverick.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Tests.ComponentModel {
    // Wish there was a way to just re-run the inherited tests without copy-pasting them...
    [TestClass]
    public class ComponentMetadataAttributeTests : ComponentMetadataAttributeTestsBase<TestAttribute> {
        protected internal override Func<TestAttribute>[] GetConstructors() {
            return new Func<TestAttribute>[] {
                () => new TestAttribute(TestName),
                () => new TestAttribute(TestName, TestVersion),
                () => new TestAttribute(TestName, TestVersion, TestDescription),
                () => new TestAttribute(TestName, TestVersion, TestDescription, TestVendor),
                () => new TestAttribute(TestName, TestVersion, TestDescription, TestVendor, TestLogoUrl)
            };
        }

        [TestMethod]
        public void Constructor_Requires_NonNullOrEmpty_Name() {
            AutoTester.StringArgumentNullOrEmpty(m => new TestAttribute(m));
            AutoTester.StringArgumentNullOrEmpty(m => new TestAttribute(m, TestVersion));
            AutoTester.StringArgumentNullOrEmpty(m => new TestAttribute(m, TestVersion, TestDescription));
            AutoTester.StringArgumentNullOrEmpty(m => new TestAttribute(m, TestVersion, TestDescription, TestVendor));
            AutoTester.StringArgumentNullOrEmpty(m => new TestAttribute(m, TestVersion, TestDescription, TestVendor, TestLogoUrl));
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

    public class TestAttribute : ComponentMetadataAttribute {
        public TestAttribute(string name) : base(name) { }
        public TestAttribute(string name, string version) : base(name, version) { }
        public TestAttribute(string name, string version, string description) : base(name, version, description) { }
        public TestAttribute(string name, string version, string description, string vendor) : base(name, version, description, vendor) { }
        public TestAttribute(string name, string version, string description, string vendor, string logoUrl) : base(name, version, description, vendor, logoUrl) { }
    }
}
