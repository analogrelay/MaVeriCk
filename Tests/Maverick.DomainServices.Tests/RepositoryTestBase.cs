// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SimpleDomainServiceTestBase.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the SimpleDomainServiceTestBase type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Maverick.Data;
using Maverick.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;
using TargetResources = Maverick.DomainServices.Properties.Resources;

namespace Maverick.DomainServices.Tests {
    public abstract class RepositoryTestBase<TService, TModel> where TService : RepositoryBase<TModel> where TModel : class {
        protected void GetModels_Returns_ModelSet_From_Database() {
            // Arrange
            TService service = CreateService();
            service.DataContextManager = CreateDataContextManager(new MockEntitySet<TModel> {
                CreateTestModels(2)
            });
            
            // Act
            IList<TModel> models = service.GetAll().ToList();
            
            // Assert
            Assert.AreEqual(2, models.Count, "Expected that 2 {0} objects would be returned", typeof(TModel).Name);
            VerifyTestModels(models, 2);
        }

        protected void AddModel_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
           RunNoDataContextManagerTest(service => service.Add(CreateTestModel(42)));
        }

        protected void AddModel_Throws_InvalidOperationException_If_No_Current_DataContext() {
            RunNoDataContextTest(service => service.Add(CreateTestModel(42)));
        }

        protected void AddModel_Throws_ArgumentNullException_If_Model_Null() {
            RunArgumentTest(service => marker => service.Add(marker));
        }

        protected void AddModel_Outside_DataBatch_Adds_Model_To_ModelSet_In_Database_And_Saves() {
            // Arrange
            MockEntitySet<TModel> entitySet = new MockEntitySet<TModel>();
            
            RunUnbatchedTest(entitySet, service => {
                // Act
                service.Add(CreateTestModel(42));

                // Assert
                Assert.AreEqual(1, entitySet.Inserted.Count, "Expected that the {0} object would be queued for insertion", typeof(TModel).Name);
                VerifyTestModel(entitySet.Inserted[0], 42);
            });
        }

        protected void AddModel_Within_DataBatch_Adds_Model_To_Context_Provided_By_Batch_And_Does_Not_Save() {
            // Arrange
            MockEntitySet<TModel> entitySet = new MockEntitySet<TModel>();
            
            // Act
            RunBatchedTest(entitySet,
                           service => {
                               service.Add(CreateTestModel(42));

                               // Assert
                               Assert.AreEqual(1,
                                               entitySet.Inserted.Count,
                                               "Expected that the {0} object would be queued for insertion",
                                               typeof(TModel).Name);
                               VerifyTestModel(entitySet.Inserted[0], 42);
                           });
        }

        protected void DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContextManager() {
            RunNoDataContextManagerTest(service => service.Delete(CreateTestModel(42)));
        }

        protected void DeleteModel_Throws_InvalidOperationException_If_No_Current_DataContext() {
            RunNoDataContextTest(service => service.Delete(CreateTestModel(42)));
        }

        protected void DeleteModel_Throws_ArgumentNullException_If_Model_Null() {
            RunArgumentTest(service => marker => service.Delete(marker));
        }

        protected void DeleteModel_Outside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Saves() {
            // Arrange
            MockEntitySet<TModel> entitySet = new MockEntitySet<TModel>();
            
            // Act
            RunUnbatchedTest(entitySet,
                             service => {
                                 service.Delete(CreateTestModel(42));

                                 // Assert
                                 Assert.AreEqual(1,
                                                 entitySet.Deleted.Count,
                                                 "Expected that the {0} object would be queued for deletion",
                                                 typeof(TModel).Name);
                                 VerifyTestModel(entitySet.Deleted[0], 42);
                             });
        }

