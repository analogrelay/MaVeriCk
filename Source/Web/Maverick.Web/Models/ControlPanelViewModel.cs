// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ControlPanelViewModel.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ControlPanelViewModel type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Mvc;

namespace Maverick.Web.Models {
    public class ControlPanelViewModel {
        public IEnumerable<SelectListItem> Modules { get; set; }
    }
}
