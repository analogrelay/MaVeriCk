// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ISessionIdentityManager.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ISessionIdentityManager type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.IdentityModel.Tokens;

namespace Maverick.Web.Identity {
    [ContractType]
    public interface ISessionIdentityManager {
        void ClearSessionPrincipal();
        void SetSessionPrincipal(SessionSecurityToken token);
    }
}