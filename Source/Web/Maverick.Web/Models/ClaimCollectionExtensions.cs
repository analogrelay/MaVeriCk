// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ClaimCollectionExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ClaimCollectionExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.IdentityModel.Claims;

namespace Maverick.Web.Models {
    public static class ClaimCollectionExtensions {
        public static string ValueOrDefault(this ClaimCollection claims, string claimName) {
            return ValueOrDefault(claims, claimName, null);
        }

        public static string ValueOrDefault(this ClaimCollection claims, string claimName, string defaultValue) {
            Claim claim = claims.Where(c => String.Equals(c.ClaimType, claimName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            if(claim == null) {
                return defaultValue;
            }
            return claim.Value;
        }
    }
}