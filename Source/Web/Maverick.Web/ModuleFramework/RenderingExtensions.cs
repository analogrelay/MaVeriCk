// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="RenderModuleExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the RenderModuleExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;
using Maverick.Web.Helpers;

namespace Maverick.Web.ModuleFramework {
    public static class RenderingExtensions {
        public static void RenderModule(this HtmlHelper helper, ModuleRequestResult moduleResult) {
            ModuleExecutionEngine.Current.ExecuteModuleResult(helper.ViewContext.HttpContext.GetPortalContext(), moduleResult);
        }

        public static void RenderModuleHeader(this HtmlHelper helper, ModuleRequestResult moduleResult) {
            ModuleExecutionEngine.Current.ExecuteModuleHeader(helper.ViewContext.HttpContext.GetPortalContext(), moduleResult);
        }
    }
}
