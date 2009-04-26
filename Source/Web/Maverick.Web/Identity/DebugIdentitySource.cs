// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DebugIdentitySource.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DebugIdentitySource type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;

namespace Maverick.Web.Identity {
    [Export(typeof(IdentitySource))]
    [IdentitySource("Debug", "0.1.0.0")]
    public class DebugIdentitySource : IdentitySource {
        public override ActionResult Login(ControllerContext controllerContext, Uri returnUrl) {
            // Just give the user the SuperUser role
            SetSessionPrincipal(CreateSessionPrincipal(new List<Claim> {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "Maverick Developer"),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "SuperUser")
            }));
            return ReturnToLastPage(controllerContext);
        }
    }
}
