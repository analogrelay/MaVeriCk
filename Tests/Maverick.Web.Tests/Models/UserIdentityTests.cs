// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="UserIdentityTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the UserIdentityTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maverick.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.IdentityModel.Claims;
using TestUtilities;
using Moq;
using Maverick.Web.Identity;
using SysClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace Maverick.Web.Tests.Models {
    [TestClass]
    public class UserIdentityTests {
        private const string TestIdentifiedBy = "Foo";
        private const string TestName = "Bar";
        private const string TestEmail = "Baz";

        [TestMethod]
        public void Constructor_Initializes_Principal_Property() {
            IClaimsPrincipal expected = Mockery.CreateClaimsPrincipal();
            Assert.AreSame(expected, new UserIdentity(expected).Principal);
        }

        [TestMethod]
        public void Constructor_Sets_Authenticated_True_If_Principal_Non_Null() {
            Assert.IsTrue(new UserIdentity(Mockery.CreateClaimsPrincipal()).Authenticated);
        }

        [TestMethod]
        public void Constructor_Sets_Authenticated_False_If_Principal_Null() {
            Assert.IsFalse(new UserIdentity(null).Authenticated);
        }

        [TestMethod]
        public void GetClaim_Requires_Non_Null_Claims_Enumerable() {
            AutoTester.ArgumentNull<IEnumerable<Claim>>(marker => UserIdentity.GetClaim(marker, "Foo"));
        }

        [TestMethod]
        public void GetClaim_Static_Requires_Non_NullOrEmpty_ClaimType() {
            AutoTester.StringArgumentNullOrEmpty(marker => UserIdentity.GetClaim(Enumerable.Empty<Claim>(), marker));
        }

        [TestMethod]
        public void GetClaim_Instance_Requires_Non_NullOrEmpty_ClaimType() {
            AutoTester.StringArgumentNullOrEmpty(marker => new UserIdentity(null).GetClaim(marker));
        }

        [TestMethod]
        public void GetClaim_Instance_Returns_Null_If_Not_Authenticated() {
            Assert.IsNull(new UserIdentity(null).GetClaim("Foo"));
        }

        [TestMethod]
        public void GetClaim_Instance_Returns_Null_If_Principal_Has_No_Identities() {
            // Default mock behaviour will return null for Identities property
            Assert.IsNull(new UserIdentity(new Mock<IClaimsPrincipal>().Object).GetClaim("Foo"));
        }

        [TestMethod]
        public void IdentifiedBy_Gets_Value_Of_IdentifiedBy_Claim() {
            RunPropertyMappingTest(TestIdentifiedBy, identity => identity.IdentifiedBy);
        }

        [TestMethod]
        public void DisplayName_Gets_Value_Of_Name_Claim() {
            RunPropertyMappingTest(TestName, identity => identity.DisplayName);
        }

        [TestMethod]
        public void EmailAddress_Gets_Value_Of_Email_Claim() {
            RunPropertyMappingTest(TestEmail, identity => identity.EmailAddress);
        }

        private static void RunPropertyMappingTest(string expected, Func<UserIdentity, string> property) {
            // Arrange
            IClaimsPrincipal principal = CreateClaimsPrincipal();

            // Act
            UserIdentity identity = new UserIdentity(principal);

            // Assert
            Assert.AreEqual(expected, property(identity));
        }

        private static IClaimsPrincipal CreateClaimsPrincipal() {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(MaverickClaimTypes.IdentifiedBy, TestIdentifiedBy),
                new Claim(SysClaimTypes.Name, TestName), 
                new Claim(SysClaimTypes.Email, TestEmail), 
            }));
        }
    }
}
