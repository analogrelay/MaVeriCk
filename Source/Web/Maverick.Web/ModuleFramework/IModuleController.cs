// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IModuleController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IModuleController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace Maverick.Web.ModuleFramework {
    public interface IModuleController : IController {
        // HACK: I'm not a huge fan of this... rather replace IController with our own interface
        // HACK: But I'm pretty sure that would cause issues in the Controller Factory.
        ActionResult ResultOfLastExecute { get; }

        ControllerContext ControllerContext { get; }
    }
}
