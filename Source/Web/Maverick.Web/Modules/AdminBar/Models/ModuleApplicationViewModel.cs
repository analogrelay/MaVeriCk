// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleApplicationViewModel.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleApplicationViewModel type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;

namespace Maverick.Web.Modules.AdminBar.Models {
    public class ModuleApplicationViewModel {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public string LogoUrl { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
    }
}