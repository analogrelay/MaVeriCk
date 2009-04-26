// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleRouteRewriter.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleRouteRewriter type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Routing;
using Maverick.Web.Helpers;
using Maverick.Web.Properties;

namespace Maverick.Web.Routing {
    public class ModuleRouteRewriter : PrerouterBase {
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consumers of this class must set this property to the route collection specified by the Module Application")]
        public RouteCollection ModuleRoutes { get; set; }

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            throw new NotSupportedException(Resources.Error_ModuleRouteRewriterOnlyForOutboundRouting);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            // Route the request through the module routes
            VirtualPathData pathData = GetVirtualPathOnAllRoutes(ModuleRoutes, requestContext, values);
            if(pathData == null) {
                return null;
            }

            // Create a new RouteValueDictionary containing the module route and current module id
            PortalRequestContext portalRequestContext = requestContext.HttpContext.GetPortalContext();
            if(portalRequestContext.ActiveModuleRequest == null || portalRequestContext.ActiveModuleRequest.Module == null) {
                return null;
            }

            RouteValueDictionary newValues = new RouteValueDictionary();
            newValues["moduleId"] = portalRequestContext.ActiveModuleRequest.Module.Id;
            newValues["moduleRoute"] = pathData.VirtualPath.TrimStart('/');

            VirtualPathData finalPathData = GetVirtualPathOnAllRoutes(RouteCollection, requestContext, newValues);

            return new VirtualPathData(this, finalPathData.VirtualPath.TrimStart('/'));
        }

        public static RouteCollection CreateRouteCollection(RouteCollection moduleRoutes) {
            return new RouteCollection {
                new ModuleRouteRewriter { ModuleRoutes = moduleRoutes }
            };
        }

        // RouteCollection.GetVirtualPath does extra processing we don't want here
        private static VirtualPathData GetVirtualPathOnAllRoutes(RouteCollection routes, RequestContext requestContext, RouteValueDictionary values) {
            using (routes.GetReadLock()) {
                foreach (RouteBase route in routes) {
                    VirtualPathData pathData = route.GetVirtualPath(requestContext, values);
                    if (pathData != null) {
                        return pathData;
                    }
                }
            }
            return null;
        }
    }
}
