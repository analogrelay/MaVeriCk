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
using Maverick.Models;
using Maverick.Web.Helpers;
using Maverick.Web.ModuleFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using Moq;

namespace Maverick.Web.Tests.Helpers {
    [TestClass]
    public class ModuleHtmlHelpersTests {
        private const string TestId = "Foo";
        private const int TestModuleId = 42;
        private const string TestPrefixedId = "m42_Foo";

        [TestMethod]
        public void Id_Requires_Non_Null_Helper() {
            AutoTester.ArgumentNull<HtmlHelper>(marker => marker.Id(TestId));
        }

        [TestMethod]
        public void Id_Requires_Non_NullOrEmpty_BaseId() {
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            AutoTester.StringArgumentNullOrEmpty(marker => helper.Id(marker));
        }

        [TestMethod]
        public void Id_Returns_BaseId_If_No_Active_Module() {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();

            // Act
            string actual = helper.Id(TestId);

            // Assert
            Assert.AreEqual(TestId, actual);
        }

        [TestMethod]
        public void Id_Prefixes_BaseId_With_ModuleId_If_ActiveModule_Exists() {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            helper.ViewContext.HttpContext.GetPortalContext().ActiveModuleRequest = new ModuleRequestResult() {
                Module = new Module() {
                    Id = TestModuleId
                },
                Application = new Mock<ModuleApplication>().Object
            };

            // Act
            string actual = helper.Id(TestId);

            // Assert
            Assert.AreEqual(TestPrefixedId, actual);
        }

        [TestMethod]
        public void Id_Returns_BaseId_If_ActiveModuleRequest_Null() {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            helper.ViewContext.HttpContext.GetPortalContext().ActiveModuleRequest = null;

            // Act
            string actual = helper.Id(TestId);

            // Assert
            Assert.AreEqual(TestId, actual);
        }

        [TestMethod]
        public void Id_Returns_BaseId_If_ActiveModuleRequest_Module_Null() {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            helper.ViewContext.HttpContext.GetPortalContext().ActiveModuleRequest = new ModuleRequestResult() {
                Module = null,
                Application = new Mock<ModuleApplication>().Object
            };

            // Act
            string actual = helper.Id(TestId);

            // Assert
            Assert.AreEqual(TestId, actual);
        }
    }
}
