// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentitySourceAttributeTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentitySourceAttributeTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maverick.Tests.ComponentModel;
using Maverick.Web.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Web.Tests.Identity {
    [TestClass]
    public class IdentitySourceAttributeTests : ComponentMetadataAttributeTestsBase<IdentitySourceAttribute> {
        protected override Func<IdentitySourceAttribute>[] GetConstructors() {
            return new Func<IdentitySourceAttribute>[] {
                () => new IdentitySourceAttribute(TestName),
                () => new IdentitySourceAttribute(TestName, TestVersion),
                () => new IdentitySourceAttribute(TestName, TestVersion, TestDescription),
                () => new IdentitySourceAttribute(TestName, TestVersion, TestDescription, TestVendor),
                () => new IdentitySourceAttribute(TestName, TestVersion, TestDescription, TestVendor, TestLogoUrl)
            };
        }

        [TestMethod]
        public void Constructor_Requires_Non_NullOrEmpty_Name() {
            AutoTester.StringArgumentNullOrEmpty(m => new IdentitySourceAttribute(m));
            AutoTester.StringArgumentNullOrEmpty(m => new IdentitySourceAttribute(m, TestVersion));
            AutoTester.StringArgumentNullOrEmpty(m => new IdentitySourceAttribute(m, TestVersion, TestDescription));
            AutoTester.StringArgumentNullOrEmpty(m => new IdentitySourceAttribute(m, TestVersion, TestDescription, TestVendor));
            AutoTester.StringArgumentNullOrEmpty(m => new IdentitySourceAttribute(m, TestVersion, TestDescription, TestVendor, TestLogoUrl));
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
