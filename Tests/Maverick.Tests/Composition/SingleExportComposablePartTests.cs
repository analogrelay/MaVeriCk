// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SingleExportComposablePartTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the SingleExportComposablePartTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Maverick.Composition;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using TargetResources = Maverick.Properties.Resources;

namespace Maverick.Tests.Composition {
    [TestClass]
    public class SingleExportComposablePartTests {
        [TestMethod]
        public void Constructor_Requires_Non_Null_Export() {
            AutoTester.ArgumentNull<Export>(marker => new SingleExportComposablePart(marker));
        }

        [TestMethod]
        public void Constructor_Sets_ExportDefinitions_To_Single_Export() {
            Export export = new Export("Foo", new Dictionary<string, object>(), () => null);
            Assert.AreSame(export.Definition, new SingleExportComposablePart(export).ExportDefinitions.Single());
        }

        [TestMethod]
        public void Part_Has_No_Imports() {
            Export export = new Export("Foo", new Dictionary<string, object>(), () => null);
            Assert.AreEqual(0, new SingleExportComposablePart(export).ImportDefinitions.Count());
        }

        [TestMethod]
        public void GetExportedObject_Returns_Exported_Object_If_ExportDefinition_Matches_Single_Export() {
            object expected = new object();
            Export export = new Export("Foo", new Dictionary<string, object>(), () => expected);
            Assert.AreSame(expected, new SingleExportComposablePart(export).GetExportedObject(export.Definition));
        }

        [TestMethod]
        public void GetExportedObject_Guards_Against_Non_Matching_ExportDefinition() {
            object expected = new object();
            Export export = new Export("Foo", new Dictionary<string, object>(), () => expected);
            ExportDefinition nonMatching = new ExportDefinition("Bar", new Dictionary<string, object>());
            ExceptionAssert.Guards(() => new SingleExportComposablePart(export).GetExportedObject(nonMatching),
                                   TargetResources.Error_PartDoesNotContainAnExportForContract, nonMatching.ContractName);
        }

        [TestMethod]
        public void SetImport_Throws_InvalidOperationException() {
            Export export = new Export("Foo", new Dictionary<string, object>(), () => null);
            ExceptionAssert.Guards(() => new SingleExportComposablePart(export).SetImport(null, null),
                                   TargetResources.Error_PartDoesNotContainAnyImports);
        }
    }
}