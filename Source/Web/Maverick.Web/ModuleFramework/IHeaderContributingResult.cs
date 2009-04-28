// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IHeaderContributingResult" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IHeaderContributingResult type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Maverick.Web.ModuleFramework {
    /// <summary>
    /// Provides an interface to an ActionResult which also contributes to the header of a Maverick page
    /// </summary>
    /// <remarks>
    /// Since the module's action result is not executed until the module is rendered, it is too late for the module
    /// to contribute to the &lt;head&gt; tag of the page.  By using an ActionResult which implements this interface,
    /// the module can participate in the &lt;head&gt; tag.
    /// </remarks>
    public interface IHeaderContributingResult {
        void ExecuteHeader(ControllerContext context);
    }
}
