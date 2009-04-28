// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Global.asax.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MaverickApplication type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.Composition;
using Maverick.Data;
using Maverick.DomainServices;
using Maverick.Web.Configuration;
using Maverick.Web.Controllers;
using Maverick.Web.ModuleFramework;
using Maverick.Web.Routing;
using Maverick.Web.Theming;

namespace Maverick.Web {
    public class MaverickApplication : HttpApplication {
        private static HttpContextBase _currentContext;
        private static RouteCollection _routes;
        private static string _dataContextManagerName;

        internal const string DefaultDataContextManagerName = "HttpContext";

        public static CompositionContainer Container { get; set; }

        [Import]
        public static PortalVirtualizationPrerouter PortalVirtualizationPrerouter { get; set; }

        [Import]
        public static PagePrerouter PagePrerouter { get; set; }

        public static HttpContextBase CurrentContext {
            get {
                if(_currentContext == null) {
                    _currentContext = new HttpContextWrapper(HttpContext.Current);
                }
                return _currentContext;
            }
            
            // We really don't want people playing with this outside of the test
            internal set { _currentContext = value; }
        }

        [Export(typeof(Func<HttpContextBase>))]
        public static HttpContextBase GetCurrentContext() {
            return CurrentContext;
        }

        public static RouteCollection Routes {
            get {
                if(_routes == null) {
                    _routes = RouteTable.Routes;
                }
                return _routes;
            }
            
            // We really don't want people playing with this outside of the test
            internal set { _routes = value; }
        }

        public static string DataContextManagerName {
            get {
                if(_dataContextManagerName == null) {
                    if(Container != null) {
                        _dataContextManagerName =
                            Container.GetExportedObjectOrDefault<string>(WebContractNames.DataContextManagerName);
                    }
                    if (String.IsNullOrEmpty(_dataContextManagerName)) {
                        _dataContextManagerName = DefaultDataContextManagerName;
                    }
                }
                return _dataContextManagerName;
            }
            set { _dataContextManagerName = value; }
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("PortalVirtualization", PortalVirtualizationPrerouter);

            routes.Add("PageRouting", PagePrerouter);

            routes.MapRoute(
                "ModuleRoute",
                "{moduleId}/{*moduleRoute}",
                new {controller = "Page", action = "View", moduleId = String.Empty, moduleRoute = String.Empty},
                new {moduleId = @"[0-9]*"});

            routes.MapRoute(
                "ModuleRenderRoute",
                "_Module/Render/{moduleId}/{*moduleRoute}",
                new { controller = "Module", action = "Render", moduleRoute = String.Empty },
                new { moduleId = @"[0-9]+" }
            );
            routes.MapRoute(
                "Default",
                "_{controller}/{action}/{id}",
                new { controller = "Page", action = "View", id = String.Empty }
            );
        }

        internal static void Compose() {
            // Load catalogs from configuration
            Container = new CompositionContainer(new ConfiguredCatalog());

            PortalVirtualizationPrerouter = Container.GetExportedObject<PortalVirtualizationPrerouter>();
            PagePrerouter = Container.GetExportedObject<PagePrerouter>();
            DataBatch.DataContextManager = Container.GetComponentOrDefaultByName<DataContextManager>(DataContextManagerName);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This code was generated as an instance method by the ASP.Net MVC templates")]
        public void Application_Start() {
            Compose();

            RegisterRoutes(Routes);

            SetupMvcExtensions();
        }

        internal static void SetupMvcExtensions() {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ModuleDelegatingViewEngine());
            ViewEngines.Engines.Add(new ThemedWebFormViewEngine());
            ControllerBuilder.Current.SetControllerFactory(new CompositionContainerControllerFactory(Container));
        }
    }
}