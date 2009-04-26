// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MaverickClaimTypes.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MaverickClaimTypes type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace Maverick.Web.Identity {
    public static class MaverickClaimTypes {
        private const string RootUrl = "http://schemas.maverick.tempuri.org/claims/2009/04/";
        public const string IdentifiedBy = RootUrl + "identified-by";
    }
}
