// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="RenderModuleResult.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the RenderModuleResult type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;
using Maverick.Web.Helpers;

namespace Maverick.Web.ModuleFramework {
    public class RenderModuleResult : ActionResult {
        public ModuleRequestResult ModuleRequestResult { get; set; }

        public override void ExecuteResult(ControllerContext context) {
            Arg.NotNull("context", context);
            if(ModuleRequestResult != null) {
                ModuleExecutionEngine.Current.ExecuteModuleResult(context.HttpContext.GetPortalContext(), ModuleRequestResult);
            }
        }
    }
}
