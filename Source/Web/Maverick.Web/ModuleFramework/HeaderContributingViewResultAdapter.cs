using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Maverick.Web.ModuleFramework {
    public class HeaderContributingViewResultAdapter : ActionResult, IHeaderContributingResult {
        public ViewResultBase InnerResult { get; private set; }

        public string HeaderViewNameFormat { get; set; }

        public HeaderContributingViewResultAdapter(ViewResultBase innerResult) {
            Arg.NotNull("innerResult", innerResult);
            InnerResult = innerResult;
            HeaderViewNameFormat = "{0}.header";
        }

        public override void ExecuteResult(ControllerContext context) {
            Arg.NotNull("context", context);
            InnerResult.ExecuteResult(context);
        }

        public void ExecuteHeader(ControllerContext context) {
            Arg.NotNull("context", context);
            ActionResult result = CreateHeaderResult(context, GetHeaderViewName(context));
            result.ExecuteResult(context);
        }
        
        protected internal virtual ActionResult CreateHeaderResult(ControllerContext context, string headerViewName) {
            Arg.NotNull("context", context);
            Arg.NotNullOrEmpty("headerViewName", headerViewName);

            ViewEngineResult viewEngineResult = InnerResult.ViewEngineCollection.FindPartialView(context, headerViewName);
            if (viewEngineResult.View == null) {
                // Unlike the normal view engine result, if there's no "header" view, we just silently fail and produce no output
                return new EmptyResult();
            }
            return new PartialViewResult() {
                TempData = InnerResult.TempData,
                ViewData = InnerResult.ViewData,
                ViewEngineCollection = InnerResult.ViewEngineCollection,
                ViewName = headerViewName,
                View = viewEngineResult.View
            };
        }

        protected internal virtual string GetHeaderViewName(ControllerContext context) {
            Arg.NotNull("context", context);
            string viewName = InnerResult.ViewName;
            if(String.IsNullOrEmpty(viewName)) {
                viewName = context.RouteData.GetRequiredString("action");
            }
            return String.Format(HeaderViewNameFormat, viewName);
        }
    }
}
