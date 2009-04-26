// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalVirtualizationPrerouterTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalVirtualizationPrerouterTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using Maverick.Data;
using Maverick.DomainServices;
using Maverick.Models;
using Maverick.Web.Helpers;
using Maverick.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Web.Tests.Routing {
    [TestClass]
    public class PortalVirtualizationPrerouterTests {
        [TestMethod]
        public void PortalVirtualizationPrerouter_Is_Exported() {
            CompositionAssert.IsExported(typeof(PortalVirtualizationPrerouter));
        }

        [TestMethod]
        public void GetRouteData_Returns_Null_If_No_PortalPrefix_Matches_Url() {
            // Arrange
            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://www.test.example/Foo/Bar/Baz?id=42");

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            Assert.IsNull(routeData, "Expected that the router would not handle the route");
        }

        [TestMethod]
        public void GetRouteData_Selects_Correct_Portal_Prefix_And_Puts_PortalContext_Into_HttpContext() {
            // Arrange
            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            router.RouteCollection = new RouteCollection();
            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar/Qux?id=43234&a=234#foo");

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            PortalRequestContext portalRequestContext = httpContext.GetPortalContext();
            Portal activePortal = portalRequestContext.ActivePortal;
            Assert.IsNotNull(activePortal, "Expected that the router would set the active portal");
            Assert.AreEqual(4, activePortal.Id, "Expected that the router would select the portal with the longest matching prefix");
            Assert.AreEqual(4, portalRequestContext.ActivePortalPrefix.Id, "Expected that the router would set the active portal prefix");
            Assert.AreEqual("localhost/Foo/Bar/", portalRequestContext.ActivePortalPrefix.Prefix, "Expected that the router would set the active portal prefix");
        }

        [TestMethod]
        public void GetRouteData_Calls_All_Routes_After_Prerouter() {
            // Arrange
            RouteData expectedRouteData = new RouteData();
            
            var mockRoute1 = new Mock<RouteBase>();
            var mockRoute2 = new Mock<RouteBase>();

            mockRoute1.Never(r => r.GetRouteData(It.IsAny<HttpContextBase>()));
            mockRoute2.Setup(r => r.GetRouteData(It.IsAny<HttpContextBase>()))
                      .Returns(expectedRouteData);

            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            RouteCollection fakeRoutes = new RouteCollection {
                {"NextRoute", mockRoute1.Object},
                {"Prerouter", router},
                {"AnotherRoute", mockRoute2.Object}
            };

            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar/Qux?id=43234&a=234#foo");
            router.RouteCollection = fakeRoutes;
            
            // Act
            RouteData actualRouteData = router.GetRouteData(httpContext);

            // Assert
            mockRoute2.Verify(r => r.GetRouteData(It.IsAny<HttpContextBase>()));
            Assert.AreEqual(expectedRouteData, actualRouteData, "Expected that the first non-null return value from the other routes would be returned");
        }

        [TestMethod]
        public void GetRouteData_Rewrites_Request_Before_Rerouting() {
            // Arrange
            HttpContextBase rewrittenContext = null;
            var mockRoute1 = new Mock<RouteBase>();

            mockRoute1.Setup(r => r.GetRouteData(It.IsAny<HttpContextBase>()))
                      .Callback<HttpContextBase>(c => rewrittenContext = c);
            
            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            RouteCollection fakeRoutes = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute1.Object},
            };

            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar/Qux?id=43234#foo");
            NameValueCollection queryString = new NameValueCollection();
            Mock.Get(httpContext.Request).Setup(r => r.QueryString).Returns(queryString);
            router.RouteCollection = fakeRoutes;

            // Act
            router.GetRouteData(httpContext);

            // Assert
            Assert.AreEqual("~/Qux/", rewrittenContext.Request.AppRelativeCurrentExecutionFilePath, "Expected that the remainder of the URL would be rewritten as the new AppRelativeCurrentExecutionFilePath");
            Assert.AreSame(queryString, rewrittenContext.Request.QueryString, "Expected that the previous query string would be preserved");
            Assert.AreEqual("http://localhost/Qux/?id=43234#foo", rewrittenContext.Request.Url.ToString(), "Expected that the URL would be rewritten to remove the portal-specific path");
        }

        [TestMethod]
        public void GetRouteData_Returns_Null_If_ActivePortal_Not_Null() {
            // Arrange
            var mockRoute1 = new Mock<RouteBase>();

            mockRoute1.Setup(r => r.GetRouteData(It.IsAny<HttpContextBase>()))
                      .Callback(() => Assert.Fail("Expected that the pre-router would be bypassed"));

            RouteCollection fakeRoutes = new RouteCollection {
                {"NextRoute", mockRoute1.Object},
            };

            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            router.RouteCollection = fakeRoutes;
            
            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar/Qux?id=43234&a=234#foo");
            httpContext.GetPortalContext().ActivePortal = new Portal();
            
            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            Assert.IsNull(routeData, "Expected that the pre-router would be skipped");
        }

        [TestMethod]
        public void GetRouteData_Correctly_Routes_Root_Of_A_Portal_Prefix_Without_Trailing_Slash() {
            // Arrange
            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            router.RouteCollection = new RouteCollection();
            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar");

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            PortalRequestContext portalRequestContext = httpContext.GetPortalContext();
            Portal activePortal = portalRequestContext.ActivePortal;
            Assert.IsNotNull(activePortal, "Expected that the router would set the active portal");
            Assert.AreEqual(4, activePortal.Id, "Expected that the router would select the portal with the longest matching prefix");
            Assert.AreEqual(4, portalRequestContext.ActivePortalPrefix.Id, "Expected that the router would set the active portal prefix");
            Assert.AreEqual("localhost/Foo/Bar/", portalRequestContext.ActivePortalPrefix.Prefix, "Expected that the router would set the active portal prefix");
        }

        [TestMethod]
        public void GetRouteData_Ignores_Port_If_No_Route_Specifically_Matches_It() {
            // Arrange
            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            router.RouteCollection = new RouteCollection();
            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost:4999");

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            PortalRequestContext portalRequestContext = httpContext.GetPortalContext();
            Portal activePortal = portalRequestContext.ActivePortal;
            Assert.IsNotNull(activePortal, "Expected that the router would set the active portal");
            Assert.AreEqual(1, activePortal.Id, "Expected that the router would select the portal with the longest matching prefix");
            Assert.AreEqual(1, portalRequestContext.ActivePortalPrefix.Id, "Expected that the router would set the active portal prefix");
            Assert.AreEqual("localhost/", portalRequestContext.ActivePortalPrefix.Prefix, "Expected that the router would set the active portal prefix");
        }

        [TestMethod]
        public void GetVirtualPath_Calls_All_Other_Routes_To_Route_Request() {
            // Arrange
            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar/Qux?id=43234&a=234");
            SetActivePortalPrefix(httpContext, "localhost/Foo/Bar/Qux");
            SetMockApplicationPath(httpContext, "/Foo");

            var mockRoute1 = new Mock<RouteBase>();
            var mockRoute2 = new Mock<RouteBase>();

            mockRoute1.Never(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()));

            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            RouteCollection fakeRoutes = new RouteCollection {
                {"FirstRoute", mockRoute1.Object},
                {"Prerouter", router},
                {"NextRoute", mockRoute2.Object}
            };

            router.RouteCollection = fakeRoutes;

            // Act
            RequestContext expectedRequestContext = new RequestContext(httpContext, new RouteData());
            RouteValueDictionary expectedValues = new RouteValueDictionary();
            router.GetVirtualPath(expectedRequestContext,
                                  expectedValues);

            // Assert
            mockRoute2.Verify(r => r.GetVirtualPath(expectedRequestContext, expectedValues));
        }

        [TestMethod]
        public void GetVirtualPath_Returns_Null_If_No_Active_PortalPrefix() {
            // Arrange
            var mockRoute1 = new Mock<RouteBase>();
            mockRoute1.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                      .Returns(new VirtualPathData(mockRoute1.Object, "Foo"));
            RouteCollection fakeRoutes = new RouteCollection {
                {"NextRoute", mockRoute1.Object},
            };
            
            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar/Qux?id=43234&a=234");
            httpContext.GetPortalContext().ActivePortalPrefix = null;
            PortalVirtualizationPrerouter router = new PortalVirtualizationPrerouter();
            
            // Act
            VirtualPathData data = router.GetVirtualPath(new RequestContext(httpContext, new RouteData()),
                                                         new RouteValueDictionary());

            // Assert
            Assert.IsNull(data, "Expected that the pre-router would not run if there was no active portal");
        }

        [TestMethod]
        public void GetVirtualPath_Forwards_RequestContext_And_RouteValues_To_Routes() {
            // Arrange
            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar/Qux?id=43234&a=234");
            SetActivePortalPrefix(httpContext, "localhost/Foo/Bar/Qux");
            SetMockApplicationPath(httpContext, "/Foo");
            
            RequestContext context = new RequestContext(httpContext, new RouteData());
            RouteValueDictionary values = new RouteValueDictionary();

            var mockRoute1 = new Mock<RouteBase>();
            var mockRoute2 = new Mock<RouteBase>();
            
            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            RouteCollection fakeRoutes = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute1.Object},
                {"AnotherRoute", mockRoute2.Object},
            };

            router.RouteCollection = fakeRoutes;

            // Act
            VirtualPathData pathData = router.GetVirtualPath(context, values);

            // Assert
            mockRoute1.Verify(r => r.GetVirtualPath(context, values));
            mockRoute2.Verify(r => r.GetVirtualPath(context, values));

            Assert.IsNull(pathData, "Expected that the path data returned by the pre-router would be null");
        }

        [TestMethod]
        public void GetVirtualPath_Prepends_AppRelativePortion_Of_ActivePortalPrefix_To_VirtualPath_From_Other_Routes() {
            // Arrange
            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar/Qux?id=43234&a=234");
            SetActivePortalPrefix(httpContext, "localhost/Foo/Bar/Qux");
            SetMockApplicationPath(httpContext, "/Foo");
            
            var mockRoute1 = new Mock<RouteBase>();
            mockRoute1.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                      .Returns(new VirtualPathData(mockRoute1.Object, "Incoming/Routed/Path"));

            PortalVirtualizationPrerouter router = CreateRouterWithMockData();
            RouteCollection fakeRoutes = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute1.Object},
            };

            router.RouteCollection = fakeRoutes;
            
            // Act
            VirtualPathData pathData = router.GetVirtualPath(new RequestContext(httpContext, new RouteData()), new RouteValueDictionary());

            // Assert
            Assert.AreEqual("Bar/Qux/Incoming/Routed/Path", pathData.VirtualPath);
        }

        private static void SetMockApplicationPath(HttpContextBase httpContext, string applicationPath) {
            Mock.Get(httpContext.Request)
                .SetupGet(r => r.ApplicationPath)
                .Returns(applicationPath);
        }

        private static void SetActivePortalPrefix(HttpContextBase httpContext, string prefix) {
            httpContext.GetPortalContext().ActivePortalPrefix = new PortalPrefix {Id = 1, Prefix = prefix};
        }

        private PortalVirtualizationPrerouter CreateRouterWithMockData() {
            var mockPrefixService = new Mock<PortalPrefixRepository> { CallBase = true };
            mockPrefixService.Setup(s => s.GetAll())
                             .Returns(CreateMockPrefixes());
            return new PortalVirtualizationPrerouter {
                PortalPrefixRepository = mockPrefixService.Object
            };
        }

        private IEntityQuery<PortalPrefix> CreateMockPrefixes() {
            return new List<PortalPrefix> {
                new PortalPrefix {Id = 1, Prefix = "localhost/", Portal = new Portal { Id = 1 }}, 
                new PortalPrefix {Id = 2, Prefix = "localhost/Foo/", Portal = new Portal { Id = 2 }},
                new PortalPrefix {Id = 3, Prefix = "localhost/Foo/Bar/Baz/", Portal = new Portal { Id = 3 }},
                new PortalPrefix {Id = 4, Prefix = "localhost/Foo/Bar/", Portal = new Portal { Id = 4 }},
                new PortalPrefix {Id = 5, Prefix = "localhost:8080/", Portal = new Portal { Id = 5 }},
                new PortalPrefix {Id = 6, Prefix = "localhost:8080/Quz/", Portal = new Portal { Id = 6 }},
            }.AsEntityQuery();
        }
    }
}
