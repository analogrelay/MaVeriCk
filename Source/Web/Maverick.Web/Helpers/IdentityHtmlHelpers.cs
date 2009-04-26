// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentityHtmlHelpers.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentityHtmlHelpers type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Maverick.Models;
using Maverick.Web.Models;
using Microsoft.IdentityModel.Claims;

namespace Maverick.Web.Helpers {
    public static class IdentityHtmlHelpers {
        internal const string DefaultLoginLinkTitle = "Login";
        internal const string DefaultLoginAction = "Login";
        internal const string DefaultLoginController = "Identity";
        internal const string DefaultLogoutLinkTitle = "Logout";
        internal const string DefaultLogoutController = "Identity";
        internal const string DefaultLogoutAction = "Logout";

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "The term Login is commonly used on the Web")]
        public static string LoginLink(this HtmlHelper htmlHelper) {
            Arg.NotNull("htmlHelper", htmlHelper);
            return LoginLink(htmlHelper, DefaultLoginLinkTitle);
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "The term Login is commonly used on the Web")]
        public static string LoginLink(this HtmlHelper htmlHelper, string title) {
            Arg.NotNull("htmlHelper", htmlHelper);
            Arg.NotNullOrEmpty("title", title);
            return htmlHelper.ActionLink(title, DefaultLoginAction, DefaultLoginController, new {returnUrl = GetReturnUrl(htmlHelper), page = (Page)null}, null);
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "The term Login is commonly used on the Web")]
        public static string LoginLink(this HtmlHelper htmlHelper, string title, string identitySource) {
            Arg.NotNull("htmlHelper", htmlHelper);
            Arg.NotNullOrEmpty("title", title);
            Arg.NotNullOrEmpty("identitySource", identitySource);
            return htmlHelper.ActionLink(title, DefaultLoginAction, DefaultLoginController, new { returnUrl = GetReturnUrl(htmlHelper), id = identitySource, page = (Page)null }, null);
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Logout", Justification = "The term Logout is commonly used on the Web")]
        public static string LogoutLink(this HtmlHelper htmlHelper) {
            return LogoutLink(htmlHelper, DefaultLogoutLinkTitle);
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Logout", Justification = "The term Logout is commonly used on the Web")]
        public static string LogoutLink(this HtmlHelper htmlHelper, string title) {
            Arg.NotNull("htmlHelper", htmlHelper);
            Arg.NotNullOrEmpty("title", title);
            return htmlHelper.ActionLink(title, DefaultLogoutAction, DefaultLogoutController, new {page = (Page)null}, null);
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Logout", Justification = "The term Logout is commonly used on the Web")]
        public static string LogoutLink(this HtmlHelper htmlHelper, string title, string identitySource) {
            Arg.NotNull("htmlHelper", htmlHelper);
            Arg.NotNullOrEmpty("title", title);
            Arg.NotNullOrEmpty("identitySource", identitySource);
            return htmlHelper.ActionLink(title, DefaultLogoutAction, DefaultLogoutController, new { id = identitySource, page = (Page)null }, null);
        }

        public static UserIdentity CurrentUser(this HtmlHelper htmlHelper) {
            Arg.NotNull("htmlHelper", htmlHelper);
            if (!htmlHelper.ViewContext.HttpContext.Request.IsAuthenticated) {
                return null;
            }
            IClaimsPrincipal principal = htmlHelper.ViewContext.HttpContext.User as IClaimsPrincipal;
            if(principal == null) {
                return null;
            }
            return new UserIdentity(principal);
        }

        private static Uri GetReturnUrl(HtmlHelper htmlHelper) {
            return htmlHelper.ViewContext.HttpContext.Request.Url;
        }
    }
}
