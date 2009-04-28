// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PrincipalExtensionsTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PrincipalExtensionsTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Principal;
using Maverick.Web.Models;
using Microsoft.IdentityModel.Claims;

namespace Maverick.Web.Tests.Models {
    [TestClass]
    public class PrincipalExtensionsTests {
        [TestMethod]
        public void AsUserIdentity_Returns_UnauthenticatedIdentity_If_Principal_Not_IClaimsPrincipal() {
            // Arrange
            IPrincipal principal = Mockery.CreatePrincipal();
            
            // Act
            UserIdentity identity = principal.AsUserIdentity();

            // Assert
            Assert.IsFalse(identity.Authenticated);
            Assert.IsNull(identity.Principal);
        }

        [TestMethod]
        public void AsUserIdentity_Returns_AuthenticatedIdentity_If_Principal_Is_IClaimsPrincipal() {
            // Arrange
            IClaimsPrincipal principal = Mockery.CreateClaimsPrincipal();

            // Act
            UserIdentity identity = principal.AsUserIdentity();

            // Assert
            Assert.IsTrue(identity.Authenticated);
            Assert.AreSame(principal, identity.Principal);
        }
    }
}
