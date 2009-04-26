// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleRequestResult.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleRequestResult type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;
using Maverick.Models;

namespace Maverick.Web.ModuleFramework {
    public class ModuleRequestResult {
        public ModuleApplication Application { get; set; }
        public ActionResult ActionResult { get; set; }
        public ControllerContext ControllerContext { get; set; }
        public Module Module { get; set; }
    }
}
