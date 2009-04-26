// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SingleExportComposablePartDefinitionBaseTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the SingleExportComposablePartDefinitionBaseTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Maverick.Composition;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Tests.Composition {
    [TestClass]
    public class SingleExportComposablePartDefinitionBaseTests {
        private class TestPartDefinition : SingleExportComposablePartDefinitionBase<string, string> {
            public TestPartDefinition(string name, string value) : base(name, value) {}

            protected override Func<string, string> CreateGetter() {
                return value => value + "Exported";
            }
        }

        [TestMethod]
        public void Constructor_Requires_Non_NullOrEmpty_Name() {
            AutoTester.StringArgumentNullOrEmpty(marker => new TestPartDefinition(marker, "Foo"));
        }

        [TestMethod]
        public void Constructor_Allows_Null_Value() {
            new TestPartDefinition("Foo", null);
        }

        [TestMethod]
        public void ExportDefinitions_Returns_Single_ExportDefinition_With_TypeIdentity_And_ContractName() {
            // Arrange
            TestPartDefinition partDefinition = new TestPartDefinition("Foo", "Bar");

            // Act
            ExportDefinition export = partDefinition.ExportDefinitions.Single();

            // Assert
            Assert.AreEqual(AttributedModelServices.GetTypeIdentity(typeof(string)), export.Metadata["ExportTypeIdentity"]);
            Assert.AreEqual("Foo", export.ContractName);
        }

        [TestMethod]
        public void ImportDefinitions_Returns_Empty_Enumerable() {
            Assert.AreEqual(0, new TestPartDefinition("Foo", "Bar").ImportDefinitions.Count());
        }

        [TestMethod]
        public void CreatePart_Returns_SingleExportComposablePart_Using_Getter_From_CreateGetter() {
            // Arrange
            TestPartDefinition partDefinition = new TestPartDefinition("Foo", "Bar");

            // Act
            ComposablePart part = partDefinition.CreatePart();

            // Assert
            Assert.AreSame(partDefinition.ExportDefinitions.Single(), part.ExportDefinitions.Single());
            Assert.AreEqual("BarExported", part.GetExportedObject(partDefinition.ExportDefinitions.Single()));
        }
    }
}