// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PageOverrideResult.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PageOverrideResult type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace Maverick.Web.ModuleFramework {
    // "Marker" wrapper to cause the page to ignore the rest of the module and just render this content
    public class PageOverrideResult : ActionResult {
        public ActionResult InnerResult { get; private set; }

        public PageOverrideResult(ActionResult innerResult) {
            InnerResult = innerResult;
        }

        public override void ExecuteResult(ControllerContext context) {
            InnerResult.ExecuteResult(context);
        }
    }
}
