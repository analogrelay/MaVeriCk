// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;
using Maverick.DomainServices;
using Maverick.Models;
using Maverick.Web.Helpers;
using Maverick.Web.ModuleFramework;

namespace Maverick.Web.Controllers {
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ModuleController : MaverickController {
        [Import]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "The composition engine must be able to set this property")]
        public ModuleApplicationCollection ModuleApplications { get; set; }

        [Import]
        public ModuleExecutionEngine ModuleExecutor { get; set; }

        [Import]
        public ModuleRepository ModuleRepository { get; set; }

        public ModuleController() {
            ModuleApplications = new ModuleApplicationCollection();
        }

        [Authorize(Roles = "SuperUser")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string modules) {
            // Check for an active page
            PortalRequestContext portalContext = HttpContext.GetPortalContext();
            if(portalContext.ActivePage == null) {
                return ResourceNotFound();
            }
            
            // Parse the guid
            Guid appGuid = Guid.Empty;
            try {
                appGuid = new Guid(modules);
            } catch(FormatException) {
                return ResourceNotFound();
            }

            // Check the module application id
            if(!ModuleApplications.Contains(appGuid)) {
                return ResourceNotFound();
            }

            // Get the module application metadata
            ModuleApplicationMetadata metadata = ModuleApplications[appGuid].MetadataView;
            
            // Construct a new module and add it to the module service
            ModuleRepository.Add(new Module {
                ModuleApplicationId = metadata.Id,
                Page = portalContext.ActivePage,
                Title = metadata.Name,
                ZoneName = "Content"
            });

            return RedirectToAction("View", "Page");
        }

        [Authorize(Roles = "SuperUser")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id) {
            // Get the module with the specified id
            Module module = (from mod in ModuleRepository.GetAll()
                             where mod.Id == id
                             select mod).FirstOrDefault();

            if(module == null) {
                return ResourceNotFound();
            }
            
            ModuleRepository.Delete(module);

            return RedirectToAction("View", "Page");
        }

        public ActionResult Render(int moduleId, string moduleRoute) {
            Module module = (from m in ModuleRepository.GetAll()
                             where m.Id == moduleId
                             select m).FirstOrDefault();
            if(module == null) {
                return ResourceNotFound();
            }
            return new RenderModuleResult {
                ModuleRequestResult = ModuleExecutor.ExecuteModule(HttpContext, module, moduleRoute)
            };
        }
    }
}
