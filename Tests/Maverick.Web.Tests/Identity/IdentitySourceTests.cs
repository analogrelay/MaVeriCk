// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentitySourceTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentitySourceTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.ComponentModel;
using Maverick.Models;
using Maverick.Web.Helpers;
using Maverick.Web.Identity;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Web.Tests.Identity {
    [TestClass]
    public class IdentitySourceTests {
        private const string TestLandingUrlBase = "http://localhost";

        [TestMethod]
        public void IdentitySource_Is_ContractType_With_MetadataViewType() {
            // Assert
            CompositionAssert.IsContractType(typeof(IdentitySource), typeof(ComponentMetadata));
        }

        [TestMethod]
        public void If_Not_Overriden_Routes_Returns_RouteTable_Routes() {
            // Arrange
            IdentitySource source = CreateIdentitySource();

            // Assert
            Assert.AreSame(RouteTable.Routes, source.Routes);
        }

        [TestMethod]
        public void SessionIdentityManager_Is_Imported_From_CompositionContainer() {
            // Assert
            CompositionAssert.IsImported<IdentitySource>(id => id.SessionIdentityManager);
        }

        [TestMethod]
        public void GetReturnUrl_Returns_returnUrl_QueryString_Parameter_Value() {
            // Arrange
            ControllerContext context = Mockery.CreateMockControllerContext();
            context.HttpContext.Request.QueryString["returnUrl"] = "http://www.foobar.example";
            IdentitySource source = CreateIdentitySource();

            // Act
            Uri returnUrl = source.GetReturnUrl(context);

            // Assert
            Assert.AreEqual("http://www.foobar.example/", returnUrl.ToString());
        }

        [TestMethod]
        public void Logout_Calls_ClearSessionPrincipal_On_SessionIdentityManager() {
            // Arrange
            var mockSessionIdentityManager = new Mock<ISessionIdentityManager>();
            IdentitySource source = CreateIdentitySource();
            source.SessionIdentityManager = mockSessionIdentityManager.Object;

            // Act
            source.Logout(new Mock<ControllerContext>().Object);

            // Assert
            mockSessionIdentityManager.Verify(s => s.ClearSessionPrincipal());
        }

        [TestMethod]
        public void OnLoginFormSubmit_Returns_EmptyResult() {
            // Arrange
            IdentitySource source = CreateIdentitySource();

            // Act
            ActionResult result = source.OnLoginFormSubmit(new Mock<ControllerContext>().Object, new Uri("http://foo.example/"));

            // Assert
            ResultAssert.IsEmpty(result);
        }

        [TestMethod]
        public void SetSessionPrincipal_Calls_Same_Method_On_SessionIdentityManager() {
            // Arrange
            var mockSessionIdentityManager = new Mock<ISessionIdentityManager>();
            IdentitySource source = CreateIdentitySource();
            source.SessionIdentityManager = mockSessionIdentityManager.Object;
            IClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();

            // Act
            source.SetSessionPrincipal(claimsPrincipal);

            // Assert
            mockSessionIdentityManager.Verify(
                s => s.SetSessionPrincipal(It.Is<SessionSecurityToken>(token => token.ClaimsPrincipal == claimsPrincipal)));
        }

        [TestMethod]
        public void GenerateLandingUrl_Generates_Correct_Landing_Url() {
            // Arrange, Act and Assert
            SetupGenerateLandingUrlTest(TestLandingUrlBase, (s, c) => s.GenerateLandingUrl(c));
        }

        [TestMethod]
        public void GenerateLandingUrl_Includes_PortNumber_In_LandingUrl_If_Not_Default() {
            // Arrange, Act and Assert
            SetupGenerateLandingUrlTest("http://localhost:43923", (s, c) => s.GenerateLandingUrl(c));
        }

        [TestMethod]
        public void GenerateLandingUrl_Does_Not_Include_PortNumber_In_LandingUrl_If_Default() {
            // Arrange, Act and Assert
            SetupGenerateLandingUrlTest("http://localhost:80", "http://localhost", (s, c) => s.GenerateLandingUrl(c));
            SetupGenerateLandingUrlTest("https://localhost:443", "https://localhost", (s, c) => s.GenerateLandingUrl(c));
        }

        [TestMethod]
        public void GenerateLandingUrl_With_Object_Argument_Adds_Object_Properties_To_Route_Data() {
            // Arrange and Act
            RouteValueDictionary actualRouteValues = SetupGenerateLandingUrlTest(TestLandingUrlBase, (s, c) => s.GenerateLandingUrl(c, new { foo = "bar" }));

            // Assert
            Assert.AreEqual("bar", actualRouteValues["foo"]);
        }

        [TestMethod]
        public void GenerateLandingUrl_With_RouteValueDictionary_Argument_Adds_Object_Properties_To_Route_Data() {
            // Arrange and Act
            RouteValueDictionary routeValues = new RouteValueDictionary {
                {"foo", "bar"}
            };
            RouteValueDictionary actualRouteValues = SetupGenerateLandingUrlTest(TestLandingUrlBase, (s, c) => s.GenerateLandingUrl(c, routeValues));

            // Assert
            Assert.AreEqual("bar", actualRouteValues["foo"]);
        }

        [TestMethod]
        public void GenerateLandingUrl_With_Object_Argument_Cannot_Override_Controller_Action_Page_And_Id_Parameters() {
            // Arrange, Act and Assert
            var obj = new {controller = "bar", action = "bar", id = "bar", page = "bar"};
            RouteValueDictionary actualRouteValues = SetupGenerateLandingUrlTest(TestLandingUrlBase, (s, c) => s.GenerateLandingUrl(c, obj));
        }

        [TestMethod]
        public void GenerateLandingUrl_With_RouteValueDictionary_Argument_Cannot_Override_Controller_Action_Page_And_Id_Parameters() {
            // Arrange, Act and Assert
            RouteValueDictionary routeValues = new RouteValueDictionary {
                {"controller", "bar"},
                {"action", "bar"},
                {"page", "bar"},
                {"id", "bar"}
            };
            RouteValueDictionary actualRouteValues = SetupGenerateLandingUrlTest(TestLandingUrlBase, (s, c) => s.GenerateLandingUrl(c, routeValues));
        }

        [TestMethod]
        public void CreateSessionPrincipal_Creates_IClaimsPrincipal_Containing_Specified_Claims() {
            // Arrange
            Claim[] claims = new[] {
                new Claim("foo", "bar"),
                new Claim("baz", "quz"),
                new Claim("zoop", "zork")
            };
            
            // Act
            IClaimsPrincipal sessionPrincipal = IdentitySource.CreateSessionPrincipal(claims);

            // Assert
            Assert.AreEqual(1, sessionPrincipal.Identities.Count);
            EnumerableAssert.ElementsAreEqual(claims, sessionPrincipal.Identities[0].Claims);
        }

        [TestMethod]
        public void ReturnToLastPage_Returns_RedirectToRouteResult_For_Current_Page_If_ReturnUrl_Invalid() {
            // Arrange
            Page expected = new Page();
            IdentitySource source = CreateIdentitySource();
            ControllerContext context = Mockery.CreateMockControllerContext();
            context.HttpContext.GetPortalContext().ActivePage = expected;
            context.HttpContext.Request.QueryString["returnUrl"] = "http://localhost:foo";

            // Act
            ActionResult result = source.ReturnToLastPage(context);

            // Assert
            ResultAssert.IsRedirectToRoute(result, new {controller = "Page", action = "View", page = expected});
        }

        [TestMethod]
        public void ReturnToLastPage_Returns_RedirectToRouteResult_For_Current_Page_If_ReturnUrl_Missing() {
            // Arrange
            Page expected = new Page();
            IdentitySource source = CreateIdentitySource();
            ControllerContext context = Mockery.CreateMockControllerContext();
            context.HttpContext.GetPortalContext().ActivePage = expected;

            // Act
            ActionResult result = source.ReturnToLastPage(context);

            // Assert
            ResultAssert.IsRedirectToRoute(result, new { controller = "Page", action = "View", page = expected });
        }

        [TestMethod]
        public void ReturnToLastPage_Returns_RedirectResult_For_ReturnUrl_If_Present_And_Valid() {
            // Arrange
            const string expected = "http://www.foo.example/";
            IdentitySource source = CreateIdentitySource();
            ControllerContext context = Mockery.CreateMockControllerContext();
            context.HttpContext.Request.QueryString["returnUrl"] = expected;

            // Act
            ActionResult result = source.ReturnToLastPage(context);

            // Assert
            ResultAssert.IsRedirect(result, expected);
        }

        private static RouteValueDictionary SetupGenerateLandingUrlTest(string requestUrlBase, Func<IdentitySource, ControllerContext, Uri> act) {
            return SetupGenerateLandingUrlTest(requestUrlBase, requestUrlBase, act);
        }

        private static RouteValueDictionary SetupGenerateLandingUrlTest(string requestUrlBase, string landingUrlBase, Func<IdentitySource, ControllerContext, Uri> act) {
            IdentitySource source = CreateIdentitySource();
            ControllerContext context = Mockery.CreateMockControllerContext(requestUrlBase);
            var mockRoute = new Mock<RouteBase>();
            RouteValueDictionary actualRouteValues = null;
            mockRoute.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                .Callback<RequestContext, RouteValueDictionary>((c, rvd) => actualRouteValues = rvd)
                .Returns(new VirtualPathData(mockRoute.Object, "foo/bar/baz"));

            source.Routes = new RouteCollection {
                mockRoute.Object
            };

            // Act
            Uri landingUrl = act(source, context);

            // Assert
            Assert.AreEqual(landingUrlBase + "/foo/bar/baz", landingUrl.ToString());
            Assert.AreEqual("Identity", actualRouteValues["controller"]);
            Assert.AreEqual("Land", actualRouteValues["action"]);
            Assert.IsTrue(actualRouteValues.ContainsKey("id"));
            Assert.IsTrue(actualRouteValues.ContainsKey("page"));
            Assert.IsNull(actualRouteValues["id"]);
            Assert.IsNull(actualRouteValues["page"]);
            return actualRouteValues;
        }

        private static IdentitySource CreateIdentitySource() {
            return new Mock<IdentitySource> { CallBase = true }.Object;
        }
    }
}
