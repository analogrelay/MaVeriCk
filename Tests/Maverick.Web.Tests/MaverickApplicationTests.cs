// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MaverickApplicationTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MaverickApplicationTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using System.IO;
using Maverick.Web.ModuleFramework;
using Maverick.Web.Theming;
using Maverick.Web.Controllers;
using System.Web;
using System.ComponentModel.Composition;

namespace Maverick.Web.Tests {
    [TestClass]
    public class MaverickApplicationTests {
        private const string TestDirectory = "Baz\\Bar\\Foo";

        [Export(WebContractNames.DataContextManagerName)]
        public static readonly string TestDataContextManagerName = "Foo";

        [TestMethod]
        public void RegisterRoutes_Requires_Non_Null_RouteCollection() {
            AutoTester.ArgumentNull<RouteCollection>(marker => MaverickApplication.RegisterRoutes(marker));
        }

        [TestMethod]
        public void FirstRoute_Should_Ignore_AXD_Urls() {
            RunRouteTest<Route>(0, r => {
                Assert.IsInstanceOfType(r.RouteHandler, typeof(StopRoutingHandler));
                Assert.AreEqual("{resource}.axd/{*pathInfo}", r.Url);
            });
        }

        [TestMethod]
        public void PortalVirtualizationRoute_Should_Preroute_To_Portal() {
            RunRouteTest<PortalVirtualizationPrerouter>("PortalVirtualization");
        }

        [TestMethod]
        public void PageRoute_Should_Preroute_To_Page() {
            RunRouteTest<PagePrerouter>("PageRouting");
        }

        [TestMethod]
        public void ModuleRoute_Should_Route_To_Module() {
            RunRouteTest<Route>("ModuleRoute", r => {
                Assert.IsInstanceOfType(r.RouteHandler, typeof(MvcRouteHandler));
                Assert.AreEqual("{moduleId}/{*moduleRoute}", r.Url);
                DictionaryAssert.ContainsEntries(new {controller="Page", action="View", moduleId=string.Empty, moduleRoute=string.Empty}, r.Defaults);
                DictionaryAssert.ContainsEntries(new {moduleId=@"[0-9]*"}, r.Constraints);
            });
        }

        [TestMethod]
        public void DefaultRoute_Should_Route_To_Maverick_System_Controller() {
            RunRouteTest<Route>("Default", r => {
                Assert.IsInstanceOfType(r.RouteHandler, typeof(MvcRouteHandler));
                Assert.AreEqual("_{controller}/{action}/{id}", r.Url);
                DictionaryAssert.ContainsEntries(new { controller = "Page", action = "View", id = string.Empty }, r.Defaults);
            });
        }

        [TestMethod]
        public void SetupMvcExtensions_Configures_ModuleDelegatingViewEngine_As_First_Engine() {
            // Arrange and Act
            PrepareSetupMvcExtensionsTest();

            // Assert
            Assert.IsInstanceOfType(ViewEngines.Engines[0], typeof(ModuleDelegatingViewEngine));
        }

        [TestMethod]
        public void SetupMvcExtensions_Configures_ThemedWebFormViewEngine_As_Second_Engine() {
            // Arrange and Act
            PrepareSetupMvcExtensionsTest();

            // Assert
            Assert.IsInstanceOfType(ViewEngines.Engines[1], typeof(ThemedWebFormViewEngine));
        }

        [TestMethod]
        public void SetupMvcExtensions_Configures_Two_Engines() {
            // Arrange and Act
            PrepareSetupMvcExtensionsTest();

            // Assert
            Assert.AreEqual(2, ViewEngines.Engines.Count);
        }

        [TestMethod]
        public void SetupMvcExtensions_Configures_CompositionContainerControllerFactory_As_ControllerFactory() {
            // Arrange and Act
            PrepareSetupMvcExtensionsTest();

            // Assert
            Assert.IsInstanceOfType(ControllerBuilder.Current.GetControllerFactory(),
                                    typeof(CompositionContainerControllerFactory));
        }

        [TestMethod]
        public void DataContextManagerName_Returns_Default_If_No_Container_And_Not_Overridden() {
            // Arrange, Act and Assert
            Assert.AreEqual(MaverickApplication.DefaultDataContextManagerName, MaverickApplication.DataContextManagerName);
            MaverickApplication.DataContextManagerName = null;
        }

