// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PageServiceTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PageServiceTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using Maverick.Models;
using Maverick.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.DomainServices.Tests {
    [TestClass]
    public class PageRepositoryTests : RepositoryTestBase<PageRepository, Page> {
        [TestMethod]
        public void PageService_Is_Exported() {
            CompositionAssert.IsExported(typeof(PageRepository));
        }

        [TestMethod]
        public void GetPages_Returns_PageSet_From_Database() {
            GetModels_Returns_ModelSet_From_Database();
        }

        [TestMethod]
        public void AddPage_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
            AddModel_Throws_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void AddPage_Throws_InvalidOperationException_If_No_Current_DataContext() {
            AddModel_Throws_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void AddPage_Throws_ArgumentNullException_If_Page_Null() {
            AddModel_Throws_ArgumentNullException_If_Model_Null();
        }

        [TestMethod]
        public void AddPage_Outside_DataBatch_Adds_Page_To_PageSet_In_Database_And_Saves() {
            AddModel_Outside_DataBatch_Adds_Model_To_ModelSet_In_Database_And_Saves();
        }

        [TestMethod]
        public void AddPage_Within_DataBatch_Adds_Page_To_Context_Provided_By_Batch_And_Does_Not_Save() {
            AddModel_Within_DataBatch_Adds_Model_To_Context_Provided_By_Batch_And_Does_Not_Save();
        }

        [TestMethod]
        public void DeletePage_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
            DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void DeletePage_Throws_InvalidOperationException_If_No_Current_DataContext() {
            DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void DeletePage_Throws_ArgumentNullException_If_Page_Null() {
            DeleteModel_Throws_ArgumentNullException_If_Model_Null();
        }

        [TestMethod]
        public void DeletePage_Outside_DataBatch_Deletes_Page_From_PageSet_In_Database_And_Saves() {
            DeleteModel_Outside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Saves();
        }

        [TestMethod]
        public void DeletePage_Inside_DataBatch_Deletes_Page_From_PageSet_In_Database_And_Does_Not_Save() {
            DeleteModel_Inside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Does_Not_Save();
        }

        [TestMethod]
        public void UpdatePage_Overrides_Throw_InvalidOperationException_If_No_Current_DataContextManager() {
            UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContextManager();
        }

        [TestMethod]
        public void UpdatePage_Overrides_Throw_InvalidOperationException_If_No_Current_DataContext() {
            UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContext();
        }

        [TestMethod]
        public void UpdatePage_Overrides_Throw_ArgumentNullException_If_Original_Or_Modified_Page_Null() {
            UpdateModel_Overrides_Throw_ArgumentNullException_If_Original_Or_Modified_Model_Null();
        }

        [TestMethod]
        public void UpdatePage_Outside_DataBatch_With_One_Page_Attaches_It_To_DataContext_As_Modified_And_Saves() {
            UpdateModel_Outside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void UpdatePage_Inside_DataBatch_With_One_Page_Attaches_It_To_DataContext_As_Modified_And_Does_Not_Save() {
            UpdateModel_Inside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Does_Not_Save();
        }

        [TestMethod]
        public void UpdatePage_Outside_DataBatch_With_Two_Pages_Attaches_Them_To_DataContext_As_Modified_And_Saves() {
            UpdateModel_Outside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void UpdatePage_Inside_DataBatch_With_Two_Pages_Attaches_Them_To_DataContext_As_Modified_And_Does_Not_Save() {
            UpdateModel_Outside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Saves();
        }

        [TestMethod]
        public void GetLongestPrefixMatch_Returns_The_Longest_Prefix_Matching_The_Provided_Path_Segments() {
            // Arrange
            MockEntitySet<Page> pages = SetupMockPrefixes();
            string path = "/Bar/Foo/Bar/Zoop/Zork/Zoink";
            PageRepository repository = CreateService();
            repository.DataContextManager = CreateDataContextManager(pages);

            // Act
            Page page = repository.GetLongestPrefixMatch(path);
            
            // Assert
            Assert.IsNotNull(page, "Expected that a match would be returned");
            Assert.AreEqual("/Bar/Foo/Bar", page.Path, "Expected that the longest matching prefix would be returned");
        }

        [TestMethod]
        public void GetLongestPrefixMatch_Returns_RootPage_If_No_Prefix_Matches_The_Provided_Path_Segments() {
            // Arrange
            MockEntitySet<Page> pages = SetupMockPrefixes();
            PageRepository repository = CreateService();
            repository.DataContextManager = CreateDataContextManager(pages);

            // Act
            Page page = repository.GetLongestPrefixMatch("/Qux/Quirk/Quark/");
            
            // Assert
            Assert.AreEqual(0, page.Id);
        }

        [TestMethod]
        public void GetLongestPrefixMatch_Returns_RootPage_If_Prefix_Is_Single_Slash() {
            // Arrange
            MockEntitySet<Page> pages = SetupMockPrefixes();
            PageRepository repository = CreateService();
            repository.DataContextManager = CreateDataContextManager(pages);

            // Act
            Page page = repository.GetLongestPrefixMatch("/");

            // Assert
            Assert.AreEqual(0, page.Id);
        }

        private MockEntitySet<Page> SetupMockPrefixes() {
            return new MockEntitySet<Page> {
                new Page { Id = 0, Path = "/" },
                new Page { Id = 1, Path = "/Bar" },
                new Page { Id = 2, Path = "/Bar/Foo" },
                new Page { Id = 3, Path = "/Bar/Foo/Bar/Baz" },
                new Page { Id = 4, Path = "/Bar/Foo/Bar" },
                new Page { Id = 5, Path = "/Baz" },
                new Page { Id = 6, Path = "/Baz/Quz" },
            };
        }

        protected override void VerifyTestModel(Page model, int id) {
            Assert.AreEqual(id, model.Id);
            Assert.AreEqual(String.Format("Test Page #{0}", id), model.Title);
            Assert.AreEqual(String.Format("/Page/Id/{0}", id), model.Path);
        }

        protected override PageRepository CreateService() {
            return new PageRepository();
        }

        protected override Page CreateTestModel(int id) {
            return new Page {
                Id = id,
                Title = String.Format("Test Page #{0}", id),
                Path = String.Format("/Page/Id/{0}", id)
            };
        }

        protected override void AddMockModelSet(MockDataContext context, MockEntitySet<Page> entitySet) {
            context.SetMockEntitySet(entitySet);
        }
    }
}
