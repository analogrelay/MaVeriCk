// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ComponentCollectionTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ComponentCollectionTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Maverick.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Data.Tests {
    [TestClass]
    public class ComponentCollectionTests {
        [TestMethod]
        public void Add_Requires_Non_Null_Export() {
            ComponentCollection<DataContextFactory> collection = new ComponentCollection<DataContextFactory>();
            AutoTester.ArgumentNull<Export<DataContextFactory, ComponentMetadata>>(
                marker => collection.Add(marker));
        }
    }
}
