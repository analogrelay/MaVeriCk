// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalVirtualizationPrerouter.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalVirtualizationPrerouter type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Web;
using System.Web.Routing;
using Maverick.DomainServices;
using Maverick.Models;
using Maverick.Web.Helpers;

namespace Maverick.Web.Routing
{
    [Export]
    public class PortalVirtualizationPrerouter : PrerouterBase {
        [Import(typeof(PortalPrefixRepository))]
        public PortalPrefixRepository PortalPrefixRepository { get; set; }

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            PortalRequestContext portalRequestContext = httpContext.GetPortalContext();

            if(httpContext.HasPortalContext() && portalRequestContext.ActivePortal != null) {
                return null; // Portal has already been specified, bypass this pre-router
            }

            Uri requestUrl = httpContext.Request.Url;

            PortalPrefix prefix = PortalPrefixRepository.GetLongestPrefixMatch(requestUrl);
            
            if(prefix == null || prefix.Portal == null) {
                return null;
            }

            portalRequestContext.ActivePortal = prefix.Portal;
            portalRequestContext.ActivePortalPrefix = prefix;

            // Determine the Portal-relative url
            string portalRelativeUrl = ExtractPortalRelativeUrl(requestUrl, prefix);

            // Rewrite and re-route the request
            // TODO: Can HttpContext.RewritePath do what we need?  I do want to preserve the old HttpContext for use after routing
            HttpContextBase rewrittenContext = new RewrittenHttpContext(httpContext, portalRelativeUrl);
            return RerouteRequest(route => route.GetRouteData(rewrittenContext));
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            PortalRequestContext portalRequestContext = requestContext.HttpContext.GetPortalContext();
            if(portalRequestContext.ActivePortalPrefix == null) {
                return null;
            }

            // Use the other routes to get the base path
            VirtualPathData pathData = RerouteRequest(route => route.GetVirtualPath(requestContext, values));
            if(pathData == null) {
                return null;
            }

            // Remove the host string from the portal prefix
            string prefix = portalRequestContext.ActivePortalPrefix.Prefix;
            string host = GetHostString(requestContext.HttpContext.Request.Url);
            if (!prefix.StartsWith(host, StringComparison.OrdinalIgnoreCase)) {
                // TODO: Should throw exception?
                return null; // Cannot generate link, active portal prefix does not match request url
            }
            prefix = prefix.Substring(host.Length);

            // Remove the application path from the portal prefix
            string applicationPath = requestContext.HttpContext.Request.ApplicationPath;
            if (!prefix.StartsWith(applicationPath, StringComparison.OrdinalIgnoreCase)) {
                // TODO: Should throw exception?
                return null; // Cannot generate link, active portal prefix does not match request url
            }
            prefix = prefix.Substring(applicationPath.Length);

            // Verify the format of the prefix
            if (prefix.StartsWith("/", StringComparison.OrdinalIgnoreCase)) {
                prefix = prefix.Substring(1);
            }
            if(!prefix.EndsWith("/", StringComparison.OrdinalIgnoreCase)) {
                prefix += "/";
            }

            // Append the prefix and return the path data
            return new VirtualPathData(this, prefix + pathData.VirtualPath);
        }

        protected virtual string ExtractPortalRelativeUrl(Uri requestUrl, PortalPrefix prefix) {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(GetHostString(requestUrl));
            urlBuilder.Append(requestUrl.AbsolutePath);
            if(!requestUrl.AbsolutePath.EndsWith("/", StringComparison.OrdinalIgnoreCase)) {
                urlBuilder.Append("/");
            }
            return urlBuilder.ToString().Substring(prefix.Prefix.Length);
        }

        private static string GetHostString(Uri requestUrl) {
            StringBuilder hostBuilder = new StringBuilder();
            hostBuilder.Append(requestUrl.Host);
            if (requestUrl.Port != 80) {
                hostBuilder.Append(":");
                hostBuilder.Append(requestUrl.Port);
            }
            return hostBuilder.ToString();
        }
    }
}
