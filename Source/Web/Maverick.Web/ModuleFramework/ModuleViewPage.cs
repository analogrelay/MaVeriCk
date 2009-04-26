// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleViewPage.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleViewPage type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;
using Maverick.Web.Routing;

namespace Maverick.Web.ModuleFramework {
    // TODO: Is there a better way to organize these to reduce duplication?
    public class ModuleViewPage : ViewPage {
        public override void InitHelpers() {
            ModuleRequestResult app = ViewContext.HttpContext.GetActiveModule();
            if(app == null) {
                base.InitHelpers();
            }

            Ajax = new AjaxHelper(ViewContext, this, ModuleRouteRewriter.CreateRouteCollection(app.Application.Routes));
            Html = new HtmlHelper(ViewContext, this, ModuleRouteRewriter.CreateRouteCollection(app.Application.Routes));
            Url = new UrlHelper(ViewContext.RequestContext, ModuleRouteRewriter.CreateRouteCollection(app.Application.Routes));
        }
    }

    public class ModuleViewPage<T> : ViewPage<T> where T : class {
        public override void InitHelpers() {
            ModuleRequestResult app = ViewContext.HttpContext.GetActiveModule();
            if (app == null) {
                base.InitHelpers();
            }

            Ajax = new AjaxHelper<T>(ViewContext, this, ModuleRouteRewriter.CreateRouteCollection(app.Application.Routes));
            Html = new HtmlHelper<T>(ViewContext, this, ModuleRouteRewriter.CreateRouteCollection(app.Application.Routes));
            Url = new UrlHelper(ViewContext.RequestContext, ModuleRouteRewriter.CreateRouteCollection(app.Application.Routes));
        }
    }
}
