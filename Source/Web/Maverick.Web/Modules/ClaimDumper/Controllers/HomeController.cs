// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HomeController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;
using Microsoft.IdentityModel.Claims;

namespace Maverick.Web.Modules.ClaimDumper.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            IClaimsPrincipal principal = User as IClaimsPrincipal;
            if (principal != null && principal.Identities.Count > 0) {
                IClaimsIdentity identity = principal.Identities[0];
                return View(identity);
            }
            return Content("No current user");
        }
    }
}
