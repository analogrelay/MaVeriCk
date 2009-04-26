// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleHtmlHelpers.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleHtmlHelpers type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Web.Mvc;
using Maverick.Web.ModuleFramework;

namespace Maverick.Web.Helpers {
    public static class ModuleHtmlHelpers {
        public static string Id(this HtmlHelper helper, string baseId) {
            Arg.NotNull("helper", helper);
            Arg.NotNullOrEmpty("baseId", baseId);

            ModuleRequestResult moduleContext = helper.ViewContext.HttpContext.GetActiveModule();
            if(moduleContext != null && moduleContext.Module != null) {
                return String.Format(CultureInfo.InvariantCulture, 
                                     "m{0}_{1}", 
                                     moduleContext.Module.Id, 
                                     baseId);
            }
            return baseId;
        }
    }
}
