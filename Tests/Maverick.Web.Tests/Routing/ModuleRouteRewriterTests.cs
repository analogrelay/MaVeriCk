// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleRouteRewriterTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleRouteRewriterTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Routing;
using Maverick.Models;
using Maverick.Web.Helpers;
using Maverick.Web.ModuleFramework;
using Maverick.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;
using TargetResources = Maverick.Web.Properties.Resources;

namespace Maverick.Web.Tests.Routing {
    [TestClass]
    public class ModuleRouteRewriterTests {
        [TestMethod]
        public void GetRouteData_Is_Not_Supported() {
            // Arrange
            ModuleRouteRewriter router = new ModuleRouteRewriter();
            
            // Act and Assert
            ExceptionAssert.Throws<NotSupportedException>(() => router.GetRouteData(new Mock<HttpContextBase>().Object), TargetResources.Error_ModuleRouteRewriterOnlyForOutboundRouting);
        }

        [TestMethod]
        public void GetVirtualPath_Routes_Request_Through_ModuleRoutes() {
            // Arrange
            RequestContext requestContext = new RequestContext(Mockery.CreateMockHttpContext("http://localhost/Foo/Bar"), new RouteData());
            RouteValueDictionary values = new RouteValueDictionary();
            
            var mockRoute1 = new Mock<RouteBase>();
            mockRoute1.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                      .Returns(new VirtualPathData(mockRoute1.Object, "Foo"));

            ModuleRouteRewriter rewriter = new ModuleRouteRewriter {
                ModuleRoutes = new RouteCollection {
                    mockRoute1.Object
                }
            };

            // Act
            rewriter.GetVirtualPath(requestContext, values);

            // Assert
            mockRoute1.Verify(r => r.GetVirtualPath(requestContext, values));
        }

        [TestMethod]
        public void GetVirtualPath_Returns_Null_If_Module_Cant_Route_Request() {
            // Arrange
            RequestContext requestContext = new RequestContext(Mockery.CreateMockHttpContext("http://localhost/Foo/Bar"), new RouteData());
            RouteValueDictionary values = new RouteValueDictionary();

            var mockRoute1 = new Mock<RouteBase>();
            mockRoute1.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                      .Returns(() => null);

            ModuleRouteRewriter rewriter = new ModuleRouteRewriter {
                ModuleRoutes = new RouteCollection {
                    mockRoute1.Object
                }
            };

            // Act
            VirtualPathData pathData = rewriter.GetVirtualPath(requestContext, values);

            // Assert
            Assert.IsNull(pathData);
        }

        [TestMethod]
        public void GetVirtualPath_Puts_ModuleRoute_And_ModuleId_Into_New_RVD_And_Reroutes_Using_RouteTable() {
            // Arrange
            RequestContext requestContext = new RequestContext(Mockery.CreateMockHttpContext("http://localhost/Foo/Bar"), new RouteData());
            RouteValueDictionary values = new RouteValueDictionary();

            PortalRequestContext portalRequestContext = requestContext.HttpContext.GetPortalContext();
            portalRequestContext.ActiveModuleRequest = new ModuleRequestResult {
                Module = new Module {Id = 42}
            };

            var mockRoute1 = new Mock<RouteBase>();
            mockRoute1.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                      .Returns(new VirtualPathData(mockRoute1.Object, "Foo/Bar/Baz"));

            RouteValueDictionary providedValues = null;
            var mockRoute2 = new Mock<RouteBase>();
            mockRoute2.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>()))
                      .Callback<RequestContext, RouteValueDictionary>((c, v) => providedValues = v)
                      .Returns(new VirtualPathData(mockRoute2.Object, "Zoop/Zork/Zoink"));

            ModuleRouteRewriter rewriter = new ModuleRouteRewriter {
                ModuleRoutes = new RouteCollection {
                    mockRoute1.Object
                },
                RouteCollection = new RouteCollection {
                    mockRoute2.Object
                }
            };

            // Act
            VirtualPathData pathData = rewriter.GetVirtualPath(requestContext, values);

            // Assert
            Assert.AreEqual(42, providedValues["moduleId"]);
            Assert.AreEqual("Foo/Bar/Baz", providedValues["moduleRoute"]);

            Assert.AreSame(rewriter, pathData.Route);
            Assert.AreEqual("Zoop/Zork/Zoink", pathData.VirtualPath);
        }
    }
}
