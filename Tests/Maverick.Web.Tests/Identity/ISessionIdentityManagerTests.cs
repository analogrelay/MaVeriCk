// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ISessionIdentityManagerTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ISessionIdentityManagerTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Maverick.Web.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Web.Tests.Identity {
    [TestClass]
    public class ISessionIdentityManagerTests {
        [TestMethod]
        public void ISessionIdentityManager_Is_ContractType() {
            CompositionAssert.IsContractType(typeof(ISessionIdentityManager));
        }
    }
}
