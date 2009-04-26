// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleHtmlHelpersTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleHtmlHelpersTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Maverick.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Web.Tests.Helpers {
    [TestClass]
    public class ModuleHtmlHelpersTests {
        [TestMethod]
        public void Id_Requires_Non_Null_Helper() {
            AutoTester.ArgumentNull<HtmlHelper>(marker => marker.Id("Foo"));
        }

        [TestMethod]
        public void Id_Requires_Non_NullOrEmpty_BaseId() {
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            AutoTester.StringArgumentNullOrEmpty(marker => helper.Id(marker));
        }
    }
}
