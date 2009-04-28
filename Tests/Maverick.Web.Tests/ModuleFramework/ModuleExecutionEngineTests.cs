// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleExecutionEngineTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleExecutionEngineTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Maverick.Models;
using Maverick.Web.ModuleFramework;
using Maverick.Web.Tests.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Web.Tests.ModuleFramework {
    [TestClass]
    public class ModuleExecutionEngineTests {
        [TestMethod]
        public void ExecuteModule_Requires_NonNull_HttpContextBase() {
            AutoTester.ArgumentNull<HttpContextBase>(
                marker => new ModuleExecutionEngine().ExecuteModule(marker, new Module(), "Foo"));
        }

        [TestMethod]
        public void ExecuteModule_Requires_NonNull_Module() {
            AutoTester.ArgumentNull<Module>(
                marker => new ModuleExecutionEngine().ExecuteModule(Mockery.CreateMockHttpContext(), marker, "Foo"));
        }

        [TestMethod]
        public void ExecuteModule_Requires_NonNull_ModuleRoute() {
            // Empty route is OK
            AutoTester.ArgumentNull<string>(
                marker => new ModuleExecutionEngine().ExecuteModule(Mockery.CreateMockHttpContext(), new Module(), marker));
        }

        
        [TestMethod]
        public void ExecuteModule_Returns_Null_If_Application_For_Provided_Module_Does_Not_Exist() {
            // Arrange
            ModuleExecutionEngine engine = CreateExecutionEngine();

            // Act
            ModuleRequestResult result = engine.ExecuteModule(Mockery.CreateMockHttpContext(),
                                                              new Module() {ModuleApplicationId = ModuleControllerTests.TestModule4Id},
                                                              String.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ExecuteModule_Executes_ModuleApplication_For_Module_If_Exists() {
            // Arrange
            ModuleExecutionEngine engine = CreateExecutionEngine();

            // Act
            engine.ExecuteModule(Mockery.CreateMockHttpContext(),
                                 new Module() { ModuleApplicationId = ModuleControllerTests.TestModule2Id },
                                 String.Empty);

            // Assert
            Mock.Get(engine.ModuleApplications[ModuleControllerTests.TestModule2Id].GetExportedObject())
                .Verify(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()));
        }

        [TestMethod]
        public void ExecuteModule_Returns_Result_Of_Executing_ModuleApplication() {
            // Arrange
            ModuleExecutionEngine engine = CreateExecutionEngine();
            ModuleRequestResult expected = new ModuleRequestResult();
            Mock.Get(engine.ModuleApplications[ModuleControllerTests.TestModule2Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Returns(expected);
            // Act
            ModuleRequestResult actual = engine.ExecuteModule(Mockery.CreateMockHttpContext(),
                                                              new Module() { ModuleApplicationId = ModuleControllerTests.TestModule2Id },
                                                              String.Empty);

            // Assert
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void ExecuteModule_Provides_Context_Data_To_Executed_ModuleApplication() {
            // Arrange
            ModuleExecutionEngine engine = CreateExecutionEngine();
            HttpContextBase httpContext = Mockery.CreateMockHttpContext();
            Module module = new Module() { ModuleApplicationId = ModuleControllerTests.TestModule2Id };
            const string route = "Foo/Bar/Baz";
            
            ModuleRequestContext providedContext = null;
            Mock.Get(engine.ModuleApplications[ModuleControllerTests.TestModule2Id].GetExportedObject())
                .Setup(app => app.ExecuteRequest(It.IsAny<ModuleRequestContext>()))
                .Callback<ModuleRequestContext>(c => providedContext = c);
            
            // Act
            engine.ExecuteModule(httpContext, module, route);

            // Assert
            Assert.AreSame(engine.ModuleApplications[ModuleControllerTests.TestModule2Id].GetExportedObject(), providedContext.Application);
            Assert.AreSame(module, providedContext.Module);
            Assert.AreSame(httpContext, providedContext.HttpContext);
            Assert.AreEqual(route, providedContext.ModuleRoutingUrl);
        }

        private static ModuleExecutionEngine CreateExecutionEngine() {
            return new ModuleExecutionEngine() {
                ModuleApplications = new ModuleApplicationCollection {
                    Mockery.CreateMockApplicationExport(ModuleControllerTests.TestModule1Id),
                    Mockery.CreateMockApplicationExport(ModuleControllerTests.TestModule2Id),
                    Mockery.CreateMockApplicationExport(ModuleControllerTests.TestModule3Id)
                }
            };
        }
    }
}
