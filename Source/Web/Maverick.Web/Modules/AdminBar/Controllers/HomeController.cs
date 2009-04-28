// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HomeController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace Maverick.Web.Modules.AdminBar.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            // Can't use Authorize because we just want to hide the control panel from non-superusers
            if (!User.IsInRole("SuperUser")) {
                return new EmptyResult();
            }

            return View();
        }
    }
}
