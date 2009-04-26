// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="RewrittenHttpContext.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the RewrittenHttpContext type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;

namespace Maverick.Web.Routing {
    public class RewrittenHttpContext : HttpContextBase {
        private readonly HttpContextBase _wrappedContext;
        private readonly HttpRequestBase _rewrittenRequest;

        public RewrittenHttpContext(HttpContextBase wrappedContext, string appRelativePath) {
            _wrappedContext = wrappedContext;
            _rewrittenRequest = new RewrittenHttpRequest(wrappedContext.Request, appRelativePath);
        }

        public override void AddError(Exception errorInfo) {
            _wrappedContext.AddError(errorInfo);
        }

        public override Exception[] AllErrors {
            get {
                return _wrappedContext.AllErrors;
            }
        }

        public override HttpApplicationStateBase Application {
            get {
                return _wrappedContext.Application;
            }
        }

        public override HttpApplication ApplicationInstance {
            get {
                return _wrappedContext.ApplicationInstance;
            }
            set {
                _wrappedContext.ApplicationInstance = value;
            }
        }

        public override Cache Cache {
            get {
                return _wrappedContext.Cache;
            }
        }

        public override void ClearError() {
            _wrappedContext.ClearError();
        }

        public override IHttpHandler CurrentHandler {
            get {
                return _wrappedContext.CurrentHandler;
            }
        }

        public override RequestNotification CurrentNotification {
            get {
                return _wrappedContext.CurrentNotification;
            }
        }

        public override bool Equals(object obj) {
            return Equals(this, obj);
        }

        public override Exception Error {
            get {
                return _wrappedContext.Error;
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.Web.HttpContextBase.GetGlobalResourceObject(System.String,System.String)", Justification = "This method is delegating to the same method on an adapted object")]
        public override object GetGlobalResourceObject(string classKey, string resourceKey) {
            return _wrappedContext.GetGlobalResourceObject(classKey, resourceKey);
        }

        public override object GetGlobalResourceObject(string classKey, string resourceKey, CultureInfo culture) {
            return _wrappedContext.GetGlobalResourceObject(classKey, resourceKey, culture);
        }

        public override int GetHashCode() {
            return _wrappedContext.GetHashCode() ^ _rewrittenRequest.GetHashCode();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1304:SpecifyCultureInfo", MessageId = "System.Web.HttpContextBase.GetLocalResourceObject(System.String,System.String)", Justification = "This method is delegating to the same method on an adapted object")]
        public override object GetLocalResourceObject(string virtualPath, string resourceKey) {
            return _wrappedContext.GetLocalResourceObject(virtualPath, resourceKey);
        }

        public override object GetLocalResourceObject(string virtualPath, string resourceKey, CultureInfo culture) {
            return _wrappedContext.GetLocalResourceObject(virtualPath, resourceKey, culture);
        }

        public override object GetSection(string sectionName) {
            return _wrappedContext.GetSection(sectionName);
        }

        public override object GetService(Type serviceType) {
            return _wrappedContext.GetService(serviceType);
        }

        public override IHttpHandler Handler {
            get {
                return _wrappedContext.Handler;
            }
            set {
                _wrappedContext.Handler = value;
            }
        }

        public override bool IsCustomErrorEnabled {
            get {
                return _wrappedContext.IsCustomErrorEnabled;
            }
        }

        public override bool IsDebuggingEnabled {
            get {
                return _wrappedContext.IsDebuggingEnabled;
            }
        }

        public override bool IsPostNotification {
            get {
                return _wrappedContext.IsPostNotification;
            }
        }

        public override IDictionary Items {
            get {
                return _wrappedContext.Items;
            }
        }

        public override IHttpHandler PreviousHandler {
            get {
                return _wrappedContext.PreviousHandler;
            }
        }

        public override ProfileBase Profile {
            get {
                return _wrappedContext.Profile;
            }
        }

        public override HttpRequestBase Request {
            get {
                return _rewrittenRequest;
            }
        }

        public override HttpResponseBase Response {
            get {
                return _wrappedContext.Response;
            }
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString) {
            _wrappedContext.RewritePath(filePath, pathInfo, queryString);
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath) {
            _wrappedContext.RewritePath(filePath, pathInfo, queryString, setClientFilePath);
        }

        public override void RewritePath(string path) {
            _wrappedContext.RewritePath(path);
        }

        public override void RewritePath(string path, bool rebaseClientPath) {
            _wrappedContext.RewritePath(path, rebaseClientPath);
        }

        public override HttpServerUtilityBase Server {
            get {
                return _wrappedContext.Server;
            }
        }

        public override HttpSessionStateBase Session {
            get {
                return _wrappedContext.Session;
            }
        }

        public override bool SkipAuthorization {
            get {
                return _wrappedContext.SkipAuthorization;
            }
            set {
                _wrappedContext.SkipAuthorization = value;
            }
        }

        public override DateTime Timestamp {
            get {
                return _wrappedContext.Timestamp;
            }
        }

        public override string ToString() {
            return _wrappedContext.ToString();
        }

        public override TraceContext Trace {
            get {
                return _wrappedContext.Trace;
            }
        }

        public override IPrincipal User {
            get {
                return _wrappedContext.User;
            }
            set {
                _wrappedContext.User = value;
            }
        }
    }
}
