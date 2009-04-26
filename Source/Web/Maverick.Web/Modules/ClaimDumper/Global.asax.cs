// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Global.asax.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ClaimDumperModuleApplication type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Web.Routing;
using Maverick.Web.ModuleFramework;

namespace Maverick.Web.Modules.ClaimDumper
{
    [Export(typeof(ModuleApplication))]
    [ModuleApplication(ApplicationId, ApplicationName)]
    public class ClaimDumperModuleApplication : ModuleApplication {
        private const string ApplicationId = "8B09AEAE-C340-4FDD-8D52-7A360D925732";
        private const string ApplicationName = "Claim Dumper";

        protected override string FolderPath {
            get { return "ClaimDumper"; }
        }

        protected internal override void Init(MaverickApplication application) {
            base.Init(application);
            RegisterRoutes(Routes);
        }

        private static void RegisterRoutes(RouteCollection routes) {
            routes.RegisterDefaultRoute("Maverick.Web.Modules.ClaimDumper.Controllers");
        }
    }
}