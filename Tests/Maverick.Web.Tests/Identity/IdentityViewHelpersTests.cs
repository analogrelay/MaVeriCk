// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentityViewHelpersTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentityViewHelpersTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maverick.Web.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;
using System.Web.Mvc;
using System.IO;

namespace Maverick.Web.Tests.Identity {
    [TestClass]
    public class IdentityViewHelpersTests {
        [TestMethod]
        public void BeginLoginForm_Requires_Non_Null_Helper() {
            AutoTester.ArgumentNull<HtmlHelper>(marker => marker.BeginLoginForm());
        }

        [TestMethod]
        public void BeginLoginForm_Returns_Form_With_Action_Referencing_Login_Action_Of_Identity_Controller_And_Current_IdentitySource() {
            // Arrange
            const string expected = "<form action=\"/Identity/Login/Id\" method=\"post\"></form>";
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            
            StringBuilder actual = new StringBuilder();
            Mock.Get(helper.ViewContext.HttpContext.Response)
                .Setup(r => r.Write(It.IsAny<string>()))
                .Callback<string>(s => actual.Append(s));

            // Act
            using(helper.BeginLoginForm()) {}

            // Assert
            Assert.AreEqual(expected, actual.ToString());
        }
    }
}
