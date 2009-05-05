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
using System.Linq.Expressions;
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
using System.Web;
using System.Threading;

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
        public void ControlPanelId_Is_Imported_From_CompositionContainer() {
            // Assert
            CompositionAssert.IsImported<PageController>(p => p.ControlPanelId, WebContractNames.ControlPanelId);
        }

        [TestMethod]
        public void ViewPage_Has_Action_Name_View() {
            // Assert
            ActionFilterAssert.HasActionName(p => p.ViewPage(null, "foo"), "View");
        }

        [TestMethod]
        public void View_Action_Executes_ControlPanel_If_Id_Is_Not_NullOrEmpty() {
            // Arrange
            PageController controller = SetupController();
            controller.ControlPanelId = ModuleControllerTests.TestModule3Id.ToString("N");

            SetupMockActivePage(controller);

            ModuleRequestResult expectedResult = new ModuleRequestResult();
            Mock.Get(controller.ModuleExecutor)
                .Setup(CreateExecuteModuleExpression(ModuleControllerTests.TestModule3Id))
                .Returns(expectedResult);

            // Act
            ActionResult result = controller.ViewPage(null, String.Empty);

            // Assert
            ResultAssert.IsViewWithModel<PageViewModel>(result, model => {
                Assert.AreSame(expectedResult, model.ControlPanelResult);
            });
        }

        [TestMethod]
        public void View_Action_Returns_RenderModuleResult_If_ControlPanel_Returns_PageOverrideResult() {
            // Arrange
            PageController controller = SetupController();
            controller.ControlPanelId = ModuleControllerTests.TestModule3Id.ToString("N");

            SetupMockActivePage(controller);

            ModuleRequestResult expectedResult = new ModuleRequestResult() {
                ActionResult = new PageOverrideResult(new EmptyResult()),
                ControllerContext = Mockery.CreateMockControllerContext()
            };
            Mock.Get(controller.ModuleExecutor)
                .Setup(CreateExecuteModuleExpression(ModuleControllerTests.TestModule3Id))
                .Returns(expectedResult);

            // Act
            ActionResult result = controller.ViewPage(null, String.Empty);

            // Assert
            ResultAssert.IsRenderModule(result, expectedResult);
        }

        [TestMethod]
        public void View_Action_Does_Not_Execute_ControlPanel_If_Id_Is_Null() {
            // Arrange
            PageController controller = SetupController();
            
            SetupMockActivePage(controller);
            Mock.Get(controller.ModuleExecutor)
                .Never(CreateExecuteModuleExpression(ModuleControllerTests.TestModule3Id));

            // Act
            controller.ViewPage(null, String.Empty);

            // Assert
        }

        [TestMethod]
        public void View_Action_Does_Not_Execute_ControlPanel_If_Id_Is_Invalid_Guid() {
            // Arrange
            PageController controller = SetupController();
            controller.ControlPanelId = "FooBarBaz";

            SetupMockActivePage(controller);
            Mock.Get(controller.ModuleExecutor)
                .Never(CreateExecuteModuleExpression(ModuleControllerTests.TestModule3Id));

            // Act
            controller.ViewPage(null, String.Empty);

            // Assert
        }

        [TestMethod]
        public void View_Action_Executes_Each_Module_On_Page() {
            // Arrange
            PageController controller = SetupController();

            Mock.Get(controller.ModuleExecutor)
                .Never(CreateExecuteModuleExpression(ModuleControllerTests.TestModule3Id));

            SetupMockActivePage(controller);

            // Act
            controller.ViewPage(null, String.Empty);

            // Assert
            Mock.Get(controller.ModuleExecutor)
                .Verify(CreateExecuteModuleExpression(ModuleControllerTests.TestModule1Id));
            Mock.Get(controller.ModuleExecutor)
                .Verify(CreateExecuteModuleExpression(ModuleControllerTests.TestModule1Id));
            Mock.Get(controller.ModuleExecutor)
                .Verify(CreateExecuteModuleExpression(ModuleControllerTests.TestModule2Id));
        }

        [TestMethod]
        public void View_Action_Returns_RenderModuleResult_From_SelectedModule_If_It_Is_PageOverrideResult() {
            // Arrange
            PageController controller = SetupController();
            SetupMockActivePage(controller);

            var moduleResult = new PageOverrideResult(new Mock<ActionResult>().Object);

            ModuleRequestResult expectedResult = new ModuleRequestResult {
                ActionResult = moduleResult,
                ControllerContext = Mockery.CreateMockControllerContext()
            };
            Mock.Get(controller.ModuleExecutor)
                .Setup(CreateExecuteModuleExpression(ModuleControllerTests.TestModule1Id))
                .Returns(expectedResult);

            // Act
            ActionResult result = controller.ViewPage(1, String.Empty);

            // Assert
            ResultAssert.IsRenderModule(result, expectedResult);
        }

        [TestMethod]
        public void View_Action_Only_Executes_Selected_Module_If_It_Returns_A_PageOverride_Result() {
            // Arrange
            PageController controller = SetupController();
            SetupMockActivePage(controller);

            var moduleResult = new PageOverrideResult(new Mock<ActionResult>().Object);

            Mock.Get(controller.ModuleExecutor)
                .Never(CreateExecuteModuleExpression(ModuleControllerTests.TestModule1Id));

            Mock.Get(controller.ModuleExecutor)
                .Setup(CreateExecuteModuleExpression(ModuleControllerTests.TestModule2Id))
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

            Mock.Get(controller.ModuleExecutor)
                .Setup(CreateExecuteModuleExpression(ModuleControllerTests.TestModule1Id))
                .Returns(new ModuleRequestResult {
                    ActionResult = module1Result,
                    ControllerContext = module1Context
                });

            Mock.Get(controller.ModuleExecutor)
                .Setup(CreateExecuteModuleExpression(ModuleControllerTests.TestModule2Id))
                .Returns(new ModuleRequestResult {
                    ActionResult = module2Result,
                    ControllerContext = module2Context
                });

            // Act
            ViewResult result = controller.ViewPage(null, String.Empty) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Expected that a ViewResult would be returned");

            PageViewModel pageModel = result.ViewData.Model as PageViewModel;
            Assert.IsNotNull(pageModel, "Expected that the Model of the ViewResult would be a PageViewModel");
            Assert.AreEqual(2, pageModel.Zones.Count);

            Assert.AreSame(module1Result, pageModel["SidebarZone"].ModuleResults[0].ActionResult, "Expected that the action result from the 1st module would be the first module result");
            Assert.AreSame(module1Context, pageModel["SidebarZone"].ModuleResults[0].ControllerContext, "Expected that the controller context from the 1st module would be the first controller context");

            Assert.AreSame(module1Result, pageModel["SidebarZone"].ModuleResults[1].ActionResult, "Expected that the action result from the 2nd module would be the first module result");
            Assert.AreSame(module1Context, pageModel["SidebarZone"].ModuleResults[1].ControllerContext, "Expected that the controller context from the 2nd module would be the first controller context");
            
            // NOTE: (3rd module also uses the "first" module application)
            Assert.AreSame(module2Result, pageModel["ContentZone"].ModuleResults[0].ActionResult, "Expected that the action result from the 2nd module would be the first module result");
            Assert.AreSame(module2Context, pageModel["ContentZone"].ModuleResults[0].ControllerContext, "Expected that the controller context from the 2nd module would be the first controller context");
        }

        [TestMethod]
        public void View_Action_Renders_Default_View() {
            // Arrange
            PageController controller = SetupController();

            SetupMockActivePage(controller);

            // Act
            ViewResult result = controller.ViewPage(null, String.Empty) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Expected that a ViewResult would be returned");
            Assert.IsTrue(String.IsNullOrEmpty(result.ViewName), "Expected that the default view would be rendered");
        }

        [TestMethod]
        public void View_Action_Provides_Module_Executor_With_Module_Object_Representing_Current_Module() {
            RunSimpleModuleExecutionTest((ctl, ctx, mod, route) => Assert.AreEqual(1, mod.Id));
        }

        [TestMethod]
        public void View_Action_Provides_Empty_ModuleRoute_If_ModuleId_Does_Not_Match_ModuleId_Parameter() {
            RunSimpleModuleExecutionTest((ctl, ctx, mod, route) => Assert.AreEqual(String.Empty, route, "Expected that the routing url provided would be empty"));
        }

        [TestMethod]
        public void View_Action_Provides_ModuleRoute_From_Parameter_If_ModuleId_Matches_Parameter() {
            RunSimpleModuleExecutionTest(1,
                                         "Zoop/Zork/Zoink",
                                         (ctl, ctx, mod, route) => Assert.AreEqual("Zoop/Zork/Zoink", route));
        }

        [TestMethod]
        public void View_Action_Provides_Original_HttpContext_Base_To_Module_Executor() {
            RunSimpleModuleExecutionTest((ctl, ctx, mod, route) => Assert.AreSame(ctl.HttpContext, ctx,
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

            Mock.Get(controller.ModuleExecutor)
                .Setup(CreateExecuteModuleExpression(ModuleControllerTests.TestModule1Id))
                .Returns(new ModuleRequestResult {
                    ActionResult = module1Result,
                    ControllerContext = module1Context
                });

            Mock.Get(controller.ModuleExecutor)
                .Setup(CreateExecuteModuleExpression(ModuleControllerTests.TestModule2Id))
                .Returns(new ModuleRequestResult {
                    ActionResult = module2Result,
                    ControllerContext = module2Context
                });

            // Act
            ViewResult result = controller.ViewPage(null, String.Empty) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Expected that a ViewResult would be returned");

            PageViewModel pageModel = result.ViewData.Model as PageViewModel;
            Assert.IsNotNull(pageModel, "Expected that the Model of the ViewResult would be a PageViewModel");
            Assert.AreEqual(2, pageModel.Zones.Count);

            // Verify SidebarZone
            Assert.AreSame(module1Result, pageModel["SidebarZone"].ModuleResults[0].ActionResult, "Expected that the action result from the 1st module would be the first module result");
            Assert.AreSame(module1Context, pageModel["SidebarZone"].ModuleResults[0].ControllerContext, "Expected that the controller context from the 1st module would be the first controller context");
            Assert.AreSame(module1Result, pageModel["SidebarZone"].ModuleResults[1].ActionResult, "Expected that the action result from the 2nd module would be the first module result");
            Assert.AreSame(module1Context, pageModel["SidebarZone"].ModuleResults[1].ControllerContext, "Expected that the controller context from the 2nd module would be the first controller context");
            
            // NOTE: (3rd module also uses the "first" module application)
            Assert.AreSame(module2Result, pageModel["ContentZone"].ModuleResults[0].ActionResult, "Expected that the action result from the 2nd module would be the first module result");
            Assert.AreSame(module2Context, pageModel["ContentZone"].ModuleResults[0].ControllerContext, "Expected that the controller context from the 2nd module would be the first controller context");
        }

        private static void RunSimpleModuleExecutionTest(Action<PageController, HttpContextBase, Module, string> assert) {
            RunSimpleModuleExecutionTest(42, "Foo/Bar/Baz", assert);
        }

        private static void RunSimpleModuleExecutionTest(int moduleId, string moduleRoute, Action<PageController, HttpContextBase, Module, string> assert) {
            PageController controller = SetupController();
            PortalRequestContext requestContext = controller.HttpContext.GetPortalContext();

            Module testModule = new Module {Id = 1, ModuleApplicationId = ModuleControllerTests.TestModule1Id};
            requestContext.ActivePage = new Page {
                Modules = new List<Module> {testModule}
            };

            bool callbackHit = false;
            Mock.Get(controller.ModuleExecutor)
                .Setup(CreateExecuteModuleExpression(ModuleControllerTests.TestModule1Id))
                .Callback<HttpContextBase, Module, string>((c, m, s) => {
                    assert(controller, c, m, s);
                    callbackHit = true;
                });

            // Act
            controller.ViewPage(moduleId, moduleRoute);

            // Assert
            Assert.IsTrue(callbackHit);
        }

        private static Expression<Func<ModuleExecutionEngine, ModuleRequestResult>> CreateExecuteModuleExpression(Guid moduleApplicationId) {
            return exec => exec.ExecuteModule(It.IsAny<HttpContextBase>(),
                                              It.Is<Module>(
                                                  m => m.ModuleApplicationId == moduleApplicationId),
                                              It.IsAny<string>());
        }

        private static PageController SetupController() {
            PageController controller = new PageController {
                ModuleExecutor = new Mock<ModuleExecutionEngine>().Object
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