        [TestMethod]
        public void DataContextManagerName_Returns_Provided_Value_If_Overridden() {
            // Arrange
            MaverickApplication.DataContextManagerName = TestDataContextManagerName;

            // Act and Assert
            Assert.AreEqual(TestDataContextManagerName, MaverickApplication.DataContextManagerName);
            MaverickApplication.DataContextManagerName = null;
        }

        [TestMethod]
        public void DataContextManagerName_Returns_Default_If_Container_Does_Not_Contain_Export() {
            // Arrange
            MaverickApplication.Container = new CompositionContainer();

            // Act and Assert
            Assert.AreEqual(MaverickApplication.DefaultDataContextManagerName, MaverickApplication.DataContextManagerName);
            MaverickApplication.DataContextManagerName = null;
            MaverickApplication.Container = null;
        }

        [TestMethod]
        public void DataContextManagerName_Returns_Value_Imported_From_Container_If_Present() {
            // Arrange
            MaverickApplication.Container = new CompositionContainer(new TypeCatalog(typeof(MaverickApplicationTests)));

            // Act and Assert
            Assert.AreEqual(TestDataContextManagerName, MaverickApplication.DataContextManagerName);
            MaverickApplication.DataContextManagerName = null;
            MaverickApplication.Container = null;
        }

        [TestMethod]
        public void CurrentContext_Returns_CurrentHttpContext_If_Not_Overridden() {
            // Arrange
            HttpRequest req = new HttpRequest("Foo.txt", "http://localhost", "foo=bar");
            HttpResponse resp = new HttpResponse(new StringWriter());
            HttpContext context = new HttpContext(req, resp);
            HttpContext.Current = context;

            // Act
            HttpContextBase httpContext = MaverickApplication.CurrentContext;

            // Assert
            // Gonna have to use reflection...
            Assert.AreSame(context, httpContext.GetType().GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(httpContext));

            MaverickApplication.CurrentContext = null;
        }

        [TestMethod]
        public void CurrentContext_Returns_Provided_HttpContextBase_If_Overridden() {
            // Arrange
            HttpContextBase expected = Mockery.CreateMockHttpContext();
            MaverickApplication.CurrentContext = expected;

            // Act
            HttpContextBase actual = MaverickApplication.CurrentContext;

            // Assert
            // Gonna have to use reflection...
            Assert.AreSame(expected, actual);

            MaverickApplication.CurrentContext = null;
        }

        [TestMethod]
        public void Routes_Returns_RouteTable_Routes_If_Not_Overridden() {
            // Assert
            Assert.AreSame(RouteTable.Routes, MaverickApplication.Routes);
        }

        [TestMethod]
        public void Routes_Returns_Provided_RouteCollection_If_Overridden() {
            // Arrange
            RouteCollection expected = new RouteCollection();
            MaverickApplication.Routes = expected;

            // Act
            RouteCollection actual = MaverickApplication.Routes;
            
            // Assert
            Assert.AreSame(expected, actual);
        }

        private static void PrepareSetupMvcExtensionsTest() {
            // Arrange
            MaverickApplication.Container = new CompositionContainer();

            // Act
            MaverickApplication.SetupMvcExtensions();

            MaverickApplication.Container = null;
        }

        private static void RunRouteTest<TRoute>(int routeIndex, Action<TRoute> assert) where TRoute : RouteBase {
            RunRouteTest(routes => routes[routeIndex], assert);
        }

        private static void RunRouteTest<TRoute>(string routeName) where TRoute : RouteBase {
            RunRouteTest<TRoute>(routeName, r => { });
        }

        private static void RunRouteTest<TRoute>(string routeName, Action<TRoute> assert) where TRoute : RouteBase {
            RunRouteTest(routes => routes[routeName], assert);
        }

        private static void RunRouteTest<TRoute>(Func<RouteCollection, RouteBase> routeSelector, Action<TRoute> assert) where TRoute : RouteBase {
            // Arrange
            MaverickApplication.PortalVirtualizationPrerouter = new PortalVirtualizationPrerouter();
            MaverickApplication.PagePrerouter = new PagePrerouter();

            RouteCollection routes = new RouteCollection();
            MaverickApplication.RegisterRoutes(routes);

            // Act
            TRoute r = routeSelector(routes) as TRoute;

            // Assert
            Assert.IsNotNull(r);
            assert(r);
        }
    }
}
