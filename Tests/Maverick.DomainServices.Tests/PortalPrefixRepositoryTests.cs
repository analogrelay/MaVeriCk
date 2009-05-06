// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalPrefixServiceTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalPrefixServiceTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using Maverick.Models;
using Maverick.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.DomainServices.Tests {
    [TestClass]
    public class PortalPrefixRepositoryTests : RepositoryTestBase<PortalPrefixRepository, PortalPrefix> {
        [TestMethod]
        public void PortalPrefixService_Is_Exported() {
            CompositionAssert.IsExported(typeof(PortalPrefixRepository));
        }

        [TestMethod]
        public void GetPortalPrefixes_Returns_PortalPrefixSet_From_Database() {
            GetModels_Returns_ModelSet_From_Database();
        }

        [TestMethod]
        public void AddPortalPrefix_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
            AddModel_Throws_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void AddPortalPrefix_Throws_InvalidOperationException_If_No_Current_DataContext() {
            AddModel_Throws_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void AddPortalPrefix_Throws_ArgumentNullException_If_PortalPrefix_Null() {
            AddModel_Throws_ArgumentNullException_If_Model_Null();
        }

        [TestMethod]
        public void AddPortalPrefix_Outside_DataBatch_Adds_PortalPrefix_To_PortalPrefixSet_In_Database_And_Saves() {
            AddModel_Outside_DataBatch_Adds_Model_To_ModelSet_In_Database_And_Saves();
        }

        [TestMethod]
        public void AddPortalPrefix_Within_DataBatch_Adds_PortalPrefix_To_Context_Provided_By_Batch_And_Does_Not_Save() {
            AddModel_Within_DataBatch_Adds_Model_To_Context_Provided_By_Batch_And_Does_Not_Save();
        }

        [TestMethod]
        public void DeletePortalPrefix_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
            DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void DeletePortalPrefix_Throws_InvalidOperationException_If_No_Current_DataContext() {
            DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void DeletePortalPrefix_Throws_ArgumentNullException_If_PortalPrefix_Null() {
            DeleteModel_Throws_ArgumentNullException_If_Model_Null();
        }

        [TestMethod]
        public void DeletePortalPrefix_Outside_DataBatch_Deletes_PortalPrefix_From_PortalPrefixSet_In_Database_And_Saves() {
            DeleteModel_Outside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Saves();
        }

        [TestMethod]
        public void DeletePortalPrefix_Inside_DataBatch_Deletes_PortalPrefix_From_PortalPrefixSet_In_Database_And_Does_Not_Save() {
            DeleteModel_Inside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Does_Not_Save();
        }

        [TestMethod]
        public void UpdatePortalPrefix_Overrides_Throw_InvalidOperationException_If_No_Current_DataContextManager() {
            UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void UpdatePortalPrefix_Overrides_Throw_InvalidOperationException_If_No_Current_DataContext() {
            UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void UpdatePortalPrefix_Overrides_Throw_ArgumentNullException_If_Original_Or_Modified_PortalPrefix_Null() {
            UpdateModel_Overrides_Throw_ArgumentNullException_If_Original_Or_Modified_Model_Null();
        }

        [TestMethod]
        public void UpdatePortalPrefix_Outside_DataBatch_With_One_PortalPrefix_Attaches_It_To_DataContext_As_Modified_And_Saves() {
            UpdateModel_Outside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void UpdatePortalPrefix_Inside_DataBatch_With_One_PortalPrefix_Attaches_It_To_DataContext_As_Modified_And_Does_Not_Save() {
            UpdateModel_Inside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Does_Not_Save();
        }

        [TestMethod]
        public void UpdatePortalPrefix_Outside_DataBatch_With_Two_PortalPrefixs_Attaches_Them_To_DataContext_As_Modified_And_Saves() {
            UpdateModel_Outside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void UpdatePortalPrefix_Inside_DataBatch_With_Two_PortalPrefixs_Attaches_Them_To_DataContext_As_Modified_And_Does_Not_Save() {
            UpdateModel_Outside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void GetLongestPrefixMatch_Returns_The_Longest_Prefix_Matching_The_Provided_Url_Segments() {
            // Arrange
            MockEntitySet<PortalPrefix> portalPrefixes = SetupMockPrefixes();
            Uri testUrl = new Uri("http://localhost:80/Foo/Bar/Quz?id=432&5434#Bar");
            PortalPrefixRepository repository = CreateService();
            repository.DataContextManager = CreateDataContextManager(portalPrefixes);
            
            // Act
            PortalPrefix prefix = repository.GetLongestPrefixMatch(testUrl);

            // Assert
            Assert.IsNotNull(prefix, "Expected that a match would be returned");
            Assert.AreEqual("localhost/Foo/Bar/", prefix.Prefix, "Expected that the longest matching prefix would be returned");
        }

        [TestMethod]
        public void GetLongestPrefixMatch_Ignores_Port_If_No_Exact_Match() {
            // Arrange
            MockEntitySet<PortalPrefix> portalPrefixes = SetupMockPrefixes();
            Uri testUrl = new Uri("http://localhost:4949/");
            PortalPrefixRepository repository = CreateService();
            repository.DataContextManager = CreateDataContextManager(portalPrefixes);

            // Act
            PortalPrefix prefix = repository.GetLongestPrefixMatch(testUrl);
            
            // Assert
            Assert.IsNotNull(prefix, "Expected that a match would be returned");
            Assert.AreEqual("localhost/", prefix.Prefix, "Expected that the longest matching prefix would be returned");
        }

        [TestMethod]
        public void GetLongestPrefixMatch_Returns_Null_If_No_Prefix_Matches_The_Provided_Url_Segments() {
            // Arrange
            MockEntitySet<PortalPrefix> portalPrefixes = SetupMockPrefixes();
            Uri testUrl = new Uri("http://www.foo.example/This/Url/Does/Not/Exist");
            PortalPrefixRepository repository = CreateService();
            repository.DataContextManager = CreateDataContextManager(portalPrefixes);

            // Act
            PortalPrefix prefix = repository.GetLongestPrefixMatch(testUrl);

            // Assert
            Assert.IsNull(prefix, "Expected that no match would be returned");
        }

        [TestMethod]
        public void GetLongestPrefixMatch_Asks_DataSession_To_Include_Portals_With_PortalPrefixes() {
            // Arrange
            MockEntitySet<PortalPrefix> portalPrefixes = SetupMockPrefixes();
            Uri testUrl = new Uri("http://www.foo.example/This/Url/Does/Not/Exist");
            PortalPrefixRepository repository = CreateService();
            repository.DataContextManager = CreateDataContextManager(portalPrefixes);

            // Act
            // Don't care about the output
            repository.GetLongestPrefixMatch(testUrl);

            // Assert
            Assert.AreEqual(1, portalPrefixes.Included.Count, "Expected that the 'Portal' property would configured for eager loading");
            Assert.AreEqual("Portal", portalPrefixes.Included[0], "Expected that the 'Portal' property would configured for eager loading");
        }

        [TestMethod]
        public void AddPortalPrefix_Throws_InvalidModelStateException_If_IsNew_False_On_Incoming_PortalPrefix() {
            AddModel_Throws_InvalidModelStateException_If_IsNew_False_On_Incoming_Model();
        }

        [TestMethod]
        public void DeletePortalPrefix_Throws_InvalidModelStateException_If_IsNew_True_On_Incoming_PortalPrefix() {
            DeleteModel_Throws_InvalidModelStateException_If_IsNew_True_On_Incoming_Model();
        }

        [TestMethod]
        public void UpdatePortalPrefix_Overrides_Throw_InvalidModelStateException_If_IsNew_True_On_Incoming_PortalPrefixes() {
            UpdateModel_Overrides_Throw_InvalidModelStateException_If_IsNew_True_On_Incoming_Models();
        }

        private static MockEntitySet<PortalPrefix> SetupMockPrefixes() {
            return new MockEntitySet<PortalPrefix> {
                new PortalPrefix() { Id = 1, Prefix = "localhost/" }, new PortalPrefix() { Id = 2, Prefix = "localhost/Foo/" }, new PortalPrefix() { Id = 3, Prefix = "localhost/Foo/Bar/Baz/" }, new PortalPrefix() { Id = 4, Prefix = "localhost/Foo/Bar/" }, new PortalPrefix() { Id = 5, Prefix = "localhost:8080/" }, new PortalPrefix() { Id = 6, Prefix = "localhost:8080/Quz/" },
            };
        }

        protected override void VerifyTestModel(PortalPrefix model, int? id) {
            Assert.AreEqual(id, model.Id);
            Assert.AreEqual(String.Format("Test Portal Prefix #{0}", id), model.Prefix);
        }

        protected override PortalPrefixRepository CreateService() {
            return new PortalPrefixRepository();
        }

        protected override PortalPrefix CreateTestModel(int? id) {
            return new PortalPrefix {
                Id = id,
                Prefix = String.Format("Test Portal Prefix #{0}", id)
            };
        }

        protected override void AddMockModelSet(MockDataContext context, MockEntitySet<PortalPrefix> entitySet) {
            context.SetMockEntitySet(entitySet);
        }
    }
}
