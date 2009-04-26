// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="StylesheetExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the StylesheetExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Web.Mvc;

namespace Maverick.Web.Helpers {
    // TODO: This is NOT the place for this
    public static class ImportExtensions {
        public static string Stylesheet(this HtmlHelper helper, string contentPath) {
            Arg.NotNull("helper", helper);
            Arg.NotNullOrEmpty("contentPath", contentPath);

            // HACK: Need a better way to use UrlHelper
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);

            return String.Format(CultureInfo.InvariantCulture,
                                 "<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\"></link>",
                                 urlHelper.Content(contentPath));
        }

        public static string Script(this HtmlHelper helper, string contentPath) {
            Arg.NotNull("helper", helper);
            Arg.NotNullOrEmpty("contentPath", contentPath);

            // HACK: Need a better way to use UrlHelper
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);

            return String.Format(CultureInfo.InvariantCulture,
                                 "<script src=\"{0}\" type=\"text/javascript\"></script>",
                                 urlHelper.Content(contentPath));
        }
    }
}
