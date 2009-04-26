// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ResultCapturingActionInvoker.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ResultCapturingActionInvoker type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace Maverick.Web.ModuleFramework {
    public class ResultCapturingActionInvoker : ControllerActionInvoker {
        public ActionResult ResultOfLastInvoke { get; set; }

        // TODO: Capture result filters to execute later

        protected override void InvokeActionResult(ControllerContext controllerContext, ActionResult actionResult) {
            // Do not invoke the action.  Instead, store it for later retrieval
            ResultOfLastInvoke = actionResult;
        }
    }
}