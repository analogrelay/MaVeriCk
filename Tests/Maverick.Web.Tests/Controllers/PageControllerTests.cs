// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PageControllerTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PageControllerTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Maverick.Models;
using Maverick.Web.Controllers;
using Maverick.Web.Helpers;
using Maverick.Web.Models;
using Maverick.Web.ModuleFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;
using System.Security.Principal;

namespace Maverick.Web.Tests.Controllers {
    [TestClass]
    public class PageControllerTests : ControllerTestBase<PageController> {
        [TestMethod]
        public void Controller_Is_Exported_To_CompositionContainer() {
            // Assert
            CompositionAssert.IsExported(typeof(PageController));
        }

        [TestMethod]
        public void Controller_Is_Exported_With_NonShared_CreationPolicy() {
            // Assert
            CompositionAssert.HasCreationPolicy(typeof(PageController), CreationPolicy.NonShared);
        }

        [TestMethod]
        public void ModuleExecutor_Is_Imported_From_CompositionContainer() {
            // Assert
            CompositionAssert.IsImported<PageController>(p => p.ModuleExecutor, typeof(ModuleExecutionEngine));
        }

        [TestMethod]
        public void ViewPage_Has_Action_Name_View() {
            // Assert
            ActionFilterAssert.HasActionName(p => p.ViewPage(null, "foo"), "View");
        }

