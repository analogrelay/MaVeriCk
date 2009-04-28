// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleApplication.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleApplication type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.Web.Properties;
using Maverick.Web.Routing;

namespace Maverick.Web.ModuleFramework {
    [ContractType(MetadataViewType = typeof(ModuleApplicationMetadata))]
    public abstract class ModuleApplication {
        private const string ControllerMasterFormat = "~/Modules/{0}/Views/{{1}}/{{0}}.master";
        private const string SharedMasterFormat = "~/Modules/{0}/Views/Shared/{{0}}.master";
        private const string ControllerViewFormat = "~/Modules/{0}/Views/{{1}}/{{0}}.aspx";
        private const string SharedViewFormat = "~/Modules/{0}/Views/Shared/{{0}}.aspx";
        private const string ControllerPartialFormat = "~/Modules/{0}/Views/{{1}}/{{0}}.ascx";
        private const string SharedPartialFormat = "~/Modules/{0}/Views/Shared/{{0}}.ascx";
        private readonly object _lock = new object();
        private IControllerFactory _controllerFactory;
        private RouteCollection _route = new RouteCollection();
        private ViewEngineCollection _viewEngines = new ViewEngineCollection();
        private bool _initialized;
        
        public IControllerFactory ControllerFactory {
            get {
                return _controllerFactory ?? ControllerBuilder.Current.GetControllerFactory();
            }
            set { _controllerFactory = value; }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consumers of this class may wish to completely replace the route collection.  Tests also replace this collection")]
        public RouteCollection Routes {
            get { return _route; }
            set { _route = value; }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consumers of this class may wish to completely replace the view engine collection.  Tests also replace this collection")]
        public ViewEngineCollection ViewEngines {
            get { return _viewEngines; }
            set { _viewEngines = value; }
        }

        protected abstract string FolderPath {
            get;
        }

        public virtual ModuleRequestResult ExecuteRequest(ModuleRequestContext context) {
            EnsureInitialized(context.HttpContext);

            // Create a rewritten HttpRequest (wrapped in an HttpContext) to provide to the routing system
            HttpContextBase rewrittenContext = new RewrittenHttpContext(context.HttpContext, context.ModuleRoutingUrl);

            // Route the request
            RouteData routeData = GetRouteData(rewrittenContext);

            // Setup request context
            string controllerName = routeData.GetRequiredString("controller");
            RequestContext requestContext = new RequestContext(context.HttpContext, routeData);

            // Construct the controller using the ControllerFactory
            IController controller = ControllerFactory.CreateController(requestContext, controllerName);
            try {
                // Check if the controller supports IModuleController and if not, try to adapt it
                IModuleController moduleController = controller as IModuleController;
                if (moduleController == null) {
                    moduleController = AdaptController(controller);
                }

                // If we couldn't adapt it, we fail.  We can't support IController implementations without some kind of adaptor :(
                // Because we need to retrieve the ActionResult without executing it, IController won't cut it
                if (moduleController == null) {
                    throw new InvalidOperationException(Resources.Error_CouldNotConstructController);
                }

                // Execute the controller and capture the result
                moduleController.Execute(requestContext);
                ActionResult result = moduleController.ResultOfLastExecute;

                // Check if the result should override the rest of the page content, and if so, package it in a PageOverrideResult
                if (!(result is PageOverrideResult) &&
                    ShouldOverrideOtherModules(result, context, moduleController.ControllerContext)) {
                    result = new PageOverrideResult(result);
                }

                // Return the final result
                return new ModuleRequestResult {
                    Application = this,
                    ActionResult = AdaptResult(result),
                    ControllerContext = moduleController.ControllerContext,
                    Module = context.Module
                };
            }
            finally {
                ControllerFactory.ReleaseController(controller);
            }
        }

        private void EnsureInitialized(HttpContextBase context) {
            // Double-check lock to wait for initialization
            // TODO: Is there a better (preferably using events and waits) way to do this?
            if (!_initialized) {
                lock(_lock) {
                    if (!_initialized) {
                        Init(context.ApplicationInstance as MaverickApplication);
                        _initialized = true;
                    }
                }
            }
        }

        protected internal virtual ActionResult AdaptResult(ActionResult result) {
            ViewResultBase viewResult = result as ViewResultBase;
            return viewResult != null ? new HeaderContributingViewResultAdapter(viewResult) : result;
        }

        protected internal virtual IModuleController AdaptController(IController controller) {
            Controller mvcController = controller as Controller;
            if (mvcController != null && mvcController.ActionInvoker is ControllerActionInvoker) {
                return new MvcControllerAdapter(mvcController);
            }
            return null;
        }

        protected internal virtual RouteData GetRouteData(HttpContextBase httpContext) {
            return Routes.GetRouteData(httpContext);
        }

        protected internal virtual bool ShouldOverrideOtherModules(ActionResult result, ModuleRequestContext moduleRequestContext, ControllerContext controllerContext) {
            // All other results, such as "File", "Json", and "Partial View" (which is usually used for AJAX Partial Rendering)
            // will override the page and be rendered as the sole result to the client
            return result is FileResult ||
                   result is HttpUnauthorizedResult ||
                   result is JavaScriptResult ||
                   result is JsonResult ||
                   result is RedirectResult ||
                   result is RedirectToRouteResult ||
                   result is PartialViewResult;
        }

        protected internal virtual void Init(MaverickApplication application) {
            string prefix = NormalizeFolderPath(FolderPath);
            string[] masterFormats = new[] { 
                String.Format(CultureInfo.InvariantCulture, ControllerMasterFormat, prefix),
                String.Format(CultureInfo.InvariantCulture, SharedMasterFormat, prefix) };
            string[] viewFormats = new[] { 
                String.Format(CultureInfo.InvariantCulture, ControllerViewFormat, prefix),
                String.Format(CultureInfo.InvariantCulture, SharedViewFormat, prefix),
                String.Format(CultureInfo.InvariantCulture, ControllerPartialFormat, prefix),
                String.Format(CultureInfo.InvariantCulture, SharedPartialFormat, prefix) };

            ViewEngines.Add(new WebFormViewEngine {
                MasterLocationFormats = masterFormats,
                ViewLocationFormats = viewFormats,
                PartialViewLocationFormats = viewFormats
            });
        }

        private static string NormalizeFolderPath(string path) {
            // Remove leading and trailing slashes
            if (!String.IsNullOrEmpty(path)) {
                return path.Trim('/');
            }
            return path;
        }
    }
}