        protected void DeleteModel_Inside_DataBatch_Deletes_Model_From_ModelSet_In_Database_And_Does_Not_Save() {
            // Arrange
            MockEntitySet<TModel> entitySet = new MockEntitySet<TModel>();
            
            // Act
            RunBatchedTest(entitySet,
                           service => {
                               service.Delete(CreateTestModel(42));

                               // Assert
                               Assert.AreEqual(1,
                                               entitySet.Deleted.Count,
                                               "Expected that the {0} object would be queued for deletion",
                                               typeof(TModel).Name);
                               VerifyTestModel(entitySet.Deleted[0], 42);
                           });
        }

        protected void UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContextManager() {
            RunNoDataContextManagerTest(service => service.Update(CreateTestModel(42)));
            RunNoDataContextManagerTest(service => service.Update(CreateTestModel(42), CreateTestModel(24)));
        }

        protected void UpdateModel_Overrides_Throw_InvalidOperationException_If_No_Current_DataContext() {
            RunNoDataContextTest(service => service.Update(CreateTestModel(42)));
            RunNoDataContextTest(service => service.Update(CreateTestModel(42), CreateTestModel(24)));
        }

        protected void UpdateModel_Overrides_Throw_ArgumentNullException_If_Original_Or_Modified_Model_Null() {
            RunArgumentTest(service => marker => service.Delete(marker));
            RunArgumentTest(service => marker => service.Update(marker));
            RunArgumentTest(service => marker => service.Update(marker, CreateTestModel(42)));
            RunArgumentTest(service => marker => service.Update(CreateTestModel(42), marker));
        }

        protected void UpdateModel_Outside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Saves() {
            // Arrange
            MockEntitySet<TModel> entitySet = new MockEntitySet<TModel>();
            
            // Act
            RunUnbatchedTest(entitySet,
                             service => {
                                 service.Update(CreateTestModel(42));

                                 // Assert
                                 Assert.AreEqual(1,
                                                 entitySet.Updated.Count,
                                                 "Expected that the {0} object would be queued for update",
                                                 typeof(TModel).Name);
                                 VerifyTestModel(entitySet.Updated[0].Original, 42);
                                 VerifyTestModel(entitySet.Updated[0].Modified, 42);
                             });
        }

        protected void UpdateModel_Inside_DataBatch_With_One_Model_Attaches_It_To_DataContext_As_Modified_And_Does_Not_Save() {
            // Arrange
            MockEntitySet<TModel> entitySet = new MockEntitySet<TModel>();
            
            // Act
            RunBatchedTest(entitySet,
                           service => {
                               service.Update(CreateTestModel(42));

                               // Assert
                               Assert.AreEqual(1,
                                               entitySet.Updated.Count,
                                               "Expected that the {0} object would be queued for update",
                                               typeof(TModel).Name);
                               VerifyTestModel(entitySet.Updated[0].Original, 42);
                               VerifyTestModel(entitySet.Updated[0].Modified, 42);
                           });
        }

        protected void UpdateModel_Outside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Saves() {
            // Arrange
            MockEntitySet<TModel> entitySet = new MockEntitySet<TModel>();
            
            // Act
            RunUnbatchedTest(entitySet,
                             service => {
                                 service.Update(CreateTestModel(42), CreateTestModel(24));

                                 // Assert
                                 Assert.AreEqual(1,
                                                 entitySet.Updated.Count,
                                                 "Expected that the {0} object would be queued for update",
                                                 typeof(TModel).Name);
                                 VerifyTestModel(entitySet.Updated[0].Original, 42);
                                 VerifyTestModel(entitySet.Updated[0].Modified, 24);
                             });
        }

        protected void UpdateModel_Inside_DataBatch_With_Two_Models_Attaches_Them_To_DataContext_As_Modified_And_Does_Not_Save() {
            // Arrange
            MockEntitySet<TModel> entitySet = new MockEntitySet<TModel>();
            
            // Act
            RunBatchedTest(entitySet,
                           service => {
                               service.Update(CreateTestModel(42), CreateTestModel(24));

                               // Assert
                               Assert.AreEqual(1,
                                               entitySet.Updated.Count,
                                               "Expected that the {0} object would be queued for update",
                                               typeof(TModel).Name);
                               VerifyTestModel(entitySet.Updated[0].Original, 42);
                               VerifyTestModel(entitySet.Updated[0].Modified, 24);
                           });
        }