        [TestMethod]
        public void View_Action_Executes_Each_Module_On_Page() {
            // Arrange
            PageController controller = SetupController();

            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule3Id].GetExportedObject())
                .Never(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()));

            SetupMockActivePage(controller);

            // Act
            controller.ViewPage(null, String.Empty);

            // Assert
            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Verify(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()));
            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Verify(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()));
            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule2Id].GetExportedObject())
                .Verify(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()));
        }

        [TestMethod]
        public void View_Action_Returns_RenderModuleResult_From_SelectedModule_If_It_Is_PageOverrideResult() {
            // Arrange
            PageController controller = SetupController();
            SetupMockActivePage(controller);

            var moduleResult = new PageOverrideResult(new Mock<ActionResult>().Object);

            ControllerContext expectedContext = Mockery.CreateMockControllerContext();
            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Returns(new ModuleRequestResult { ActionResult = moduleResult, 
                                                     ControllerContext = expectedContext});

            // Act
            RenderModuleResult actualResult = controller.ViewPage(1, String.Empty) as RenderModuleResult;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(moduleResult, actualResult.ModuleRequestResult.ActionResult);
            Assert.AreSame(expectedContext, actualResult.ModuleRequestResult.ControllerContext);
        }

        [TestMethod]
        public void View_Action_Only_Executes_Selected_Module_If_It_Returns_A_PageOverride_Result() {
            // Arrange
            PageController controller = SetupController();
            SetupMockActivePage(controller);

            var moduleResult = new PageOverrideResult(new Mock<ActionResult>().Object);

            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Never(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()));

            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule2Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Returns(new ModuleRequestResult { ActionResult = moduleResult });

            // Act
            controller.ViewPage(2, String.Empty);

            // Assert (by finishing the test without the Assert.Fail call generated by the .Never call above)
        }

        [TestMethod]
        public void View_Action_Returns_PageViewModel_Containing_All_Module_Results_If_No_Module_Overrides_Page() {
            // Arrange
            PageController controller = SetupController();
            SetupMockActivePage(controller);

            ActionResult module1Result = new Mock<ActionResult>().Object;
            ActionResult module2Result = new Mock<ActionResult>().Object;

            ControllerContext module1Context = Mockery.CreateMockControllerContext();
            ControllerContext module2Context = Mockery.CreateMockControllerContext();

            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Returns(new ModuleRequestResult {
                    ActionResult = module1Result,
                    ControllerContext = module1Context
                });

            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule2Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Returns(new ModuleRequestResult {
                    ActionResult = module2Result,
                    ControllerContext = module2Context
                });

            // Act
            ViewResult result = controller.ViewPage(null, String.Empty) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Expected that a ViewResult would be returned");

            PageViewModel renderingModel = result.ViewData.Model as PageViewModel;
            Assert.IsNotNull(renderingModel, "Expected that the Model of the ViewResult would be a PageViewModel");
            Assert.AreEqual(2, renderingModel.Zones.Count);

            Assert.AreSame(module1Result, renderingModel["SidebarZone"].ModuleResults[0].ActionResult, "Expected that the action result from the 1st module would be the first module result");
            Assert.AreSame(module1Context, renderingModel["SidebarZone"].ModuleResults[0].ControllerContext, "Expected that the controller context from the 1st module would be the first controller context");

            Assert.AreSame(module1Result, renderingModel["SidebarZone"].ModuleResults[1].ActionResult, "Expected that the action result from the 2nd module would be the first module result");
            Assert.AreSame(module1Context, renderingModel["SidebarZone"].ModuleResults[1].ControllerContext, "Expected that the controller context from the 2nd module would be the first controller context");
            
            // NOTE: (3rd module also uses the "first" module application)
            Assert.AreSame(module2Result, renderingModel["ContentZone"].ModuleResults[0].ActionResult, "Expected that the action result from the 2nd module would be the first module result");
            Assert.AreSame(module2Context, renderingModel["ContentZone"].ModuleResults[0].ControllerContext, "Expected that the controller context from the 2nd module would be the first controller context");
        }

        [TestMethod]
        public void View_Action_Renders_Default_View() {
            // Arrange
            PageController controller = SetupController();

            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule3Id].GetExportedObject())
                .Never(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()));

            SetupMockActivePage(controller);

            // Act
            ViewResult result = controller.ViewPage(null, String.Empty) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Expected that a ViewResult would be returned");
            Assert.IsTrue(String.IsNullOrEmpty(result.ViewName), "Expected that the default view would be rendered");
        }

        [TestMethod]
        public void View_Action_Provides_Module_Application_With_Module_Object_Representing_Current_Module() {
            // Arrange
            PageController controller = SetupController();
            PortalRequestContext requestContext = controller.HttpContext.GetPortalContext();

            Module testModule = new Module {Id = 1, ModuleApplicationId = ModuleControllerTests.TestModule1Id};
            requestContext.ActivePage = new Page {
                Modules = new List<Module> { testModule }
            };

            ModuleRequestContext actualContext = null;
            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Callback<ModuleRequestContext>(context => actualContext = context);

            // Act
            controller.ViewPage(null, String.Empty);

            // Assert
            Assert.AreSame(testModule, actualContext.Module, "Expected that the first module would receive the correct module object");
        }

        [TestMethod]
        public void View_Action_Stores_ModuleApplication_In_ModuleContext() {
            // Arrange
            PageController controller = SetupController();
            PortalRequestContext requestContext = controller.HttpContext.GetPortalContext();

            Module testModule = new Module { Id = 1, ModuleApplicationId = ModuleControllerTests.TestModule1Id };
            requestContext.ActivePage = new Page {
                Modules = new List<Module> { testModule }
            };

            ModuleRequestContext actualContext = null;
            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Callback<ModuleRequestContext>(context => actualContext = context);

            // Act
            controller.ViewPage(null, String.Empty);

            // Assert
            Assert.AreSame(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject(), actualContext.Application, 
                           "Expected that the first module would receive the correct module application");
        }

        [TestMethod]
        public void View_Action_Provides_Empty_ModuleRoute_If_ModuleId_Does_Not_Match_ModuleId_Parameter() {
            RunSimpleModuleExecutionTest((ctl, ctx) => Assert.AreEqual(String.Empty, ctx.ModuleRoutingUrl, "Expected that the routing url provided would be empty"));
        }

        [TestMethod]
        public void View_Action_Provides_ModuleRoute_From_Parameter_If_ModuleId_Matches_Parameter() {
            // Arrange
            PageController controller = SetupController();
            PortalRequestContext requestContext = controller.HttpContext.GetPortalContext();

            Module testModule = new Module { Id = 1, ModuleApplicationId = ModuleControllerTests.TestModule1Id };
            Module otherModule = new Module { Id = 42, ModuleApplicationId = ModuleControllerTests.TestModule2Id };
            requestContext.ActivePage = new Page {
                Modules = new List<Module> { testModule, otherModule }
            };

            ModuleRequestContext actualContext = null;
            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Callback<ModuleRequestContext>(context => actualContext = context);

            ModuleRequestContext otherContext = null;
            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule2Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Callback<ModuleRequestContext>(context => otherContext = context);

            // Act
            controller.ViewPage(1, "Foo/Bar/Baz");

            // Assert
            Assert.AreEqual("Foo/Bar/Baz", actualContext.ModuleRoutingUrl,
                            "Expected that the routing url provided would be the module route");
            Assert.AreEqual(String.Empty, otherContext.ModuleRoutingUrl,
                            "Expected that the routing url provided would be empty");
        }

        [TestMethod]
        public void View_Action_Provides_Original_HttpContext_Base_To_Module_Application() {
            RunSimpleModuleExecutionTest((ctl, ctx) => Assert.AreSame(ctl.HttpContext, ctx.HttpContext,
                           "Expected that the request context provided to the module would be for the 'app' root"));
        }

        [TestMethod]
        public void View_Action_Groups_ModuleResults_By_Zone() {
            // Arrange
            PageController controller = SetupController();
            SetupMockActivePage(controller);

            ActionResult module1Result = new Mock<ActionResult>().Object;
            ActionResult module2Result = new Mock<ActionResult>().Object;

            ControllerContext module1Context = Mockery.CreateMockControllerContext();
            ControllerContext module2Context = Mockery.CreateMockControllerContext();

            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Returns(new ModuleRequestResult {
                    ActionResult = module1Result,
                    ControllerContext = module1Context
                });

            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule2Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Returns(new ModuleRequestResult {
                    ActionResult = module2Result,
                    ControllerContext = module2Context
                });

            // Act
            ViewResult result = controller.ViewPage(null, String.Empty) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Expected that a ViewResult would be returned");

            PageViewModel renderingModel = result.ViewData.Model as PageViewModel;
            Assert.IsNotNull(renderingModel, "Expected that the Model of the ViewResult would be a PageViewModel");
            Assert.AreEqual(2, renderingModel.Zones.Count);

            // Verify SidebarZone
            Assert.AreSame(module1Result, renderingModel["SidebarZone"].ModuleResults[0].ActionResult, "Expected that the action result from the 1st module would be the first module result");
            Assert.AreSame(module1Context, renderingModel["SidebarZone"].ModuleResults[0].ControllerContext, "Expected that the controller context from the 1st module would be the first controller context");
            Assert.AreSame(module1Result, renderingModel["SidebarZone"].ModuleResults[1].ActionResult, "Expected that the action result from the 2nd module would be the first module result");
            Assert.AreSame(module1Context, renderingModel["SidebarZone"].ModuleResults[1].ControllerContext, "Expected that the controller context from the 2nd module would be the first controller context");
            
            // NOTE: (3rd module also uses the "first" module application)
            Assert.AreSame(module2Result, renderingModel["ContentZone"].ModuleResults[0].ActionResult, "Expected that the action result from the 2nd module would be the first module result");
            Assert.AreSame(module2Context, renderingModel["ContentZone"].ModuleResults[0].ControllerContext, "Expected that the controller context from the 2nd module would be the first controller context");
        }

        //[TestMethod]
        //public void View_Action_Does_Not_Provide_ControlPanelModel_If_No_User() {
        //    // Arrange
        //    PageController controller = SetupController();
        //    controller.HttpContext.GetPortalContext().ActivePage = new Page() {Modules = new List<Module>()};
            
        //    // Act
        //    ActionResult result = controller.ViewPage(null, String.Empty);

        //    // Assert
        //    ResultAssert.IsViewWithModel<PageViewModel>(result, model => {
        //        Assert.IsNull(model.ControlPanelModel);
        //    });
        //}

        //[TestMethod]
        //public void View_Action_Does_Not_Provide_ControlPanelModel_If_User_Not_SuperUser() {
        //    // Arrange
        //    PageController controller = SetupController();
        //    controller.HttpContext.GetPortalContext().ActivePage = new Page() { Modules = new List<Module>() };
        //    SetupMockUser(controller, false);

        //    // Act
        //    ActionResult result = controller.ViewPage(null, String.Empty);

        //    // Assert
        //    ResultAssert.IsViewWithModel<PageViewModel>(result, model => {
        //        Assert.IsNull(model.ControlPanelModel);
        //    });
        //}

        //[TestMethod]
        //public void View_Action_Provides_ControlPanelModel_If_User_Is_SuperUser() {
        //    // Arrange
        //    PageController controller = SetupController();
        //    controller.HttpContext.GetPortalContext().ActivePage = new Page() { Modules = new List<Module>() };
        //    SetupMockUser(controller, true);

        //    // Act
        //    ActionResult result = controller.ViewPage(null, String.Empty);

        //    // Assert
        //    ResultAssert.IsViewWithModel<PageViewModel>(result, model => {
        //        Assert.IsNotNull(model.ControlPanelModel);
        //        Assert.AreEqual(controller.ModuleExecutor.ModuleApplications.Count, model.ControlPanelModel.Modules.Count());
        //        EnumerableAssert.ElementsMatch(controller.ModuleExecutor.ModuleApplications,
        //                                       model.ControlPanelModel.Modules,
        //                                       (e, c) =>
        //                                       e.MetadataView.Id.ToString("N") == c.Value &&
        //                                       e.MetadataView.Name == c.Text);
        //    });
        //}

        private static void SetupMockUser(Controller controller, bool superUser) {
            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(p => p.IsInRole("SuperUser"))
                .Returns(superUser);
            Mock.Get(controller.HttpContext)
                .Setup(c => c.User)
                .Returns(mockPrincipal.Object);
        }

        private static void RunSimpleModuleExecutionTest(Action<PageController, ModuleRequestContext> assert) {
            PageController controller = SetupController();
            PortalRequestContext requestContext = controller.HttpContext.GetPortalContext();

            Module testModule = new Module { Id = 1, ModuleApplicationId = ModuleControllerTests.TestModule1Id };
            requestContext.ActivePage = new Page {
                Modules = new List<Module> { testModule }
            };

            ModuleRequestContext actualContext = null;
            Mock.Get(controller.ModuleExecutor.ModuleApplications[ModuleControllerTests.TestModule1Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Callback<ModuleRequestContext>(context => actualContext = context);

            // Act
            controller.ViewPage(42, "Foo/Bar/Baz");

            // Assert
            assert(controller, actualContext);
        }

        private static PageController SetupController() {
            PageController controller = new PageController {
                ModuleExecutor = new ModuleExecutionEngine {
                    ModuleApplications = new ModuleApplicationCollection {
                        Mockery.CreateMockApplicationExport(ModuleControllerTests.TestModule1Id),
                        Mockery.CreateMockApplicationExport(ModuleControllerTests.TestModule2Id),
                        Mockery.CreateMockApplicationExport(ModuleControllerTests.TestModule3Id)
                    }
                }
            };
            controller.ControllerContext = Mockery.CreateMockControllerContext(controller);
            return controller;
        }

        private static void SetupMockActivePage(PageController controller) {
            controller.HttpContext.GetPortalContext().ActivePage = new Page {
                Modules = new List<Module> {
                    new Module {Id = 1, ModuleApplicationId = ModuleControllerTests.TestModule1Id, ZoneName = "SidebarZone"},
                    new Module {Id = 2, ModuleApplicationId = ModuleControllerTests.TestModule2Id, ZoneName = "ContentZone"},
                    new Module {Id = 3, ModuleApplicationId = ModuleControllerTests.TestModule1Id, ZoneName = "SidebarZone"}
                }
            };
        }

    }
}
