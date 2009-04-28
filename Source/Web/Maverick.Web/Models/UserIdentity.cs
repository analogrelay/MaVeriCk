// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="UserIdentity.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the UserIdentity type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Maverick.Web.Identity;
using Microsoft.IdentityModel.Claims;
using ClaimTypes=System.IdentityModel.Claims.ClaimTypes;

namespace Maverick.Web.Models {
    public class UserIdentity {
        public IClaimsPrincipal Principal { get; private set; }

        public string IdentifiedBy { get { return GetClaim(MaverickClaimTypes.IdentifiedBy); } }
        public string DisplayName { get { return GetClaim(ClaimTypes.Name); } }
        public string EmailAddress { get { return GetClaim(ClaimTypes.Email); } }
        public bool Authenticated { get; private set; }

        public UserIdentity(IClaimsPrincipal principal) {
            Principal = principal;
            Authenticated = principal != null;
        }

        internal string GetClaim(string claimType) {
            Arg.NotNullOrEmpty("claimType", claimType);

            if(!Authenticated || Principal.Identities == null) {
                return null;
            }
            // Check all the identities for the claim
            return (from identity in Principal.Identities
                    select GetClaim(identity.Claims, claimType)).FirstOrDefault();
        }

        internal static string GetClaim(IEnumerable<Claim> claims, string claimType) {
            Arg.NotNull("claims", claims);
            Arg.NotNullOrEmpty("claimType", claimType);

            return (from claim in claims
                    where claim.ClaimType == claimType
                    select claim.Value).FirstOrDefault();
        }
    }
}
