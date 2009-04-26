// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleRequestContextExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleRequestContextExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web;
using Maverick.Web.Helpers;

namespace Maverick.Web.ModuleFramework {
    public static class ModuleRequestContextExtensions {
        public static ModuleRequestResult GetActiveModule(this HttpContextBase httpContext) {
            ModuleRequestResult result = httpContext.GetPortalContext().ActiveModuleRequest;
            if (result == null || result.Application == null) {
                return null;
            }
            return result;
        }
    }
}
