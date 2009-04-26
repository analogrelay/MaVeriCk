// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleRouteCollectionExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleRouteCollectionExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;
using System.Web.Routing;

namespace Maverick.Web.ModuleFramework {
    public static class ModuleRouteCollectionExtensions {
        private const string DefaultControllerName = "Home";
        private const string DefaultActionName = "Index";
        private const string DefaultId = "";
        private const string DefaultRouteName = "Default";

        public static void RegisterDefaultRoute(this RouteCollection routes, string controllerNamespace) {
            routes.RegisterDefaultRoute(DefaultControllerName, DefaultActionName, DefaultId, new[] {controllerNamespace});
        }

        public static void RegisterDefaultRoute(this RouteCollection routes, string[] namespaces) {
            routes.RegisterDefaultRoute(DefaultControllerName, DefaultActionName, DefaultId, namespaces);
        }

        public static void RegisterDefaultRoute(this RouteCollection routes, string defaultController, string[] namespaces) {
            routes.RegisterDefaultRoute(defaultController, DefaultActionName, DefaultId, namespaces);
        }
        public static void RegisterDefaultRoute(this RouteCollection routes, string defaultController, string defaultAction, string[] namespaces) {
            routes.RegisterDefaultRoute(defaultController, defaultAction, DefaultId, namespaces);
        }
        public static void RegisterDefaultRoute(this RouteCollection routes, string defaultController, string defaultAction, string defaultId, string[] namespaces) {
            routes.RegisterDefaultRoute(DefaultRouteName, defaultController, defaultAction, defaultId, namespaces);
        }
        public static void RegisterDefaultRoute(this RouteCollection routes, string routeName, string defaultController, string defaultAction, string defaultId, string[] namespaces) {
            routes.MapRoute(
                routeName,
                "{controller}/{action}/{id}",
                new {controller = defaultController, action = defaultAction, id = defaultId},
                namespaces
                );
        }
    }
}