// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleDelegatingViewEngineTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleDelegatingViewEngineTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Web.Mvc;
using Maverick.Web.Helpers;
using Maverick.Web.ModuleFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maverick.Web.Tests.ModuleFramework {
    [TestClass]
    public class ModuleDelegatingViewEngineTests {
        [TestMethod]
        public void Should_Forward_FindPartialView_To_Current_ModuleApplication_ViewEngineCollection() {
            // Arrange
            var mockEngines = new Mock<ViewEngineCollection>();
            ViewEngineResult result = new ViewEngineResult(new[] {"foo", "bar", "baz"});
            ControllerContext context = Mockery.CreateMockControllerContext();
            string viewName = "Foo";
            mockEngines.Setup(e => e.FindPartialView(context, viewName))
                       .Returns(result);

            SetupMockModuleApplication(context, mockEngines.Object);

            ModuleDelegatingViewEngine viewEngine = new ModuleDelegatingViewEngine();

            // Act
            ViewEngineResult engineResult = viewEngine.FindPartialView(context, viewName, true);

            // Assert
            mockEngines.Verify(e => e.FindPartialView(context, viewName));
            Assert.AreEqual("foo", engineResult.SearchedLocations.ElementAt(0));
            Assert.AreEqual("bar", engineResult.SearchedLocations.ElementAt(1));
            Assert.AreEqual("baz", engineResult.SearchedLocations.ElementAt(2));
        }

        [TestMethod]
        public void Should_Forward_FindView_To_Current_ModuleApplication_ViewEngineCollection() {
            // Arrange
            var mockEngines = new Mock<ViewEngineCollection>();
            ViewEngineResult result = new ViewEngineResult(new[] { "foo", "bar", "baz" });
            ControllerContext context = Mockery.CreateMockControllerContext();
            string viewName = "Foo";
            string masterName = "Bar";
            mockEngines.Setup(e => e.FindView(context, viewName, masterName))
                       .Returns(result);

            SetupMockModuleApplication(context, mockEngines.Object);

            ModuleDelegatingViewEngine viewEngine = new ModuleDelegatingViewEngine();

            // Act
            ViewEngineResult engineResult = viewEngine.FindView(context, viewName, masterName, true);

            // Assert
            mockEngines.Verify(e => e.FindView(context, viewName, masterName));
            Assert.AreEqual("foo", engineResult.SearchedLocations.ElementAt(0));
            Assert.AreEqual("bar", engineResult.SearchedLocations.ElementAt(1));
            Assert.AreEqual("baz", engineResult.SearchedLocations.ElementAt(2));
        }

        [TestMethod]
        public void Should_Track_ViewEngine_View_Pairs_On_FindView_And_Releases_View_Appropriately() {
            // Arrange
            var mockEngines = new Mock<ViewEngineCollection>();
            var mockEngine = new Mock<IViewEngine>();
            var mockView = new Mock<IView>();
            ViewEngineResult result = new ViewEngineResult(mockView.Object, mockEngine.Object);
            ControllerContext context = Mockery.CreateMockControllerContext();
            string viewName = "Foo";
            string masterName = "Bar";
            mockEngines.Setup(e => e.FindView(context, viewName, masterName))
                       .Returns(result);

            SetupMockModuleApplication(context, mockEngines.Object);

            ModuleDelegatingViewEngine viewEngine = new ModuleDelegatingViewEngine();

            // Act
            ViewEngineResult engineResult = viewEngine.FindView(context, viewName, masterName, true);
            viewEngine.ReleaseView(context, engineResult.View);

            // Assert
            mockEngine.Verify(e => e.ReleaseView(context, mockView.Object));
        }

        [TestMethod]
        public void Should_Track_ViewEngine_View_Pairs_On_FindPartialView_And_Releases_View_Appropriately() {
            // Arrange
            var mockEngines = new Mock<ViewEngineCollection>();
            var mockEngine = new Mock<IViewEngine>();
            var mockView = new Mock<IView>();
            ViewEngineResult result = new ViewEngineResult(mockView.Object, mockEngine.Object);
            ControllerContext context = Mockery.CreateMockControllerContext();
            string viewName = "Foo";
            mockEngines.Setup(e => e.FindPartialView(context, viewName))
                       .Returns(result);

            SetupMockModuleApplication(context, mockEngines.Object);

            ModuleDelegatingViewEngine viewEngine = new ModuleDelegatingViewEngine();

            // Act
            ViewEngineResult engineResult = viewEngine.FindPartialView(context, viewName, true);
            viewEngine.ReleaseView(context, engineResult.View);

            // Assert
            mockEngine.Verify(e => e.ReleaseView(context, mockView.Object));
        }

        [TestMethod]
        public void Should_Return_Failed_ViewEngineResult_For_FindView_If_No_Current_Module_Application() {
            // Arrange
            ModuleDelegatingViewEngine viewEngine = new ModuleDelegatingViewEngine();
            
            // Act
            ViewEngineResult engineResult = viewEngine.FindView(Mockery.CreateMockControllerContext(), "Foo", "Bar", true);

            // Assert
            Assert.IsNotNull(engineResult, "Expected that the ViewEngineResult would not be null");
            Assert.IsNull(engineResult.View, "Expected that no view would be returned");
            Assert.AreEqual(0, engineResult.SearchedLocations.Count(), "Expected that no searched locations would be specified");
        }

        [TestMethod]
        public void Should_Return_Failed_ViewEngineResult_For_FindPartialView_If_No_Current_Module_Application() {
            // Arrange
            ModuleDelegatingViewEngine viewEngine = new ModuleDelegatingViewEngine();

            // Act
            ViewEngineResult engineResult = viewEngine.FindPartialView(Mockery.CreateMockControllerContext(), "Foo", true);

            // Assert
            Assert.IsNotNull(engineResult, "Expected that the ViewEngineResult would not be null");
            Assert.IsNull(engineResult.View, "Expected that no view would be returned");
            Assert.AreEqual(0, engineResult.SearchedLocations.Count(), "Expected that no searched locations would be specified");
        }

        private static void SetupMockModuleApplication(ControllerContext context, ViewEngineCollection engines) {
            var mockApp = new Mock<ModuleApplication>();
            mockApp.Object.ViewEngines = engines;

            context.HttpContext.GetPortalContext().ActiveModuleRequest = new ModuleRequestResult {
                Application = mockApp.Object
            };
        }
    }
}
