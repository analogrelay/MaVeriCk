// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PagePrerouter.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PagePrerouter type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Web;
using System.Web.Routing;
using Maverick.DomainServices;
using Maverick.Models;
using Maverick.Web.Helpers;

namespace Maverick.Web.Routing {
    [Export]
    public class PagePrerouter : PrerouterBase {
        [Import(typeof(PageRepository))]
        public PageRepository PageRepository { get; set; }

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            PortalRequestContext context = httpContext.GetPortalContext();
            if(context.ActivePage != null) {
                return null;
            }

            string pagePath = httpContext.Request.AppRelativeCurrentExecutionFilePath;

            // Normalize path to the form: /Segment/Segment/Segment
            pagePath = NormalizePagePath(pagePath);

            // Search for the page
            Page page = PageRepository.GetLongestPrefixMatch(pagePath);
            
            // If there is no matching page, return null
            if(page == null) {
                return null;
            }

            // Set the page in the context
            context.ActivePage = page;

            // Remove the actual page path and set as the new app-relative path
            string appRelativePath = pagePath.Substring(page.Path.Length);

            // Rewrite and reroute the request
            // TODO: Can HttpContext.RewritePath do what we need?  I do want to preserve the old HttpContext for use after routing
            HttpContextBase rewrittenContext = new RewrittenHttpContext(httpContext, appRelativePath);
            return RerouteRequest(r => r.GetRouteData(rewrittenContext));
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            PortalRequestContext context = requestContext.HttpContext.GetPortalContext();

            // Remove the "page" from the route data before rerouting
            Page page = null;
            if(values.ContainsKey("page")) {
                // If page is present, but is null, then the user explicitly requested that no page routing be done
                page = values["page"] as Page;
                if(page == null) {
                    return null;
                }
                values.Remove("page");
            }

            // Reroute the request
            VirtualPathData pathData = RerouteRequest(r => r.GetVirtualPath(requestContext, values));
            if(pathData == null) {
                return null; // Rerouting failed
            }

            // If we didn't find a page to route the request to, select the active page
            if(page == null) {
                page = context.ActivePage;    
            }

            // If we still don't have a page, ignore this route
            if(page == null) {
                return null;
            }

            // Append the page path to the virtual path received and return the new path
            string newVirtualPath = String.Format(CultureInfo.InvariantCulture, "{0}/{1}", page.Path.Substring(1), pathData.VirtualPath).Trim('/');
            return new VirtualPathData(this, newVirtualPath);
        }

        protected virtual string NormalizePagePath(string pagePath) {
            if (pagePath.StartsWith("~/", StringComparison.OrdinalIgnoreCase)) {
                pagePath = pagePath.Substring(1);
            }
            else if (!pagePath.StartsWith("/", StringComparison.OrdinalIgnoreCase)) {
                pagePath = "/" + pagePath;
            }
            if (pagePath.EndsWith("/", StringComparison.OrdinalIgnoreCase)) {
                pagePath = pagePath.Substring(0, pagePath.Length - 1);
            }
            if (String.IsNullOrEmpty(pagePath)) {
                return "/";
            }
            return pagePath;
        }
    }
}
