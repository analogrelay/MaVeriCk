// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextManagerTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextManagerTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Maverick.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Data.Tests {
    [TestClass]
    public class DataContextManagerTests {
        [TestMethod]
        public void DataContextManager_Is_ContractType_With_MetadataView() {
            CompositionAssert.IsContractType(typeof(DataContextManager), typeof(ComponentMetadata));
        }
    }
}
