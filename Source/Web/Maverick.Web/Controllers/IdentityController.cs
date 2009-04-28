// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentityController.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentityController type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.ComponentModel;
using Maverick.Models;
using Maverick.Web.Identity;
using Maverick.Web.Models;

namespace Maverick.Web.Controllers {
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class IdentityController : MaverickController {
        private const string DefaultControllerName = "Page";
        private const string DefaultActionName = "View";

        [Import]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "The composition engine must be able to set this property")]
        public ComponentCollection<IdentitySource> IdentitySources { get; set; }

        [Import]
        public ISessionIdentityManager SessionIdentityManager { get; set; }

        public IdentityController() {
            IdentitySources = new ComponentCollection<IdentitySource>();
        }
        
        [ValidateInput(false)]
        public ActionResult Land(string id) {
            // "id" is the name of the identity source
            // TODO: Refactor this into a Model Binder to reduce duplication
            IdentitySource identitySource = GetIdentitySource(id);
            if(identitySource == null) {
                return ResourceNotFound();
            }

            identitySource.OnReturnFromProvider(ControllerContext);

            Uri returnUrl = identitySource.GetReturnUrl(ControllerContext);
            if (returnUrl == null) {
                return new RedirectToRouteResult(new RouteValueDictionary(new {
                    page = (Page)null,
                    controller = DefaultControllerName,
                    action = DefaultActionName
                }));
            }
            return new RedirectResult(returnUrl.ToString());
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "The term Login is commonly used on the Web")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "The value of returnUrl is provided by the user and may be malformed")]
        [ActionName("Login")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoginGet(string id, string returnUrl) {
            // "id" is the name of the identity source
            IdentitySource identitySource = GetIdentitySource(id);
            if (identitySource == null) {
                return ResourceNotFound();
            }

            return TransformActionResult(id, identitySource.Login(ControllerContext, ToUriOrNull(returnUrl)));
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "The term Login is commonly used on the Web")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "The value of returnUrl is provided by the user and may be malformed")]
        [ActionName("Login")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LoginPost(string id, string returnUrl) {
            // "id" is the name of the identity source
            IdentitySource identitySource = GetIdentitySource(id);
            if(identitySource == null) {
                return ResourceNotFound();
            }

            return TransformActionResult(id, identitySource.OnLoginFormSubmit(ControllerContext, ToUriOrNull(returnUrl)));
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Logout", Justification = "The term Logout is commonly used on the Web")]
        public ActionResult Logout() {
            IdentitySource identitySource = null;
            
            // If there's an "authenticated-by" claim in the token, we can use that to identify the identity source
            UserIdentity identity = Thread.CurrentPrincipal.AsUserIdentity();
            
            if (!String.IsNullOrEmpty(identity.IdentifiedBy)) {
                identitySource = GetIdentitySource(identity.IdentifiedBy);
            }

            // Remove the session token
            SessionIdentityManager.ClearSessionPrincipal();

            // If we found an identity source, give it a chance to log out
            if(identitySource != null) {
                identitySource.Logout(ControllerContext);
            }
            
            return RedirectToAction("View", "Page", new {page = (Page)null});
        }

        private IdentitySource GetIdentitySource(string name) {
            if (String.IsNullOrEmpty(name)) {
                return null;
            }
            return (from source in IdentitySources
                    where String.Equals(source.MetadataView.Name, name, StringComparison.OrdinalIgnoreCase)
                    select source.GetExportedObject()).SingleOrDefault();
        }

        private static ActionResult TransformActionResult(string identitySourceName, ActionResult result) {
            ViewResult viewResult = result as ViewResult;
            if (viewResult != null) {
                viewResult.MasterName = "Site";
                if (viewResult.ViewName[0] != '~') {
                    viewResult.ViewName = String.Format(CultureInfo.InvariantCulture, 
                                                        "{0}/{1}", 
                                                        identitySourceName, 
                                                        viewResult.ViewName);
                }
                return viewResult;
            }
            return result;
        }

        internal static Uri ToUriOrNull(string returnUrl) {
            Uri uri;
            if (Uri.TryCreate(returnUrl, UriKind.RelativeOrAbsolute, out uri)) {
                return uri;
            }
            return null;
        }
    }
}
