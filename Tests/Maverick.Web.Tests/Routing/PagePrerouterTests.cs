// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PagePrerouterTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PagePrerouterTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
    public class PagePrerouterTests {
        [TestMethod]
        public void PagePrerouter_Is_Exported() {
            CompositionAssert.IsExported(typeof(PagePrerouter));
        }

        [TestMethod]
        public void GetRouteData_Returns_Null_If_No_Url_Prefixes_Match_Page_Path() {
            // Arrange
            PagePrerouter router = CreateRouterWithMockData();
            HttpContextBase httpContext = CreateMockHttpContext("~/Zoop/Zork/Zoink");

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            Assert.IsNull(routeData, "Expected that the router would not handle the route");
        }

        [TestMethod]
        public void GetRouteData_Sets_ActivePage_If_Url_Prefix_Matches_Page_Path() {
            // Arrange
            PagePrerouter router = CreateRouterWithMockData();
            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Baz/Bar/Zoop/Zork/Zoink");

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            PortalRequestContext context = httpContext.GetPortalContext();
            Assert.AreEqual(7, context.ActivePage.Id, "Expected that the correct page would be set as active");
        }

        [TestMethod]
        public void GetRouteData_Selects_Root_Page_If_AppRelativePath_Is_App_Root() {
            // Arrange
            PagePrerouter router = CreateRouterWithMockData();
            HttpContextBase httpContext = CreateMockHttpContext("~/");

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            PortalRequestContext context = httpContext.GetPortalContext();
            Assert.AreEqual(1, context.ActivePage.Id, "Expected that the correct page would be set as active");
        }

        [TestMethod]
        public void GetRouteData_Calls_All_Routes_After_Prerouter() {
            // Arrange
            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Baz/Bar/Zoop/Zork/Zoink");
            
            var mockRoute1 = new Mock<RouteBase>();
            var mockRoute2 = new Mock<RouteBase>();

            mockRoute1.Never(r => r.GetRouteData(It.IsAny<HttpContextBase>()));

            PagePrerouter router = CreateRouterWithMockData();
            router.RouteCollection = new RouteCollection {
                {"FirstRoute", mockRoute1.Object},
                {"Prerouter", router},
                {"NextRoute", mockRoute2.Object}
            };

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            mockRoute2.Verify(r => r.GetRouteData(It.IsAny<HttpContextBase>()));
        }

        [TestMethod]
        public void GetRouteData_Rewrites_Request_To_Remove_Page_Path() {
            // Arrange
            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Baz/Bar/Zoop/Zork/Zoink");

            HttpContextBase rewrittenContext = null;
            var mockRoute = new Mock<RouteBase>();
            mockRoute.Setup(r => r.GetRouteData(It.IsAny<HttpContextBase>()))
                     .Callback<HttpContextBase>(c => rewrittenContext = c);
            
            PagePrerouter router = CreateRouterWithMockData();
            router.RouteCollection = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute.Object}
            };

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            Assert.AreEqual("~/Zork/Zoink", rewrittenContext.Request.AppRelativeCurrentExecutionFilePath, "Expected that the request path would be rewritten to remove the page path before rerouting");
        }

        [TestMethod]
        public void GetRouteData_Properly_Rewrites_An_Exact_Page_Match() {
            // Arrange
            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Baz/Bar/Zoop");

            HttpContextBase rewrittenContext = null;
            var mockRoute = new Mock<RouteBase>();
            mockRoute.Setup(r => r.GetRouteData(It.IsAny<HttpContextBase>()))
                     .Callback<HttpContextBase>(c => rewrittenContext = c);

            PagePrerouter router = CreateRouterWithMockData();
            router.RouteCollection = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute.Object}
            };

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            Assert.AreEqual("~/", rewrittenContext.Request.AppRelativeCurrentExecutionFilePath, "Expected that the request path would be rewritten to remove the page path before rerouting");
        }

        [TestMethod]
        public void GetRouteData_Properly_Rewrites_An_Exact_Page_Match_With_Trailing_Slash() {
            // Arrange
            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Baz/Bar/Zoop/");

            HttpContextBase rewrittenContext = null;
            var mockRoute = new Mock<RouteBase>();
            mockRoute.Setup(r => r.GetRouteData(It.IsAny<HttpContextBase>()))
                     .Callback<HttpContextBase>(c => rewrittenContext = c);

            PagePrerouter router = CreateRouterWithMockData();
            router.RouteCollection = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute.Object}
            };

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            Assert.AreEqual("~/", rewrittenContext.Request.AppRelativeCurrentExecutionFilePath, "Expected that the request path would be rewritten to remove the page path before rerouting");
        }

        [TestMethod]
        public void GetRouteData_Returns_Null_If_ActivePage_Not_Null() {
            // Arrange
            var mockRoute1 = new Mock<RouteBase>();

            mockRoute1.Setup(r => r.GetRouteData(It.IsAny<HttpContextBase>()))
                      .Callback(() => Assert.Fail("Expected that the pre-router would be bypassed"));

            RouteCollection fakeRoutes = new RouteCollection {
                {"NextRoute", mockRoute1.Object},
            };

            PagePrerouter router = CreateRouterWithMockData();
            router.RouteCollection = fakeRoutes;

            HttpContextBase httpContext = Mockery.CreateMockHttpContext("http://localhost/Foo/Bar/Qux?id=43234&a=234#foo");
            httpContext.GetPortalContext().ActivePage = new Page();

            // Act
            RouteData routeData = router.GetRouteData(httpContext);

            // Assert
            Assert.IsNull(routeData, "Expected that the pre-router would be skipped");
        }

        [TestMethod]
        public void GetVirtualPath_Calls_All_Routes_After_Prerouter() {
            // Arrange
            var mockRoute1 = new Mock<RouteBase>();
            var mockRoute2 = new Mock<RouteBase>();

            mockRoute1.Never(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()));

            PagePrerouter router = new PagePrerouter();
            router.RouteCollection = new RouteCollection {
                {"FirstRoute", mockRoute1.Object},
                {"Prerouter", router},
                {"NextRoute", mockRoute2.Object}
            };

            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Bar/Baz");
            httpContext.GetPortalContext().ActivePage = new Page {Id = 4, Path = "/"};

            // Act
            VirtualPathData pathData = router.GetVirtualPath(new RequestContext(httpContext, new RouteData()),
                                                             new RouteValueDictionary());
            // Assert
            mockRoute2.Verify(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()));
        }

        [TestMethod]
        public void GetVirtualPath_Removes_Page_RouteValue_If_Present_Before_Rerouting() {
            // Arrange
            var mockRoute = new Mock<RouteBase>();

            PagePrerouter router = new PagePrerouter();
            router.RouteCollection = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute.Object}
            };

            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Bar/Baz");
            RouteValueDictionary values = new RouteValueDictionary(new {page = new Page {Path = "/"}, foo = "bar"});
            
            // Act
            RequestContext expectedRequestContext = new RequestContext(httpContext, new RouteData());
            VirtualPathData pathData = router.GetVirtualPath(expectedRequestContext, values);

            // Assert
            mockRoute.Verify(r => r.GetVirtualPath(expectedRequestContext, It.Is<RouteValueDictionary>(rvd =>
                String.Equals((string)rvd["foo"], "bar") && !rvd.ContainsKey("page")
            )));
        }

        [TestMethod]
        public void GetVirtualPath_Prepends_Path_Of_Page_From_Values_To_Path_Returned_By_Rerouting() {
            // Arrange
            var mockRoute = new Mock<RouteBase>();

            mockRoute.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                     .Returns(new VirtualPathData(mockRoute.Object, "Zoop/Zork/Zoink"));

            PagePrerouter router = new PagePrerouter();
            router.RouteCollection = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute.Object}
            };

            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Bar/Baz");
            RouteValueDictionary values = new RouteValueDictionary(new { page = new Page { Path = "/Foo/Bar" }});

            // Act
            VirtualPathData pathData = router.GetVirtualPath(new RequestContext(httpContext, new RouteData()), values);

            // Assert
            Assert.AreEqual("Foo/Bar/Zoop/Zork/Zoink", pathData.VirtualPath, "Expected that the correct page prefix would be prepended");
        }

        [TestMethod]
        public void GetVirtualPath_Prepends_Path_Of_ActivePage_From_Context_If_No_Path_In_Values() {
            // Arrange
            var mockRoute = new Mock<RouteBase>();

            mockRoute.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                     .Returns(new VirtualPathData(mockRoute.Object, "Zoop/Zork/Zoink"));

            PagePrerouter router = new PagePrerouter();
            router.RouteCollection = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute.Object}
            };

            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Bar/Baz");
            httpContext.GetPortalContext().ActivePage = new Page {Path = "/Foo/Bar"};
            RouteValueDictionary values = new RouteValueDictionary();

            // Act
            VirtualPathData pathData = router.GetVirtualPath(new RequestContext(httpContext, new RouteData()), values);

            // Assert
            Assert.AreEqual("Foo/Bar/Zoop/Zork/Zoink", pathData.VirtualPath, "Expected that the correct page prefix would be prepended");
        }

        [TestMethod]
        public void GetVirtualPath_Correctly_Prepends_Page_Path_If_Route_Result_Is_Empty() {
            // Arrange
            var mockRoute = new Mock<RouteBase>();

            mockRoute.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                     .Returns(new VirtualPathData(mockRoute.Object, String.Empty));

            PagePrerouter router = new PagePrerouter();
            router.RouteCollection = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute.Object}
            };

            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Bar/Baz");
            httpContext.GetPortalContext().ActivePage = new Page { Path = "/Foo/Bar" };
            RouteValueDictionary values = new RouteValueDictionary();

            // Act
            VirtualPathData pathData = router.GetVirtualPath(new RequestContext(httpContext, new RouteData()), values);

            // Assert
            Assert.AreEqual("Foo/Bar", pathData.VirtualPath, "Expected that the correct page prefix would be prepended");
        }

        [TestMethod]
        public void GetVirtualPath_Correctly_Prepends_Page_Path_If_ActivePage_Is_Root_Page() {
            // Arrange
            var mockRoute = new Mock<RouteBase>();

            mockRoute.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                     .Returns(new VirtualPathData(mockRoute.Object, "Zoop/Zork/Zoink"));

            PagePrerouter router = new PagePrerouter();
            router.RouteCollection = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute.Object}
            };

            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Bar/Baz");
            httpContext.GetPortalContext().ActivePage = new Page { Path = "/" };
            RouteValueDictionary values = new RouteValueDictionary();

            // Act
            VirtualPathData pathData = router.GetVirtualPath(new RequestContext(httpContext, new RouteData()), values);

            // Assert
            Assert.AreEqual("Zoop/Zork/Zoink", pathData.VirtualPath, "Expected that the correct page prefix would be prepended");
        }

        [TestMethod]
        public void GetVirtualPath_Returns_Null_If_Null_Page_Present_In_Values() {
            // Arrange
            var mockRoute = new Mock<RouteBase>();

            mockRoute.Never(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()));
                     
            PagePrerouter router = new PagePrerouter();
            router.RouteCollection = new RouteCollection {
                {"Prerouter", router},
                {"NextRoute", mockRoute.Object}
            };

            HttpContextBase httpContext = CreateMockHttpContext("~/Foo/Bar/Baz");
            RouteValueDictionary values = new RouteValueDictionary(new {page = (Page)null});

            // Act
            VirtualPathData pathData = router.GetVirtualPath(new RequestContext(httpContext, new RouteData()), values);

            // Assert
            Assert.IsNull(pathData, "Expected that the prerouter would be bypassed");
        }

        private static HttpContextBase CreateMockHttpContext(string appRelativePath) {
            var mockContext = new Mock<HttpContextBase>();
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.SetupGet(r => r.ApplicationPath)
                       .Returns("/TestAppPath");
            mockRequest.SetupGet(r => r.Url)
                       .Returns(new Uri(String.Format("http://localhost/TestAppPath{0}?query=q#bar", appRelativePath.TrimStart('~'))));
            mockRequest.SetupGet(r => r.AppRelativeCurrentExecutionFilePath)
                       .Returns(appRelativePath);
            mockContext.SetupGet(c => c.Request)
                       .Returns(mockRequest.Object);
            mockContext.SetupGet(c => c.Items)
                       .Returns(new Dictionary<string, object>());

            return mockContext.Object;
        }

        private static PagePrerouter CreateRouterWithMockData() {
            var mockPageService = new Mock<PageRepository> { CallBase = true };
            mockPageService.Setup(s => s.GetAll())
                             .Returns(CreatePages());
            return new PagePrerouter {
                PageRepository = mockPageService.Object
            };
        }

        private static IEntityQuery<Page> CreatePages() {
            return new List<Page> {
                new Page {Id = 1, Title = "Test Page 1", Path = "/"}, 
                new Page {Id = 2, Title = "Test Page 2", Path = "/Foo"}, 
                new Page {Id = 3, Title = "Test Page 3", Path = "/Bar"}, 
                new Page {Id = 4, Title = "Test Page 4", Path = "/Foo/Bar"}, 
                new Page {Id = 5, Title = "Test Page 5", Path = "/Bar/Qux/Baz"}, 
                new Page {Id = 6, Title = "Test Page 6", Path = "/Foo/Baz/Bar"}, 
                new Page {Id = 7, Title = "Test Page 6", Path = "/Foo/Baz/Bar/Zoop"}, 
                new Page {Id = 8, Title = "Test Page 7", Path = "/Foo/Baz/Foo/Bar/Baz"}, 
            }.AsEntityQuery();
        }
    }
}
