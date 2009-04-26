// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentityViewHelpers.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentityViewHelpers type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Maverick.Web.Identity {
    public static class IdentityViewHelpers {
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "The term Login is commonly used on the Web")]
        public static MvcForm BeginLoginForm(this HtmlHelper helper) {
            return helper.BeginForm("Login", "Identity", new {id = helper.ViewContext.RouteData.Values["id"]}, FormMethod.Post, null);
        }
    }
}
