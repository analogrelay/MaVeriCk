// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleExecutionEngine.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleExecutionEngine type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Maverick.Models;

namespace Maverick.Web.ModuleFramework {
    [Export]
    public class ModuleExecutionEngine {
        private static ModuleExecutionEngine _current;

        public static ModuleExecutionEngine Current {
            get {
                if(_current == null) {
                    _current = MaverickApplication.Container.GetExportedObject<ModuleExecutionEngine>();
                }
                return _current;
            }
            set { _current = value; }
        }

        [ImportMany(typeof(ModuleApplication))]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "The composition engine must be able to set this property")]
        public ModuleApplicationCollection ModuleApplications { get; set; }

        public virtual ModuleRequestResult ExecuteModule(HttpContextBase httpContext, Module moduleModel, string moduleRoute) {
            Arg.NotNull("httpContext", httpContext);
            Arg.NotNull("moduleModel", moduleModel);
            Arg.NotNull("moduleRoute", moduleRoute); // Empty route is OK!

            // If the module application for this module is installed
            if (ModuleApplications.Contains(moduleModel.ModuleApplicationId)) {
                // Get the module's application
                ModuleApplication app = ModuleApplications[moduleModel.ModuleApplicationId].GetExportedObject();

                // Setup the module's context
                ModuleRequestContext moduleRequestContext = new ModuleRequestContext {
                    Application = app,
                    Module = moduleModel,
                    ModuleRoutingUrl = moduleRoute,
                    HttpContext = httpContext
                };


                // Run the module
                ModuleRequestResult result = app.ExecuteRequest(moduleRequestContext);
                return result;
            }
            return null;
        }


        public virtual void ExecuteModuleResult(PortalRequestContext portalContext, ModuleRequestResult moduleResult) {
            RunInModuleResultContext(portalContext,
                                     moduleResult,
                                     () => moduleResult.ActionResult.ExecuteResult(moduleResult.ControllerContext));
        }

        public virtual void ExecuteModuleHeader(PortalRequestContext portalContext, ModuleRequestResult moduleResult) {
            IHeaderContributingResult headerResult = moduleResult.ActionResult as IHeaderContributingResult;
            if (headerResult != null) {
                RunInModuleResultContext(portalContext,
                                         moduleResult,
                                         () => headerResult.ExecuteHeader(moduleResult.ControllerContext));
            }
        }

        protected internal void RunInModuleResultContext(PortalRequestContext portalContext, ModuleRequestResult moduleResult, Action action) {
            // Set the active module
            ModuleRequestResult oldRequest = portalContext.ActiveModuleRequest;
            portalContext.ActiveModuleRequest = moduleResult;

            // Run the action
            action();

            // Restore the previous active module
            portalContext.ActiveModuleRequest = oldRequest;
        }
    }
}
