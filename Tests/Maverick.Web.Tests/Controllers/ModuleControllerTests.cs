// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleControllerTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleControllerTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Mvc;
using Maverick.Data;
using Maverick.DomainServices;
using Maverick.Models;
using Maverick.Web.Controllers;
using Maverick.Web.Helpers;
using Maverick.Web.ModuleFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Web.Tests.Controllers {
    [TestClass]
    public class ModuleControllerTests : ControllerTestBase<ModuleController> {
        public static readonly Guid TestModule1Id = new Guid("{58E48CC5-A6DF-471e-AFB6-5DA107300FA4}");
        public static readonly Guid TestModule2Id = new Guid("{3BEA8263-7E60-4801-B9F9-8E294158113E}");
        public static readonly Guid TestModule3Id = new Guid("{EEA85012-1754-4326-824F-2483F10EDFEF}");

        [TestMethod]
        public void Controller_Is_Exported_To_CompositionContainer() {
            // Assert
            CompositionAssert.IsExported(typeof(ModuleController));
        }

        [TestMethod]
        public void Controller_Is_Exported_Non_Shared() {
            // Assert
            CompositionAssert.HasCreationPolicy(typeof(ModuleController), CreationPolicy.NonShared);
        }

        [TestMethod]
        public void ModuleApplications_Are_Imported_From_CompositionContainer() {
            // Assert
            CompositionAssert.IsImported<ModuleController>(p => p.ModuleApplications);
        }

        [TestMethod]
        public void ModuleService_Is_Imported_From_CompositionContainer() {
            // Assert
            CompositionAssert.IsImported<ModuleController>(p => p.ModuleRepository);
        }

        [TestMethod]
        public void Constructor_Initializes_ModuleApplications_Collection() {
            Assert.IsNotNull(CreateController().ModuleApplications);
        }

        [TestMethod]
        public void Create_Requires_SuperUser_Role() {
            ActionFilterAssert.RequiresRole(c => c.Create(String.Empty), "SuperUser");
        }

        [TestMethod]
        public void Create_Only_Accepts_POST_Verb() {
            ActionFilterAssert.AcceptsVerb(c => c.Create(String.Empty), HttpVerbs.Post);
        }

        [TestMethod]
        public void Create_Returns_NotFoundResult_If_Id_Is_Not_A_Valid_Guid() {
            // Arrange
            ModuleController controller = CreateController();
            Page activePage = new Page { Id = 42 };
            controller.HttpContext.GetPortalContext().ActivePage = activePage;
            Mock<ModuleRepository> mockModuleService = ConfigureModuleService(controller);
            mockModuleService.Never(s => s.Add(It.IsAny<Module>()));

            // Act
            ActionResult result = controller.Create("Not a valid guid!");

            // Assert
            ResultAssert.IsResourceNotFound(result);
        }

        [TestMethod]
        public void Create_Returns_NotFoundResult_If_No_ModuleApplication_With_Specified_Id() {
            // Arrange
            ModuleController controller = CreateController();
            Page activePage = new Page { Id = 42 };
            controller.HttpContext.GetPortalContext().ActivePage = activePage;
            var moduleService = ConfigureModuleService(controller);
            moduleService.Never(s => s.Add(It.IsAny<Module>()));

            // Act
            ActionResult result = controller.Create(Guid.NewGuid().ToString("N"));
            
            // Assert
            ResultAssert.IsResourceNotFound(result);
        }

        [TestMethod]
        public void Create_Returns_NotFoundResult_If_No_ActivePage() {
            // Arrange
            ModuleController controller = CreateController();
            var moduleService = ConfigureModuleService(controller);
            moduleService.Never(s => s.Add(It.IsAny<Module>()));
            Guid fakeAppId = SetupMockApplication(controller);

            // Act
            ActionResult result = controller.Create(fakeAppId.ToString("N"));
            
            // Assert
            ResultAssert.IsResourceNotFound(result);
        }

        [TestMethod]
        public void Create_Adds_New_Module_To_ModuleService_With_Specified_AppId() {
            // Arrange/Act/Assert
            RunAddModuleTest(CreateController(), (a, m) => Assert.AreEqual(a.Id, m.ModuleApplicationId));
        }

        [TestMethod]
        public void Create_Adds_New_Module_To_ModuleService_With_Name_As_Title() {
            // Arrange/Act/Assert
            RunAddModuleTest(CreateController(), (a, m) => Assert.AreEqual(a.Name, m.Title));
        }

        [TestMethod]
        public void Create_Adds_New_Module_To_Content_Zone() {
            // Arrange/Act/Assert
            RunAddModuleTest(CreateController(), (a, m) => Assert.AreEqual("Content", m.ZoneName));
        }

        [TestMethod]
        public void Create_Adds_New_Module_To_Current_Page() {
            // Arrange
            ModuleController controller = CreateController();
            controller.ControllerContext = Mockery.CreateMockControllerContext();
            Page activePage = new Page {Id = 42};
            controller.HttpContext.GetPortalContext().ActivePage = activePage;

            // Act/Assert
            RunAddModuleTest(controller, (a, m) => Assert.AreEqual(activePage.Id, m.Page.Id));
        }

        [TestMethod]
        public void Create_Redirects_To_Page_View_Action() {
            // Arrange
            ModuleController controller = CreateController();
            controller.ControllerContext = Mockery.CreateMockControllerContext();
            Page activePage = new Page { Id = 42 };
            controller.HttpContext.GetPortalContext().ActivePage = activePage;
            Guid fakeAppId = SetupMockApplication(controller);
            ConfigureModuleService(controller);

            // Act
            ActionResult result = controller.Create(fakeAppId.ToString("N"));

            // Assert
            ResultAssert.IsRedirectToAction(result, "View", "Page");
        }

        [TestMethod]
        public void Delete_Requires_SuperUser_Role() {
            ActionFilterAssert.RequiresRole(c => c.Delete(42), "SuperUser");
        }

        [TestMethod]
        public void Delete_Only_Accepts_POST_Verb() {
            ActionFilterAssert.AcceptsVerb(c => c.Delete(42), HttpVerbs.Post);
        }

        [TestMethod]
        public void Delete_Returns_NotFoundResult_If_No_Module_With_Id_Exists() {
            // Arrange
            ModuleController controller = CreateController();
            Mock<ModuleRepository> mockModuleService = ConfigureModuleService(controller);
            mockModuleService.Never(s => s.Delete(It.IsAny<Module>()));
            mockModuleService.Setup(s => s.GetAll())
                             .Returns(new List<Module> {
                                new Module {Id = 42},
                                new Module {Id = 24}
                             }.AsEntityQuery());

            // Act
            ActionResult result = controller.Delete(43);

            // Assert
            ResultAssert.IsResourceNotFound(result);
        }

        [TestMethod]
        public void Delete_Deletes_Module_With_Specified_Id() {
            // Arrange
            ModuleController controller = CreateController();
            Mock<ModuleRepository> mockModuleService = ConfigureModuleService(controller);
            mockModuleService.Setup(s => s.GetAll())
                             .Returns(new List<Module> {
                                new Module {Id = 42},
                                new Module {Id = 24}
                             }.AsEntityQuery());

            // Act
            ActionResult result = controller.Delete(24);

            // Assert
            ResultAssert.IsRedirectToAction(result, "View", "Page");
        }

        [TestMethod]
        public void Delete_Redirects_To_Page_View_Action() {
            // Arrange
            ModuleController controller = CreateController();
            Mock<ModuleRepository> mockModuleService = ConfigureModuleService(controller);
            mockModuleService.Setup(s => s.GetAll())
                             .Returns(new List<Module> {
                                new Module {Id = 42},
                                new Module {Id = 24}
                             }.AsEntityQuery());

            // Act
            controller.Delete(24);

            // Assert
            mockModuleService.Verify(s => s.Delete(It.Is<Module>(m => m.Id == 24)));
        }

        [TestMethod]
        public void Render_Returns_ResourceNotFound_Result_If_No_Module_With_Id() {
            // Arrange
            ModuleController controller = CreateController();

            // Act
            ActionResult result = controller.Render(42, String.Empty);

            // Assert
            ResultAssert.IsResourceNotFound(result);
        }

        [TestMethod]
        public void Render_Executes_Module_With_Id_If_Found_Returns_ActionResult_And_Sets_ControllerContext() {
            RunSuccessfulRenderTest(String.Empty);
        }

        [TestMethod]
        public void Render_Passes_Module_Route_To_Module() {
            RunSuccessfulRenderTest("foo/bar/baz");
        }

        private static void RunSuccessfulRenderTest(string moduleRoute) {
            // Arrange
            ModuleController controller = CreateController();
            EmptyResult expectedResult = new EmptyResult();
            ControllerContext expectedContext = Mockery.CreateMockControllerContext();
            Mock.Get(controller.ModuleExecutor)
                .Setup(e => e.ExecuteModule(It.IsAny<HttpContextBase>(),
                                            It.Is<Module>(m => m.Id == 3),
                                            moduleRoute))
                .Returns(new ModuleRequestResult {
                    ActionResult = expectedResult,
                    ControllerContext = expectedContext
                });

            // Act
            RenderModuleResult actualResult = controller.Render(3, moduleRoute) as RenderModuleResult;

            // Assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult, actualResult.ModuleRequestResult.ActionResult);
            Assert.AreSame(expectedContext, actualResult.ModuleRequestResult.ControllerContext);
        }

        private static ModuleController CreateController() {
            ModuleController controller = new ModuleController {
                ControllerContext = Mockery.CreateMockControllerContext(),
                ModuleApplications = new ModuleApplicationCollection {
                    Mockery.CreateMockApplicationExport(TestModule1Id),
                    Mockery.CreateMockApplicationExport(TestModule2Id),
                    Mockery.CreateMockApplicationExport(TestModule3Id)
                },
                ModuleExecutor = new Mock<ModuleExecutionEngine>().Object,
                ModuleRepository = new Mock<ModuleRepository>().Object
            };

            Mock.Get(controller.ModuleRepository)
                .Setup(s => s.GetAll())
                .Returns(new List<Module> {
                    new Module {Id = 1, ModuleApplicationId = TestModule1Id},
                    new Module {Id = 2, ModuleApplicationId = TestModule2Id},
                    new Module {Id = 3, ModuleApplicationId = TestModule1Id}
                }.AsEntityQuery());

            controller.ControllerContext = Mockery.CreateMockControllerContext(controller);
            return controller;
        }

        private static void RunAddModuleTest(ModuleController controller, Action<ModuleApplicationMetadata, Module> assert) {
            // Arrange
            Guid fakeAppId = SetupMockApplication(controller);

            var moduleService = ConfigureModuleService(controller);
            Module added = null;
            moduleService.Setup(s => s.Add(It.IsAny<Module>()))
                .Callback<Module>(m => added = m);

            Page activePage = new Page {Id = 42};
            controller.HttpContext.GetPortalContext().ActivePage = activePage;

            // Act
            controller.Create(fakeAppId.ToString("N"));

            // Assert
            assert(controller.ModuleApplications[fakeAppId].MetadataView, added);
        }

        private static Guid SetupMockApplication(ModuleController controller) {
            Guid fakeAppId = new Guid("083830CC-91D3-4CDF-839B-DA6E051BBC39");
            controller.ModuleApplications.Add(Mockery.CreateMockApplicationExport(fakeAppId));
            return fakeAppId;
        }

        private static Mock<ModuleRepository> ConfigureModuleService(ModuleController controller) {
            var mockModuleService = new Mock<ModuleRepository>();
            controller.ModuleRepository = mockModuleService.Object;
            return mockModuleService;
        }
    }
}