        protected void RunArgumentTest(Func<TService, Expression<Action<TModel>>> callToTest) {
            // Arrange
            TService service = CreateService();

            // Act and Assert
            AutoTester.ArgumentNull(callToTest(service));
        }

        protected void RunNoDataContextTest(Action<TService> callToTest) {
            // Arrange
            TService service = CreateService();
            var mockContextManager = new Mock<DataContextManager>();
            service.DataContextManager = mockContextManager.Object;
            
            // Act and Assert
            ExceptionAssert.Throws<InvalidOperationException>(() => callToTest(service), TargetResources.Error_NoDataContext);
        }

        protected void RunNoDataContextManagerTest(Action<TService> callToTest) {
            DataBatch.DataContextManager = null;
            
            // Arrange
            TService service = CreateService();

            // Act and Assert
            ExceptionAssert.Throws<InvalidOperationException>(() => callToTest(service), TargetResources.Error_NoDataContextManager);
        }

        protected DataContextManager CreateDataContextManager(MockEntitySet<TModel> entitySet) {
            MockDataContext context = new MockDataContext();
            AddMockModelSet(context, entitySet);

            var mockContextManager = new Mock<DataContextManager>();
            mockContextManager.Setup(c => c.GetCurrentDataContext())
                              .Returns(context);

            return mockContextManager.Object;
        }

        protected void VerifyTestModels(IEnumerable<TModel> portals, int count) {
            VerifyTestModels(portals, 0, count);
        }

        protected void VerifyTestModels(IEnumerable<TModel> portals, int startId, int count) {
            IEnumerator<TModel> portalEnum = portals.GetEnumerator();
            for (int i = startId; i < startId + count; i++) {
                portalEnum.MoveNext();
                VerifyTestModel(portalEnum.Current, i);
            }
        }

        protected IEnumerable<TModel> CreateTestModels(int count) {
            return CreateTestModels(0, count);
        }

        protected IEnumerable<TModel> CreateTestModels(int startId, int count) {
            return from id in Enumerable.Range(startId, count)
                   select CreateTestModel(id);
        }


        protected abstract TService CreateService();
        protected abstract TModel CreateTestModel(int id);
        protected abstract void VerifyTestModel(TModel model, int id);
        protected abstract void AddMockModelSet(MockDataContext context, MockEntitySet<TModel> entitySet);

        private TService SetupServiceForBatchedTest(MockEntitySet<TModel> entitySet) {
            DataBatch.DataContextManager = CreateDataContextManager(entitySet);
            return CreateService();
        }

        private TService SetupServiceForUnbatchedTest(MockEntitySet<TModel> entitySet) {
            TService service = CreateService();
            service.DataContextManager = CreateDataContextManager(entitySet);
            return service;
        }

        private void RunBatchedTest(MockEntitySet<TModel> entitySet, Action<TService> test) {
            // Arrange
            TService service = SetupServiceForBatchedTest(entitySet);

            // Act/Assert
            using (DataBatch.Start()) {
                test(service);
                Assert.IsFalse(DataBatch.Current.Context.AssertCast<MockDataContext>().ChangesSaved);
            }
        }

        private void RunUnbatchedTest(MockEntitySet<TModel> entitySet, Action<TService> test) {
            // Arrange
            TService service = SetupServiceForUnbatchedTest(entitySet);

            // Act/Assert
            test(service);

            // Assert some more
            Assert.IsTrue(service.DataContextManager.GetCurrentDataContext().AssertCast<MockDataContext>().ChangesSaved);
        }
    }
}
