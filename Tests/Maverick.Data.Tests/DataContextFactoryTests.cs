// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextFactoryTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextFactoryTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Maverick.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Data.Tests {
    [TestClass]
    public class DataContextFactoryTests {
        [TestMethod]
        public void DataContextFactory_Is_ContractType_With_MetadataView() {
            CompositionAssert.IsContractType(typeof(DataContextFactory), typeof(ComponentMetadata));
        }
    }
}
