// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ResultAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ResultAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Web.Tests {
    public static class ResultAssert {
        private class ObjectEqualityComparer<T> : IEqualityComparer<T> where T : class {
            public bool Equals(T x, T y) {
                return (x != null) && x.Equals(y);
            }

            public int GetHashCode(T obj) {
                return obj.GetHashCode();
            }
        }

        public static void IsRedirectToRoute(ActionResult result) {
            result.AssertCast<RedirectToRouteResult>();
        }

        public static void IsRedirectToRoute(ActionResult result, object routeValues) {
            IsRedirectToRoute(result, new RouteValueDictionary(routeValues));
        }

        public static RedirectToRouteResult IsRedirectToRoute(ActionResult result, RouteValueDictionary routeValues) {
            RedirectToRouteResult routeResult = result.AssertCast<RedirectToRouteResult>();
            IsRedirectToRoute(routeResult);
            DictionaryAssert.ContainsEntries(routeValues, routeResult.RouteValues);
            return routeResult;
        }

        public static void IsRedirectToRoute(ActionResult result, string routeName) {
            RedirectToRouteResult routeResult = result.AssertCast<RedirectToRouteResult>();
            IsRedirectToRoute(routeResult);
            Assert.AreEqual(routeName, routeResult.RouteName);
        }

        public static void IsRedirectToRoute(ActionResult result, string routeName, object routeValues) {
            IsRedirectToRoute(result, routeName, new RouteValueDictionary(routeValues));
        }


        public static void IsRedirectToRoute(ActionResult result, string routeName, RouteValueDictionary routeValues) {
            IsRedirectToRoute(result, routeValues);
            IsRedirectToRoute(result, routeName);
        }

        public static void IsRedirectToAction(ActionResult result, string actionName) {
            IsRedirectToRoute(result, new {action = actionName});
        }

        public static void IsRedirectToAction(ActionResult result, string actionName, string controllerName) {
            IsRedirectToRoute(result, new {action = actionName, controller = controllerName});
        }

        public static void IsViewWithModel<T>(ActionResult result, T expectedModel) where T : class {
            IsViewWithModel(result, String.Empty, String.Empty, expectedModel, new ObjectEqualityComparer<T>());
        }

        public static void IsViewWithModel<T>(ActionResult result, T expectedModel, IEqualityComparer<T> comparer) where T : class {
            IsViewWithModel(result, String.Empty, String.Empty, expectedModel, comparer);
        }

        public static void IsViewWithModel<T>(ActionResult result, Action<T> modelAsserts) where T : class {
            IsViewWithModel(result, String.Empty, String.Empty, modelAsserts);
        }

        public static void IsViewWithModel<T>(ActionResult result, string viewName, T expectedModel) where T : class {
            IsViewWithModel(result, viewName, String.Empty, expectedModel, new ObjectEqualityComparer<T>());
        }

        public static void IsViewWithModel<T>(ActionResult result, string viewName, T expectedModel, IEqualityComparer<T> comparer) where T : class {
            IsViewWithModel(result, viewName, String.Empty, expectedModel, comparer);
        }

        public static void IsViewWithModel<T>(ActionResult result, string viewName, Action<T> modelAsserts) where T : class {
            IsViewWithModel(result, viewName, String.Empty, modelAsserts);
        }

        public static void IsViewWithModel<T>(ActionResult result, string viewName, string masterName, T expectedModel) where T : class {
            IsViewWithModel(result, viewName, masterName, expectedModel, new ObjectEqualityComparer<T>());
        }

        public static void IsViewWithModel<T>(ActionResult result, string viewName, string masterName, T expectedModel, IEqualityComparer<T> comparer) where T : class {
            IsViewWithModel<T>(result, 
                               viewName, 
                               masterName, 
                               t => Assert.IsTrue(comparer.Equals(expectedModel, t), 
                                                  "Expected that the view model would equal the expected model"));            
        }

        public static void IsViewWithModel<T>(ActionResult result, string viewName, string masterName, Action<T> modelAsserts) where T : class {
            IsView(result, viewName, masterName);

            ViewResult viewResult = result.AssertCast<ViewResult>();
            T model = viewResult.ViewData.Model as T;
            Assert.IsNotNull(model, "Expected that the view model would be an object of type {0}", typeof(T).FullName);
            modelAsserts(model);
        }

        public static void IsView(ActionResult result, string viewName) {
            IsView(result, viewName, String.Empty, new RouteValueDictionary());
        }

        public static void IsView(ActionResult result, string viewName, string masterName) {
            IsView(result, viewName, masterName, new RouteValueDictionary());
        }

        public static void IsView(ActionResult result, object expectedViewData) {
            IsView(result, String.Empty, String.Empty, new RouteValueDictionary(expectedViewData));
        }

        public static void IsView(ActionResult result, RouteValueDictionary expectedViewData) {
            IsView(result, String.Empty, String.Empty, expectedViewData);
        }

        public static void IsView(ActionResult result, string viewName, object expectedViewData) {
            IsView(result, viewName, String.Empty, new RouteValueDictionary(expectedViewData));
        }

        public static void IsView(ActionResult result, string viewName, RouteValueDictionary expectedViewData) {
            IsView(result, viewName, String.Empty, expectedViewData);
        }

        public static void IsView(ActionResult result, string viewName, string masterName, object expectedViewData) {
            IsView(result, viewName, masterName, new RouteValueDictionary(expectedViewData));
        }

        public static void IsView(ActionResult result, string viewName, string masterName, RouteValueDictionary expectedViewData) {
            ViewResult viewResult = result.AssertCast<ViewResult>();
            StringsEqualOrBothNullOrEmpty(viewName, viewResult.ViewName, "Expected that the view result would be for the {0} view", "default");
            StringsEqualOrBothNullOrEmpty(viewName, viewResult.ViewName, "Expected that the master view for the view result would be the {0} master", "default");

            DictionaryAssert.ContainsEntries(expectedViewData, viewResult.ViewData);
        }

        public static void IsRedirect(ActionResult result, string url) {
            RedirectResult redirectResult = result.AssertCast<RedirectResult>();
            Assert.AreEqual(url, redirectResult.Url);
        }

        public static void IsResourceNotFound(ActionResult result) {
            IsResourceNotFound(result, innerResult => Assert.IsNull(innerResult, 
                                                                    "Expected that the default inner result would be used"));
        }

        public static void IsResourceNotFound(ActionResult result, Action<ActionResult> innerResultAssert) {
            ResourceNotFoundResult notFoundResult = result.AssertCast<ResourceNotFoundResult>();
            innerResultAssert(notFoundResult.InnerResult);
        }

        public static void IsResourceNotFound(ActionResult result, string viewName) {
            ResourceNotFoundResult notFoundResult = result.AssertCast<ResourceNotFoundResult>();
            IsView(notFoundResult.InnerResult, viewName);
        }

        public static void IsResourceNotFound(ActionResult result, string viewName, string masterName) {
            ResourceNotFoundResult notFoundResult = result.AssertCast<ResourceNotFoundResult>();
            IsView(notFoundResult.InnerResult, viewName, masterName);
        }

        public static void IsEmpty(ActionResult result) {
            Assert.IsInstanceOfType(result, typeof(EmptyResult));
        }

        private static void StringsEqualOrBothNullOrEmpty(string expected, string actual, string messageFormat, string bothEmptyParameter) {
            if (String.IsNullOrEmpty(expected)) {
                Assert.IsTrue(String.IsNullOrEmpty(actual), messageFormat, bothEmptyParameter);
            }
            else {
                Assert.AreEqual(expected,
                                actual,
                                messageFormat,
                                expected);
            }
        }

    }
}
