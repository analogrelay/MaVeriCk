// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleRequestContext.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleRequestContext type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Routing;
using Maverick.Models;

namespace Maverick.Web.ModuleFramework {
    public class ModuleRequestContext {
        public Module Module { get; set; }
        public ModuleApplication Application { get; set; }
        public RouteData RouteData { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "This property is a relative URL which arrives from the routing engine as a string")]
        public string ModuleRoutingUrl { get; set; }
        public HttpContextBase HttpContext { get; set; }
    }
}
