// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextBuilderTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextBuilderTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.Composition;
using Maverick.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;
using TargetResources = Maverick.Data.Properties.Resources;

namespace Maverick.Data.Tests {
    [TestClass]
    public class DataContextBuilderTests {
        private const string TestFactory = "FooBar";
        private const string ExpectedFactory = "Expected";

        [TestMethod]
        public void ContextFactories_Requires_Non_Null_Value() {
            DataContextBuilder contextBuilder = new DataContextBuilder();
            AutoTester.SetNull(() => contextBuilder.ContextFactories);
        }

        [TestMethod]
        public void ContextFactories_Property_Is_ReadWrite() {
            DataContextBuilder contextBuilder = new DataContextBuilder();
            AutoTester.TestReadWriteProperty(() => contextBuilder.ContextFactories, new ComponentCollection<DataContextFactory>());
        }

        [TestMethod]
        public void DefaultFactoryName_Requires_Non_Null_Value() {
            DataContextBuilder contextBuilder = new DataContextBuilder();
            AutoTester.SetNull(() => contextBuilder.DefaultFactoryName);
        }

        [TestMethod]
        public void DefaultFactoryName_Property_Is_ReadWrite() {
            DataContextBuilder contextBuilder = new DataContextBuilder();
            AutoTester.TestReadWriteProperty(() => contextBuilder.DefaultFactoryName, "FooBar");
        }

        [TestMethod]
        public void CreateDataContext_Guards_Against_Null_ContextFactory() {
            DataContextBuilder contextBuilder = new DataContextBuilder();
            contextBuilder.ContextFactories.Add(
                new Export<DataContextFactory, ComponentMetadata>(
                    new Dictionary<string, object> {
                        {"Name", TestFactory}
                    }, () => null));

            contextBuilder.DefaultFactoryName = TestFactory;
            ExceptionAssert.Guards(() => contextBuilder.CreateDataContext(),
                                   TargetResources.Error_DataContextFactoryWasNull,
                                   TestFactory);
        }

        [TestMethod]
        public void DataContextBuilder_Is_Exported_To_CompositionContainer() {
            CompositionAssert.IsExported(typeof(DataContextBuilder));
        }

        [TestMethod]
        public void ContextFactories_Are_Imported_From_CompositionContainer() {
            CompositionAssert.IsImportedCollection<DataContextBuilder>(b => b.ContextFactories, typeof(DataContextFactory));
        }

        [TestMethod]
        public void DefaultFactoryName_Is_Imported_From_CompositionContainer() {
            CompositionAssert.IsImported<DataContextBuilder>(b => b.DefaultFactoryName,
                                                             DataContractNames.DataContextFactoryName);
        }

        [TestMethod]
        public void CreateDataContext_Creates_Context_Using_Factory_With_Name_Matching_DefaultFactoryName() {
            // Arrange
            DataContextBuilder contextBuilder = new DataContextBuilder();

            var expectedSession = new Mock<DataContext>();
            var expectedFactory = new Mock<DataContextFactory>();
            expectedFactory.Setup(f => f.CreateDataContext()).Returns(expectedSession.Object);

            var otherFactory = new Mock<DataContextFactory>();
            otherFactory.Setup(f => f.CreateDataContext()).Returns(() => null);

            contextBuilder.ContextFactories.Add(TestFactory, () => otherFactory.Object);
            contextBuilder.ContextFactories.Add(ExpectedFactory, () => expectedFactory.Object);
            contextBuilder.DefaultFactoryName = ExpectedFactory;

            // Act
            DataContext actualContext = contextBuilder.CreateDataContext();

            // Assert
            Assert.AreSame(expectedSession.Object, actualContext, "Expected that the session returned would be constructed by the default session factory");
        }

        [TestMethod]
        public void CreateDataContext_Guards_Against_No_Factory_Matching_DefaultFactoryName_Being_Registered() {
            // Arrange
            DataContextBuilder contextBuilder = new DataContextBuilder();
            contextBuilder.DefaultFactoryName = TestFactory;

            // Act and Assert
            ExceptionAssert.Guards(() => contextBuilder.CreateDataContext(),
                                   TargetResources.Error_DataContextFactoryNotFound,
                                   TestFactory,
                                   DataContractNames.DataContextFactoryName);
        }
    }
}
