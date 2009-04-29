// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CurrentTimeModuleApplication.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CurrentTimeModuleApplication type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Web.Routing;
using Maverick.Web.ModuleFramework;

namespace Maverick.Web.Modules.CurrentTime {
    [Export(typeof(ModuleApplication))]
    [ModuleApplication(ApplicationId, ApplicationName, "1.0.0.0", "Displays the Current Time", "Maverick", "~/Modules/CurrentTime/Content/Images/Icon.png")]
    public class CurrentTimeModuleApplication : ModuleApplication {
        private const string ApplicationId = "A1FE2A31-0BC9-4B12-9B81-3B75C098EB33";
        private const string ApplicationName = "Current Time";

        protected override string FolderPath {
            get { return "CurrentTime"; }
        }

        protected internal override void Init(MaverickApplication application) {
            base.Init(application);
            RegisterRoutes(Routes);
        }

        private static void RegisterRoutes(RouteCollection routes) {
            routes.RegisterDefaultRoute("Maverick.Web.Modules.CurrentTime.Controllers");
        }
    }
}