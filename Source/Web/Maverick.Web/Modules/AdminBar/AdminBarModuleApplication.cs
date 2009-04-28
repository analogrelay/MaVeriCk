// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="AdminBarModuleApplication.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the AdminBarModuleApplication type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Web.Routing;
using Maverick.Web.ModuleFramework;

namespace Maverick.Web.Modules.AdminBar
{
    [Export(typeof(ModuleApplication))]
    [ModuleApplication(ApplicationId, ApplicationName)]
    public class AdminBarModuleApplication : ModuleApplication {
        private const string ApplicationId = "332883DC-9133-4333-9781-08CF6971C09C";
        private const string ApplicationName = "AdminBar";

        protected override string FolderPath {
            get { return "AdminBar"; }
        }

        protected internal override void Init(MaverickApplication application) {
            base.Init(application);
            RegisterRoutes(Routes);
        }

        private static void RegisterRoutes(RouteCollection routes) {
            routes.RegisterDefaultRoute("Maverick.Web.Modules.AdminBar.Controllers");
        }
    }
}