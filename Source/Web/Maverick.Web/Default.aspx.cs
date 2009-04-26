// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Default.aspx.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DefaultPage type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Diagnostics;

namespace Maverick.Web {
    public class DefaultPage : Page {
        [DebuggerNonUserCode]
        public void Page_Load(object sender, EventArgs e) {
            // Change the current path so that the Routing handler can correctly interpret
            // the request, then restore the original path so that the OutputCache module
            // can correctly process the response (if caching is enabled).

            string originalPath = Request.Path;
            HttpContext.Current.RewritePath(Request.ApplicationPath, false);
            IHttpHandler httpHandler = new MvcHttpHandler();
            httpHandler.ProcessRequest(HttpContext.Current);
            HttpContext.Current.RewritePath(originalPath, false);
        }
    }
}
