// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HomeController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace Maverick.Web.Modules.TaskList.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult Create(string id) {
            return PartialView("Task", id);
        }
    }
}
