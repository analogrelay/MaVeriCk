// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ImportExtensionsTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ImportExtensionsTests type.
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
    public class ImportExtensionsTests {
        [TestMethod]
        public void Stylesheet_Requires_NonNull_Helper() {
            AutoTester.ArgumentNull<HtmlHelper>(marker => marker.Stylesheet("Foo"));
        }

        [TestMethod]
        public void Stylesheet_Requires_NonNullOrEmpty_ContentPath() {
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            AutoTester.StringArgumentNullOrEmpty(marker => helper.Stylesheet(marker));
        }

        [TestMethod]
        public void Script_Requires_NonNull_Helper() {
            AutoTester.ArgumentNull<HtmlHelper>(marker => marker.Script("Foo"));
        }

        [TestMethod]
        public void Script_Requires_NonNullOrEmpty_ContentPath() {
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            AutoTester.StringArgumentNullOrEmpty(marker => helper.Script(marker));
        }

        [TestMethod]
        public void Stylesheet_Returns_Link_Tag_Referencing_Resolved_Content_Path() {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();

            // Act
            string actual = helper.Stylesheet("~/Content/Styles.css");

            // Assert
            Assert.AreEqual("<link href=\"/Content/Styles.css\" type=\"text/css\" rel=\"stylesheet\"></link>", actual);
        }

        [TestMethod]
        public void Script_Returns_Script_Tag_Referencing_Resolved_Content_Path() {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();

            // Act
            string actual = helper.Script("~/Scripts/jquery.js");

            // Assert
            Assert.AreEqual("<script src=\"/Scripts/jquery.js\" type=\"text/javascript\"></script>", actual);
        }
    }
}
