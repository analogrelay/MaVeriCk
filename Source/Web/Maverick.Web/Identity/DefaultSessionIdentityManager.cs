// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DefaultSessionIdentityManager.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DefaultSessionIdentityManager type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;

namespace Maverick.Web.Identity {
    [Export(typeof(ISessionIdentityManager))]
    public class DefaultSessionIdentityManager : ISessionIdentityManager {
        public void ClearSessionPrincipal() {
            FederatedAuthentication.SignOut();
        }

        public void SetSessionPrincipal(SessionSecurityToken token) {
            SessionAuthenticationModule authModule = new SessionAuthenticationModule();
            authModule.SetPrincipalAndWriteSessionToken(token, true);
        }
    }
}