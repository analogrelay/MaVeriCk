// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalDatabaseTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalDatabaseTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Maverick.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maverick.Data.NHibernate.Tests.Integration {
    [TestClass]
    public class PortalDatabaseTests : DatabaseTestBase<Portal> {
        private const string TestPortalName = "Test Portal";
        private const string OtherPortalName = "Other Portal";

        // MSTest doesn't have an easy way to provide test methods on a base class, so it's copy-paste time
        [TestMethod]
        public void InsertOnSave_Creates_New_Portal_In_Database() {
            RunSuccessfulInsertTest();
        }

        [TestMethod]
        public void InsertOnSave_Sets_Id_Of_Portal_On_SaveChanges() {
            RunInsertSetsIdTest<int>();
        }

        [TestMethod]
        public void DeleteOnSave_With_Attached_Portal_Removes_Portal_From_Database() {
            RunDeleteAttachedTest();
        }

        [TestMethod]
        public void DeleteOnSave_With_Detached_Portal_Removes_Portal_From_Database() {
            RunDeleteDetachedTest();
        }

        [TestMethod]
        public void DeleteOnSave_With_New_Detached_Portal_With_Matching_Id_Removes_Portal_From_Database() {
            RunDeleteNewDetachedTest();
        }

        [TestMethod]
        public void UpdateOnSave_With_Modified_Updates_Database() {
            RunUpdateModifiedTest();
        }

        [TestMethod]
        public void UpdateOnSave_With_Original_And_Modified_Updates_Database() {
            RunUpdateOriginalAndModifiedTest();
        }

        [TestMethod]
        public void Detach_Disassociates_Portal_From_Session() {
            RunDetachTest();
        }

        [TestMethod]
        public void Attach_Reassociates_Portal_With_Session() {
            RunAttachTest();
        }

        protected override object GetId(Portal entity) {
            return entity.Id;
        }

        protected override Portal CreateNewDetachedDeleteEntity(object id) {
            return new Portal { Id = (int)id, Name = OtherPortalName };
        }

        protected override Portal CreateEntity() {
            return new Portal { Name = TestPortalName };
        }

        protected override void ModifyEntity(Portal entity) {
            entity.Name = OtherPortalName;
        }

        protected override Portal CopyEntity(Portal entity) {
            return new Portal {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        protected override void VerifyInserted(Portal inserted) {
            Assert.AreEqual(TestPortalName, inserted.Name);
        }

        protected override void VerifyUpdated(Portal updated) {
            Assert.AreEqual(OtherPortalName, updated.Name);
        }
    }
}
