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
            HeaderViewNameFormat = "{0}.import";
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

            return new PartialViewResult() {
                TempData = InnerResult.TempData,
                ViewData = InnerResult.ViewData,
                ViewEngineCollection = InnerResult.ViewEngineCollection,
                ViewName = headerViewName
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
