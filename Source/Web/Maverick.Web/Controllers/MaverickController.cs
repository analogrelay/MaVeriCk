// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MaverickController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MaverickController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;
using Maverick.Web.Helpers;

namespace Maverick.Web.Controllers {
    public abstract class MaverickController : Controller {
        protected internal virtual ResourceNotFoundResult ResourceNotFound() {
            return new ResourceNotFoundResult();
        }

        protected internal virtual ResourceNotFoundResult ResourceNotFound(string viewName) {
            Arg.NotNullOrEmpty("viewName", viewName);
            return ResourceNotFound(View(viewName));
        }

        protected internal virtual ResourceNotFoundResult ResourceNotFound(string viewName, string masterName) {
            Arg.NotNullOrEmpty("viewName", viewName);
            Arg.NotNullOrEmpty("masterName", masterName);
            return ResourceNotFound(View(viewName, masterName));
        }

        protected internal virtual ResourceNotFoundResult ResourceNotFound(ActionResult innerResult) {
            Arg.NotNull("innerResult", innerResult);
            return new ResourceNotFoundResult {InnerResult = innerResult};
        }
    }
}
