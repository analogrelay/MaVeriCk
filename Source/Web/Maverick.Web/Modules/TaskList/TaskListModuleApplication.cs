// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TaskListModuleApplication.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the TaskListModuleApplication type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Web.Routing;
using Maverick.Web.ModuleFramework;

namespace Maverick.Web.Modules.TaskList {
    [Export(typeof(ModuleApplication))]
    [ModuleApplication(ApplicationId, ApplicationName)]
    public class TaskListModuleApplication : ModuleApplication {
        private const string ApplicationId = "3DF331C9-9D4F-4694-9917-1653BFA703FC";
        private const string ApplicationName = "TaskList";

        protected override string FolderPath {
            get { return ApplicationName; }
        }

        protected internal override void Init(MaverickApplication application) {
            base.Init(application);
            RegisterRoutes(Routes);
        }

        private static void RegisterRoutes(RouteCollection routes) {
            routes.RegisterDefaultRoute("Maverick.Web.Modules.TaskList.Controllers");
        }
    }
}