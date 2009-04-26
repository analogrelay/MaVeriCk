// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HomeController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.Mvc;

namespace Maverick.Web.Modules.CurrentTime.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewData["CurrentTime"] = DateTimeOffset.UtcNow.ToString();
            return View();
        }

        public ActionResult IndexJson() {
            return Json(new {
                currentTime = DateTimeOffset.UtcNow.ToString()
            });
        }
    }
}
