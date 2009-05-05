using System.Collections.Generic;
using Maverick.Models;

namespace Maverick.Web.Modules.AdminBar.Models {
    public class ControlPanelViewModel {
        public Page ActivePage { get; set; }
        public IList<ModuleApplicationViewModel> ModuleApplications { get; set; }
    }
}