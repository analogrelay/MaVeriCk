// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalRequestContext.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalRequestContext type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web;
using Maverick.Models;
using Maverick.Web.ModuleFramework;

namespace Maverick.Web {
    // Statically-typed Portal Context data
    public class PortalRequestContext {
        private string _currentTheme;
        
        public PortalRequestContext(HttpContextBase httpContext) {
            Arg.NotNull("httpContext", httpContext);
            HttpContext = httpContext;
        }

        public HttpContextBase HttpContext { get; private set; }
        public Portal ActivePortal { get; set; }
        public PortalPrefix ActivePortalPrefix { get; set; }
        public Page ActivePage { get; set;}
        public ModuleRequestResult ActiveModuleRequest { get; set; }

        public string CurrentTheme {
            get {
                if(_currentTheme == null && MaverickApplication.Container != null) {
                    _currentTheme = MaverickApplication.Container
                                                       .GetExportedObjectOrDefault<string>(WebContractNames.AppDefaultTheme);
                }
                return _currentTheme;
            }
            set { _currentTheme = value; }
        }
    }
}
