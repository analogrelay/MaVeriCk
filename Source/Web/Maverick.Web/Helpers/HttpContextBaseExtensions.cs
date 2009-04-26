// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ContextManager.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ContextManager type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace Maverick.Web.Helpers {
    public static class HttpContextBaseExtensions {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is required and cannot be inferred")]
        public static bool HasPortalContext(this HttpContextBase httpContext) {
            string key = GetKeyFor<PortalRequestContext>();
            return httpContext.Items.Contains(key) && (httpContext.Items[key] != null);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is required and cannot be inferred")]
        public static PortalRequestContext GetPortalContext(this HttpContextBase httpContext) {
            string key = GetKeyFor<PortalRequestContext>();
            PortalRequestContext obj = httpContext.Items[key] as PortalRequestContext;
            if (obj == null) {
                obj = new PortalRequestContext(httpContext);
                httpContext.Items[key] = obj;
            }
            return obj;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is required and cannot be inferred")]
        internal static string GetKeyFor<T>() {
            return String.Concat("__MaverickContext:", typeof(T).FullName);
        }
    }
}