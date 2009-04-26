// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentityHtmlHelpersTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentityHtmlHelpersTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc.Html;
using Maverick.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web;
using Moq;
using Maverick.Web.Models;
using Microsoft.IdentityModel.Claims;

namespace Maverick.Web.Tests.Helpers {
    [TestClass]
    public class IdentityHtmlHelpersTests {
        private const string LoginLinkFormat = "<a href=\"/{1}/{2}{4}?returnUrl={0}\">{3}</a>";
        private const string LogoutLinkFormat = "<a href=\"/{1}/{2}{4}\">{3}</a>";

        [TestMethod]
        public void LoginLink_With_No_Arguments_Uses_Defaults() {
            RunLinkTest(h => h.LoginLink(), GetExpectedLoginLink);
        }

        [TestMethod]
        public void LoginLink_Title_Parameter_Overrides_Default_Link_Title() {
            RunLinkTest(h => h.LoginLink("Foo"), h => GetExpectedLoginLink(h, "Foo"));
        }

        [TestMethod]
        public void LoginLink_IdentitySource_Parameter_Appends_IdentitySource_To_Url() {
            RunLinkTest(h => h.LoginLink("Foo", "Bar"), h => GetExpectedLoginLink(h, "Foo", "Bar"));
        }

        [TestMethod]
        public void LogoutLink_With_No_Arguments_Uses_Defaults() {
            RunLinkTest(h => h.LogoutLink(), GetExpectedLogoutLink);
        }

        [TestMethod]
        public void LogoutLink_Title_Parameter_Overrides_Default_Link_Title() {
            RunLinkTest(h => h.LogoutLink("Foo"), h => GetExpectedLogoutLink(h, "Foo"));
        }

        [TestMethod]
        public void LogoutLink_IdentitySource_Parameter_Appends_IdentitySource_To_Url() {
            RunLinkTest(h => h.LogoutLink("Foo", "Bar"), h => GetExpectedLogoutLink(h, "Foo", "Bar"));
        }

        [TestMethod]
        public void CurrentUser_Returns_Null_If_Request_Not_Authenticated() {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            Mock.Get(helper.ViewContext.HttpContext.Request)
                .SetupGet(r => r.IsAuthenticated)
                .Returns(false);

            // Act
            UserIdentity user = helper.CurrentUser();

            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void CurrentUser_Returns_Null_If_Current_User_Not_IClaimsPrincipal() {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            Mock.Get(helper.ViewContext.HttpContext.Request)
                .SetupGet(r => r.IsAuthenticated)
                .Returns(true);
            Mock.Get(helper.ViewContext.HttpContext)
                .SetupGet(c => c.User)
                .Returns(new Mock<IPrincipal>().Object);

            // Act
            UserIdentity user = helper.CurrentUser();

            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void CurrentUser_Returns_UserIdentity_If_Current_User_Is_IClaimsPrincipal() {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            Mock.Get(helper.ViewContext.HttpContext.Request)
                .SetupGet(r => r.IsAuthenticated)
                .Returns(true);

            IClaimsPrincipal expected = new Mock<IClaimsPrincipal>().Object;
            Mock.Get(helper.ViewContext.HttpContext)
                .SetupGet(c => c.User)
                .Returns(expected);

            // Act
            UserIdentity user = helper.CurrentUser();

            // Assert
            Assert.IsNotNull(user);
            Assert.AreSame(expected, user.Principal);
        }

        private static void RunLinkTest(Func<HtmlHelper, string> getActual, Func<HtmlHelper, string> getExpected) {
            // Arrange
            HtmlHelper helper = Mockery.CreateHtmlHelper();
            
            // Act
            string actual = getActual(helper);

            // Assert (using IsTrue because we want case-insensitive matching)
            Assert.IsTrue(String.Equals(getExpected(helper), actual, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetExpectedLoginLink(HtmlHelper h) {
            return GetExpectedLoginLink(h, IdentityHtmlHelpers.DefaultLoginLinkTitle, String.Empty);
        }

        private static string GetExpectedLoginLink(HtmlHelper h, string title) {
            return GetExpectedLoginLink(h, title, String.Empty);
        }

        private static string GetExpectedLoginLink(HtmlHelper h, string title, string identitySource) {
            string idSourceFormat = String.IsNullOrEmpty(identitySource) ? String.Empty : "/" + identitySource;
            return String.Format(LoginLinkFormat,
                                 GetReturnUrl(h),
                                 IdentityHtmlHelpers.DefaultLoginController,
                                 IdentityHtmlHelpers.DefaultLoginAction,
                                 title,
                                 idSourceFormat);
        }

        private static string GetExpectedLogoutLink(HtmlHelper h) {
            return GetExpectedLogoutLink(h, IdentityHtmlHelpers.DefaultLogoutLinkTitle, String.Empty);
        }

        private static string GetExpectedLogoutLink(HtmlHelper h, string title) {
            return GetExpectedLogoutLink(h, title, String.Empty);
        }

        private static string GetExpectedLogoutLink(HtmlHelper h, string title, string identitySource) {
            string idSourceFormat = String.IsNullOrEmpty(identitySource) ? String.Empty : "/" + identitySource;
            return String.Format(LogoutLinkFormat,
                                 GetReturnUrl(h),
                                 IdentityHtmlHelpers.DefaultLogoutController,
                                 IdentityHtmlHelpers.DefaultLogoutAction,
                                 title,
                                 idSourceFormat);
        }

        private static string GetReturnUrl(HtmlHelper helper) {
            return HttpUtility.UrlEncode(helper.ViewContext.HttpContext.Request.Url.ToString());
        }
    }
}
