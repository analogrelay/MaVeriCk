// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PrincipalExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PrincipalExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Security.Principal;
using Microsoft.IdentityModel.Claims;

namespace Maverick.Web.Models {
    public static class PrincipalExtensions {
        public static UserIdentity AsUserIdentity(this IPrincipal principal) {
            IClaimsPrincipal claimsPrincipal = principal as IClaimsPrincipal;
            if(claimsPrincipal == null) {
                return null;
            }

            return new UserIdentity(claimsPrincipal);
        }
    }
}