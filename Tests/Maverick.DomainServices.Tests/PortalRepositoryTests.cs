// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalServiceTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalServiceTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using Maverick.Models;
using Maverick.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.DomainServices.Tests {
    [TestClass]
    public class PortalRepositoryTests : RepositoryTestBase<PortalRepository, Portal> {
        [TestMethod]
        public void PortalService_Is_Exported() {
            CompositionAssert.IsExported(typeof(PortalRepository));
        }

        [TestMethod]
        public void GetPortals_Returns_PortalSet_From_Database() {
            GetModels_Returns_ModelSet_From_Database();
        }

        [TestMethod]
        public void AddPortal_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
            AddModel_Throws_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void AddPortal_Throws_InvalidOperationException_If_No_Current_DataContext() {
            AddModel_Throws_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void AddPortal_Throws_ArgumentNullException_If_Portal_Null() {
            AddModel_Throws_ArgumentNullException_If_Model_Null();
        }

        [TestMethod]
        public void AddPortal_Outside_DataBatch_Adds_Portal_To_PortalSet_In_Database_And_Saves() {
            AddModel_Outside_DataBatch_Adds_Model_To_ModelSet_In_Database_And_Saves();
        }

        [TestMethod]
        public void AddPortal_Within_DataBatch_Adds_Portal_To_Context_Provided_By_Batch_And_Does_Not_Save() {
            AddModel_Within_DataBatch_Adds_Model_To_Context_Provided_By_Batch_And_Does_Not_Save();
        }

        [TestMethod]
        public void DeletePortal_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
            DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void DeletePortal_Throws_InvalidOperationException_If_No_Current_DataContext() {
            DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void DeletePortal_Throws_ArgumentNullException_If_Portal_Null() {
            DeleteModel_Throws_ArgumentNullException_If_Model_Null();
        }

        [TestMethod]
        public void DeletePortal_Outside_DataBatch_Deletes_Portal_From_PortalSet_In_Database_And_Saves() {
            DeleteModel_Outside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Saves();
        }

        [TestMethod]
        public void DeletePortal_Inside_DataBatch_Deletes_Portal_From_PortalSet_In_Database_And_Does_Not_Save() {
            DeleteModel_Inside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Does_Not_Save();
        }

        [TestMethod]
        public void UpdatePortal_Overrides_Throw_InvalidOperationException_If_No_Current_DataContextManager() {
            UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void UpdatePortal_Overrides_Throw_InvalidOperationException_If_No_Current_DataContext() {
            UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void UpdatePortal_Overrides_Throw_ArgumentNullException_If_Original_Or_Modified_Portal_Null() {
            UpdateModel_Overrides_Throw_ArgumentNullException_If_Original_Or_Modified_Model_Null();
        }

        [TestMethod]
        public void UpdatePortal_Outside_DataBatch_With_One_Portal_Attaches_It_To_DataContext_As_Modified_And_Saves() {
            UpdateModel_Outside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void UpdatePortal_Inside_DataBatch_With_One_Portal_Attaches_It_To_DataContext_As_Modified_And_Does_Not_Save() {
            UpdateModel_Inside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Does_Not_Save();
        }

        [TestMethod]
        public void UpdatePortal_Outside_DataBatch_With_Two_Portals_Attaches_Them_To_DataContext_As_Modified_And_Saves() {
            UpdateModel_Outside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void UpdatePortal_Inside_DataBatch_With_Two_Portals_Attaches_Them_To_DataContext_As_Modified_And_Does_Not_Save() {
            UpdateModel_Outside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Saves();
        }

        protected override void VerifyTestModel(Portal model, int id) {
            Assert.AreEqual(id, model.Id);
            Assert.AreEqual(String.Format("Test Portal #{0}", id), model.Name);
        }

        protected override PortalRepository CreateService() {
            return new PortalRepository();
        }

        protected override Portal CreateTestModel(int id) {
            return new Portal {
                Id = id,
                Name = String.Format("Test Portal #{0}", id)
            };
        }

        protected override void AddMockModelSet(MockDataContext context, MockEntitySet<Portal> entitySet) {
            context.SetMockEntitySet(entitySet);
        }
    }
}
