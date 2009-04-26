// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataBatchTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataBatchTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Maverick.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;
using TargetResources = Maverick.DomainServices.Properties.Resources;

namespace Maverick.DomainServices.Tests {
    [TestClass]
    public class DataBatchTests {
        [TestCleanup]
        public void TestCleanup() {
            // Ensure the Current Domain Session is cleared for test consistency
            Assert.IsNull(DataBatch.Current, "The test failed to clean up its DataBatch, all tests must clear the current DataBatch (in a finally block) at the end to ensure consistency");
        }

        [TestMethod]
        public void Constructor_Requires_Non_Null_Context() {
            AutoTester.ArgumentNull<DataContext>(marker => new DataBatch(marker));
        }

        [TestMethod]
        public void Start_Returns_New_DataBatch_If_No_Current_DataBatch_With_Current_DataContext() {
            // Arrange
            SetupDataContextManager();

            // Act
            using (DataBatch dataBatch = DataBatch.Start()) {
                // Assert
                Assert.IsNotNull(dataBatch, "Expected that a DataBatch would be started");
                Assert.AreSame(dataBatch.Context, DataBatch.DataContextManager.GetCurrentDataContext());
            }
        }

        [TestMethod]
        public void Start_Guards_Against_No_Current_DataContext() {
            // Arrange
            SetupDataContextManager(null);

            ExceptionAssert.Guards(() => DataBatch.Start(), TargetResources.Error_NoDataContext);
        }

        [TestMethod]
        public void Start_Throws_InvalidOperationException_If_There_Is_A_Current_DomainSession() {
            // Arrange
            SetupDataContextManager();

            using (DataBatch.Start()) {

                // Act and Assert
                DataBatch u = null;
                try {
                    ExceptionAssert.Guards(() => u = DataBatch.Start(), TargetResources.Error_DataBatchAlreadyRunning);
                } finally {
                    if(u != null) {
                        u.Dispose();
                    }
                }
            }
        }

        [TestMethod]
        public void Start_Throws_InvalidOperationException_If_No_DataContextManager_Configured() {
            // Arrange
            DataBatch.DataContextManager = null;

            // Act and Assert
            ExceptionAssert.Guards(() => DataBatch.Start(), TargetResources.Error_NoDataContextManager);
            if(DataBatch.Current != null) {
                DataBatch.Current.Dispose();
            }
        }

        [TestMethod]
        public void Current_Is_Null_If_No_Current_DataBatch() {
            Assert.IsNull(DataBatch.Current, "Expected that there would be no current DataBatch by default");
        }

        [TestMethod]
        public void Current_Property_Returns_The_Current_DataBatch() {
            // Arrange
            SetupDataContextManager();
            using (DataBatch expected = DataBatch.Start()) {

                // Act
                DataBatch actual = DataBatch.Current;

                // Assert
                Assert.AreSame(expected, actual, "Expected that the current DataBatch is the one that was just started");
            }
        }

        [TestMethod]
        public void Current_Property_Is_Cleared_When_Active_DataBatch_Is_Disposed() {
            // Arrange
            SetupDataContextManager();
            using (DataBatch.Start()) {
                // Act (DataBatch will be disposed upon exiting the using block)   
                Assert.IsNotNull(DataBatch.Current, "Expected that the DataBatch would be the current one during its lifetime");
            }

            // Assert
            Assert.IsNull(DataBatch.Current, "Expected that the DataBatch would no longer be the current one when its lifetime ended");
        }

        [TestMethod]
        public void SaveChanges_Method_Saves_Changes_To_DataContext() {
            // Arrange
            SetupDataContextManager();
            
            // Act
            using (DataBatch.Start()) {
                DataBatch.Current.SaveChanges();
            }

            // Assert
            Mock.Get(DataBatch.DataContextManager.GetCurrentDataContext())
                .Verify(c => c.SaveChanges());
        }

        [TestMethod]
        public void SaveCurrentBatch_Static_Method_Does_Nothing_If_Current_Null() {
            Assert.IsNull(DataBatch.Current, "A previous test failed to clean up its DataBatch, all tests must clear the current DataBatch (in a finally block) at the end to ensure consistency");
            DataBatch.SaveCurrentBatch();
        }

        [TestMethod]
        public void SaveCurrentBatch_Calls_SaveChanges_On_Current_DataBatch() {
            // Arrange
            SetupDataContextManager();

            // Act
            using (DataBatch.Start()) {
                DataBatch.SaveCurrentBatch();
            }

            // Assert
            Mock.Get(DataBatch.DataContextManager.GetCurrentDataContext())
                .Verify(c => c.SaveChanges());
        }

        [TestMethod]
        public void SaveCurrentBatch_Clears_Current_DomainSession() {
            // Arrange
            SetupDataContextManager();

            // Act
            using (DataBatch.Start()) {
                DataBatch.SaveCurrentBatch();
            }

            // Assert
            Assert.IsNull(DataBatch.Current, "Expected that submitting the current DataBatch would clear it");
        }

        private static void SetupDataContextManager() {
            SetupDataContextManager(new Mock<DataContext>().Object);
        }

        private static void SetupDataContextManager(DataContext context) {
            var mockContextManager = new Mock<DataContextManager>();
            mockContextManager.Setup(f => f.GetCurrentDataContext()).Returns(context);
            DataBatch.DataContextManager = mockContextManager.Object;
        }
    }
}
