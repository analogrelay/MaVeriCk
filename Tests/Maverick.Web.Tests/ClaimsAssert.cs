// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ClaimsAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ClaimsAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maverick.Web.Tests {
    public static class ClaimsAssert {
        public static void HasClaim(IEnumerable<Claim> claims, string type, string value) {
            HasClaim(claims, new Claim(type, value));
        }

        public static void HasClaim(IEnumerable<Claim> claims, string type, string value, string valueType, string issuer) {
            HasClaim(claims, new Claim(type, value, valueType, issuer));
        }

        public static void HasClaim(IEnumerable<Claim> claims, Claim expected) {
            Assert.AreEqual(1,
                            (from c in claims
                             where c.ClaimType == expected.ClaimType &&
                                   c.Value == expected.Value &&
                                   (String.IsNullOrEmpty(expected.OriginalIssuer) || c.OriginalIssuer == expected.OriginalIssuer) &&
                                   (String.IsNullOrEmpty(expected.ValueType) || c.ValueType == expected.ValueType) &&
                                   (String.IsNullOrEmpty(expected.Issuer) || c.Issuer == expected.Issuer)
                             select c).Count());
        }
    }
}
