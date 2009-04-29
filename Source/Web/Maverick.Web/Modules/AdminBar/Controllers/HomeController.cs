// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HomeController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Maverick.Web.Models;
using Maverick.Web.Modules.AdminBar.Models;
using Maverick.Web.ModuleFramework;
using System.ComponentModel.Composition;

namespace Maverick.Web.Modules.AdminBar.Controllers {
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HomeController : Controller {
        [Import]
        public ModuleApplicationCollection ModuleApplications { get; set; }

        public ActionResult Index() {
            // Can't use Authorize because we just want to hide the control panel from non-superusers
            if (!User.IsInRole("SuperUser")) {
                return new EmptyResult();
            }

            return View(new ControlPanelViewModel() {
                ModuleApplications = CreateModuleApplicationModels()
            });
        }

        public ActionResult ListApps() {
            // Can't use Authorize because we just want to hide the control panel from non-superusers
            if (!User.IsInRole("SuperUser")) {
                return new EmptyResult();
            }

            return PartialView(CreateModuleApplicationModels());
        }

        private IList<ModuleApplicationViewModel> CreateModuleApplicationModels() {
            return (from appExport in ModuleApplications
                    let metadata = appExport.MetadataView
                    select new ModuleApplicationViewModel() {
                        Id = metadata.Id.ToString("N"),
                        Name = metadata.Name,
                        Description = metadata.Description,
                        LogoUrl = metadata.LogoUrl == null ? null : metadata.LogoUrl.ToString(),
                        Version = metadata.Version.ToString(),
                        Vendor = metadata.Vendor
                    }).ToList();
        }
    }
}
