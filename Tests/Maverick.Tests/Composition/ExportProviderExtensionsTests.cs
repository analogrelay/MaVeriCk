// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ExportProviderExtensionsTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ExportProviderExtensionsTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Maverick.Composition;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using System.ComponentModel.Composition.Hosting;
using Moq;
using Maverick.ComponentModel;

namespace Maverick.Tests.Composition {
    [TestClass]
    public class ExportProviderExtensionsTests {
        public abstract class TestExportProvider : ExportProvider {
            protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition) {
                return InternalGetExportsCore(definition);
            }

            protected internal abstract IEnumerable<Export> InternalGetExportsCore(ImportDefinition definition);
        }
        public class TestMetadata : ComponentMetadata {
            public TestMetadata(IDictionary<string, object> metadata) : base(metadata) {}
            public string TestData {
                get { return GetMetadataProperty<string>("TestData"); }
            }
        }

        [TestMethod]
        public void GetComponentOrDefaultByName_Requires_NonNullOrEmpty_Name() {
            ExportProvider provider = new Mock<TestExportProvider>().Object;
            AutoTester.StringArgumentNullOrEmpty(m => provider.GetComponentOrDefaultByName<string>(m));
            AutoTester.StringArgumentNullOrEmpty(m => provider.GetComponentOrDefaultByName<string>("Foo",m));
        }

        [TestMethod]
        public void ExtensionMethods_Require_NonNullOrEmpty_ContractName() {
            ExportProvider provider = new Mock<TestExportProvider>().Object;
            AutoTester.StringArgumentNullOrEmpty(m => provider.GetComponentOrDefaultByName<string>(m, "Foo"));
            AutoTester.StringArgumentNullOrEmpty(m => provider.GetExportedObjectOrDefault<string, ComponentMetadata>(m, e => false));
            AutoTester.StringArgumentNullOrEmpty(m => provider.GetExport<string, ComponentMetadata>(m, e => false));
        }

        [TestMethod]
        public void ExtensionMethods_Require_NonNull_Criteria() {
            ExportProvider provider = new Mock<TestExportProvider>().Object;
            AutoTester.ArgumentNull<Predicate<Export<string, ComponentMetadata>>>(m => provider.GetExportedObjectOrDefault(m));
            AutoTester.ArgumentNull<Predicate<Export<string, ComponentMetadata>>>(m => provider.GetExportedObjectOrDefault("Foo", m));
            AutoTester.ArgumentNull<Predicate<Export<string, ComponentMetadata>>>(m => provider.GetExport(m));
            AutoTester.ArgumentNull<Predicate<Export<string, ComponentMetadata>>>(m => provider.GetExport("Foo", m));
        }

        [TestMethod]
        public void GetComponentOrDefaultByName_Retrieves_Component_With_Specified_ContractType_And_Name() {
            // Arrange
            var mockProvider = new Mock<TestExportProvider>() {CallBase = true};
            mockProvider.Setup(e => e.InternalGetExportsCore(It.IsAny<ImportDefinition>()))
                        .Returns(CreateMockExports());
                        
            // Act
            string str = mockProvider.Object.GetComponentOrDefaultByName<string>("Name6");

            // Assert
            Assert.AreEqual("TestComponent6", str);
        }

        [TestMethod]
        public void GetComponentOrDefaultByName_Retrieves_Component_With_Specified_ContractName_And_Name() {
            // Arrange
            var mockProvider = new Mock<TestExportProvider>() { CallBase = true };
            mockProvider.Setup(e => e.InternalGetExportsCore(It.IsAny<ImportDefinition>()))
                        .Returns(CreateMockExports());

            // Act
            string str = mockProvider.Object.GetComponentOrDefaultByName<string>("TestContract2", "Name3");

            // Assert
            Assert.AreEqual("TestComponent3", str);
        }

        [TestMethod]
        public void GetComponentOrDefaultByName_Returns_Default_If_GetExports_Returns_Null() {
            // Arrange
            var mockProvider = new Mock<TestExportProvider>() { CallBase = true };
            mockProvider.Setup(e => e.InternalGetExportsCore(It.IsAny<ImportDefinition>()))
                        .Returns(() => null);

            // Act
            object obj = mockProvider.Object.GetComponentOrDefaultByName<object>("TestContract2", "Name3");

            // Assert
            Assert.IsNull(obj);
        }

        private static IEnumerable<Export> CreateMockExports() {
            return new[] {
                new Export("TestContract1", CreateTestMetadata("Name1"), () => "TestComponent1"),
                new Export("TestContract1", CreateTestMetadata("Name2"), () => "TestComponent2"),
                new Export("TestContract2", CreateTestMetadata("Name3"), () => "TestComponent3"),
                new Export("TestContract2", CreateTestMetadata("Name4"), () => "TestComponent4"),
                new Export(AttributedModelServices.GetContractName(typeof(string)), CreateTestMetadata("Name5"), () => "TestComponent5"),
                new Export(AttributedModelServices.GetContractName(typeof(string)), CreateTestMetadata("Name6"), () => "TestComponent6"),
            };
        }

        private static IDictionary<string, object> CreateTestMetadata(string name) {
            return new Dictionary<string, object>() {
                {"Name", name},
                {"TestData", "Foo"}
            };
        }
    }
}
