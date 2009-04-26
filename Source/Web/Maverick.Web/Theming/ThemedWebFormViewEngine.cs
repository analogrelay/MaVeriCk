// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ThemedWebFormViewEngine.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ThemedWebFormViewEngine type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Maverick.Web.Helpers;

namespace Maverick.Web.Theming {
    // Inspired by KonaViewEngine from the MVC Storefront sample which is licensed under the Ms-PL
    // See: http://mvcsamples.codeplex.com/
    // Also based on code from VirtualPathProviderViewEngine from (Ms-PL licensed) ASP.Net MVC Framework Source Code
    // TODO: Caching
    // TODO: Testing (this is a Spike at the moment)
    public class ThemedWebFormViewEngine : WebFormViewEngine {
        public ThemedWebFormViewEngine() {
            ViewLocationFormats = new[] {
                "~/Views/Themes/{0}/{1}/{2}.aspx",
                "~/Views/Themes/{0}/Shared/{2}.aspx",
                "~/Views/Themes/{0}/{1}/{2}.ascx",
                "~/Views/Themes/{0}/Shared/{2}.ascx",
                "~/Views/Themes/_Default/{1}/{2}.aspx",
                "~/Views/Themes/_Default/Shared/{2}.aspx",
                "~/Views/Themes/_Default/{1}/{2}.ascx",
                "~/Views/Themes/_Default/Shared/{2}.ascx"
            };
            MasterLocationFormats = new[] {
                "~/Views/Themes/{0}/{1}/{2}.master",
                "~/Views/Themes/{0}/Shared/{2}.master",
                "~/Views/Themes/_Default/{1}/{2}.master",
                "~/Views/Themes/_Default/Shared/{2}.master",
            };
            PartialViewLocationFormats = ViewLocationFormats;
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache) {
            Arg.NotNull("controllerContext", controllerContext);
            Arg.NotNullOrEmpty("viewName", partialViewName);

            PortalRequestContext requestContext = controllerContext.HttpContext.GetPortalContext();

            IList<string> searchedLocations = new List<string>();
            string viewPath = GetPath(controllerContext, ViewLocationFormats, partialViewName, requestContext.CurrentTheme, searchedLocations);
            if (String.IsNullOrEmpty(viewPath)) {
                return new ViewEngineResult(searchedLocations);
            }
            return new ViewEngineResult(CreatePartialView(controllerContext, viewPath), this);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache) {
            Arg.NotNull("controllerContext", controllerContext);
            Arg.NotNullOrEmpty("viewName", viewName);

            PortalRequestContext requestContext = controllerContext.HttpContext.GetPortalContext();

            IList<string> searchedLocations = new List<string>();
            string viewPath = GetPath(controllerContext, ViewLocationFormats, viewName, requestContext.CurrentTheme, searchedLocations);
            string masterPath = String.Empty;
            if(!String.IsNullOrEmpty(masterName)) {
                masterPath = GetPath(controllerContext, MasterLocationFormats, masterName, requestContext.CurrentTheme, searchedLocations);
            }
            if(String.IsNullOrEmpty(viewPath) || (String.IsNullOrEmpty(masterPath) && !String.IsNullOrEmpty(masterName))) {
                return new ViewEngineResult(searchedLocations);
            }
            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
        }

        private string GetPath(ControllerContext context, string[] locationFormats, string name, string theme, IList<string> searchedLocations) {
            Arg.NotNull("context", context);
            Arg.NotNull("locationFormats", locationFormats);
            Arg.NotNullOrEmpty("name", name);
            Arg.NotNull("searchedLocations", searchedLocations);

            if(IsSpecificPath(name)) {
                return GetSpecificPath(context, name, searchedLocations);
            }
            return GetGeneralPath(context, name, theme, locationFormats, searchedLocations);
        }

        private string GetGeneralPath(ControllerContext context, string name, string theme, string[] locationFormats, IList<string> searchedLocations) {
            string controllerName = context.RouteData.GetRequiredString("controller");
            foreach(string format in locationFormats) {
                string path = String.Format(CultureInfo.InvariantCulture, format, theme, controllerName, name);
                if(FileExists(context, path)) {
                    return path;
                }
                searchedLocations.Add(path);
            }
            return String.Empty;
        }

        private string GetSpecificPath(ControllerContext context, string name, IList<string> searchedLocations) {
            if(!FileExists(context, name)) {
                searchedLocations.Add(name);
                return String.Empty;
            }
            return name;
        }

        public static bool IsSpecificPath(string name) {
            return name.Length > 0 && name[0] == '~' || name[0] == '/';
        }
    }
}
