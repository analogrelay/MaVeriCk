// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PageController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PageController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Maverick.DomainServices;
using Maverick.Models;
using Maverick.Web.Helpers;
using Maverick.Web.Models;
using Maverick.Web.ModuleFramework;

namespace Maverick.Web.Controllers {
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PageController : Controller {
        [Import(typeof(ModuleExecutionEngine))]
        public ModuleExecutionEngine ModuleExecutor { get; set; }

        [Import(typeof(ModuleRepository))]
        public ModuleRepository ModuleRepository { get; set; }

        public Page ActivePage {
            get { return HttpContext.GetPortalContext().ActivePage; }
        }

        [ActionName("View")]
        public ActionResult ViewPage(int? moduleId, string moduleRoute) {
            // Get the current page
            Page page = ActivePage;

            // Start setting up the view model
            PageViewModel pageModel = new PageViewModel {
                Page = page
            };

            IList<Module> modules = page.Modules.ToList();

            // If there is a selected module (moduleId != null), run it first
            ModuleRequestResult selectedResult = null;
            if (moduleId.HasValue) {
                Module selectedModule = modules.SingleOrDefault(m => m.Id == moduleId.Value);
                if (selectedModule != null) {
                    selectedResult = ModuleExecutor.ExecuteModule(HttpContext, selectedModule, moduleRoute);

                    // If the result is a "Page Override" result (meaning it should be executed as the sole ActionResult for this page)
                    // Then execute it immediately
                    if (selectedResult != null && selectedResult.ActionResult is PageOverrideResult) {
                        return new RenderModuleResult {ModuleRequestResult = selectedResult};
                    }
                }
            }

            // Execute all the modules on the page            
            foreach (Module module in page.Modules) {
                ModuleRequestResult result;

                // If the current module is the selected module, then we already have the result, so we
                // don't need to execute it.
                if (moduleId.HasValue && moduleId.Value == module.Id) {
                    result = selectedResult;
                } else {
                    result = ModuleExecutor.ExecuteModule(HttpContext, module, String.Empty);
                }

                if (result != null) {
                    // Store the result, only the selected module can override the page
                    pageModel[module.ZoneName].ModuleResults.Add(result);
                }
            }

            return View(pageModel);
        }
    }
}
