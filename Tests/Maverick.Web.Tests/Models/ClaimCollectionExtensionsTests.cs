// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ClaimCollectionExtensionsTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ClaimCollectionExtensionsTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maverick.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.IdentityModel.Claims;
using Moq;

namespace Maverick.Web.Tests.Models {
    [TestClass]
    public class ClaimCollectionExtensionsTests {
        [TestMethod]
        public void ValueOrDefault_Without_DefaultValue_Returns_Null_If_No_Such_Claim_Exists() {
            // Arrange
            ClaimCollection claims = CreateTestClaims();

            // Act
            string value = claims.ValueOrDefault("qux");

            // Assert
            Assert.IsNull(value);
        }

        [TestMethod]
        public void ValueOrDefault_With_DefaultValue_Returns_Default_If_No_Such_Claim_Exists() {
            // Arrange
            ClaimCollection claims = CreateTestClaims();

            // Act
            string value = claims.ValueOrDefault("qux", "quark");

            // Assert
            Assert.AreEqual("quark", value);
        }

        [TestMethod]
        public void ValueOrDefault_Without_DefaultValue_Returns_Value_Of_Claim_If_It_Exists() {
            // Arrange
            ClaimCollection claims = CreateTestClaims();

            // Act
            string value = claims.ValueOrDefault("bar");

            // Assert
            Assert.AreEqual("baz", value);
        }

        [TestMethod]
        public void ValueOrDefault_With_DefaultValue_Returns_Value_Of_Claim_If_It_Exists() {
            // Arrange
            ClaimCollection claims = CreateTestClaims();

            // Act
            string value = claims.ValueOrDefault("bar", "quark");

            // Assert
            Assert.AreEqual("baz", value);
        }

        private static ClaimCollection CreateTestClaims() {
            return new ClaimCollection(new Mock<IClaimsIdentity>().Object) {
                new Claim("foo", "bar"),
                new Claim("bar", "baz")
            };
        }
    }
}
