// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleServiceTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleServiceTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using Maverick.Models;
using Maverick.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.DomainServices.Tests {
    [TestClass]
    public class ModuleRepositoryTests : RepositoryTestBase<ModuleRepository, Module> {
        [TestMethod]
        public void ModuleService_Is_Exported() {
            CompositionAssert.IsExported(typeof(ModuleRepository));
        }

        [TestMethod]
        public void GetModules_Returns_ModuleSet_From_Database() {
            GetModels_Returns_ModelSet_From_Database();
        }

        [TestMethod]
        public void AddModule_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
            AddModel_Throws_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void AddModule_Throws_InvalidOperationException_If_No_Current_DataContext() {
            AddModel_Throws_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void AddModule_Throws_ArgumentNullException_If_Module_Null() {
            AddModel_Throws_ArgumentNullException_If_Model_Null();
        }

        [TestMethod]
        public void AddModule_Outside_DataBatch_Adds_Module_To_ModuleSet_In_Database_And_Saves() {
            AddModel_Outside_DataBatch_Adds_Model_To_ModelSet_In_Database_And_Saves();
        }

        [TestMethod]
        public void AddModule_Within_DataBatch_Adds_Module_To_Context_Provided_By_Batch_And_Does_Not_Save() {
            AddModel_Within_DataBatch_Adds_Model_To_Context_Provided_By_Batch_And_Does_Not_Save();
        }

        [TestMethod]
        public void DeleteModule_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
            DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void DeleteModule_Throws_InvalidOperationException_If_No_Current_DataContext() {
            DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void DeleteModule_Throws_ArgumentNullException_If_Module_Null() {
            DeleteModel_Throws_ArgumentNullException_If_Model_Null();
        }

        [TestMethod]
        public void DeleteModule_Outside_DataBatch_Deletes_Module_From_ModuleSet_In_Database_And_Saves() {
            DeleteModel_Outside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Saves();
        }

        [TestMethod]
        public void DeleteModule_Inside_DataBatch_Deletes_Module_From_ModuleSet_In_Database_And_Does_Not_Save() {
            DeleteModel_Inside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Does_Not_Save();
        }

        [TestMethod]
        public void UpdateModule_Overrides_Throw_InvalidOperationException_If_No_Current_DataContextManager() {
            UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void UpdateModule_Overrides_Throw_InvalidOperationException_If_No_Current_DataContext() {
            UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void UpdateModule_Overrides_Throw_ArgumentNullException_If_Original_Or_Modified_Module_Null() {
            UpdateModel_Overrides_Throw_ArgumentNullException_If_Original_Or_Modified_Model_Null();
        }

        [TestMethod]
        public void UpdateModule_Outside_DataBatch_With_One_Module_Attaches_It_To_DataContext_As_Modified_And_Saves() {
            UpdateModel_Outside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void UpdateModule_Inside_DataBatch_With_One_Module_Attaches_It_To_DataContext_As_Modified_And_Does_Not_Save() {
            UpdateModel_Inside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Does_Not_Save();
        }

        [TestMethod]
        public void UpdateModule_Outside_DataBatch_With_Two_Modules_Attaches_Them_To_DataContext_As_Modified_And_Saves() {
            UpdateModel_Outside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void UpdateModule_Inside_DataBatch_With_Two_Modules_Attaches_Them_To_DataContext_As_Modified_And_Does_Not_Save() {
            UpdateModel_Outside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Saves();
        }

        protected override void VerifyTestModel(Module model, int id) {
            Assert.AreEqual(id, model.Id);
            Assert.AreEqual(String.Format("Test Module #{0}", id), model.Title);
        }

        protected override ModuleRepository CreateService() {
            return new ModuleRepository();
        }

        protected override Module CreateTestModel(int id) {
            return new Module {
                Id = id,
                Title = String.Format("Test Module #{0}", id)
            };
        }

        protected override void AddMockModelSet(MockDataContext context, MockEntitySet<Module> entitySet) {
            context.SetMockEntitySet(entitySet);
        }
    }
}
