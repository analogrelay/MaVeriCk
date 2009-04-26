// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentitySource.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentitySource type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.ComponentModel;
using Maverick.Web.Helpers;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Maverick.Web.Identity {
    // TODO: Move ControllerContext to be a property? Do we need an IdentitySourceContext?
    [ContractType(MetadataViewType = typeof(ComponentMetadata))]
    public abstract class IdentitySource {
        
        private RouteCollection _routes;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consumers of this class may wish to completely replace the route collection.  Tests also replace this collection")]
        public RouteCollection Routes {
            get {
                if(_routes == null) {
                    _routes = RouteTable.Routes;
                }
                return _routes;
            }
            set { _routes = value; }
        }

        [Import]
        public ISessionIdentityManager SessionIdentityManager { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "The term Login is commonly used on the Web")]
        public abstract ActionResult Login(ControllerContext controllerContext, Uri returnUrl);

        public virtual Uri GetReturnUrl(ControllerContext context) {
            string returnUrl = context.HttpContext.Request.QueryString["returnUrl"];
            if(String.IsNullOrEmpty(returnUrl)) {
                return null;
            }
            return new Uri(returnUrl);
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Logout", Justification = "The term Logout is commonly used on the Web")]
        public virtual void Logout(ControllerContext controllerContext) {
            SessionIdentityManager.ClearSessionPrincipal();
        }

        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "The term Login is commonly used on the Web")]
        public virtual ActionResult OnLoginFormSubmit(ControllerContext context, Uri returnUrl) {
            return new EmptyResult();
        }

        public virtual void OnReturnFromProvider(ControllerContext context) {
        }

        protected internal virtual ActionResult ReturnToLastPage(ControllerContext context) {
            return new RedirectResult(GetReturnUrl(context).ToString());
        }

        protected internal virtual void SetSessionPrincipal(IClaimsPrincipal principal) {
            SessionIdentityManager.SetSessionPrincipal(new SessionSecurityToken(principal));
        }

        protected internal virtual Uri GenerateLandingUrl(ControllerContext context) {
            return GenerateLandingUrl(context, new RouteValueDictionary());
        }

        protected internal virtual Uri GenerateLandingUrl(ControllerContext context, object extraValues) {
            return GenerateLandingUrl(context, new RouteValueDictionary(extraValues));
        }

        protected internal virtual Uri GenerateLandingUrl(ControllerContext context, RouteValueDictionary values) {
            // Add the route values to direct the landing page back to the 
            values["controller"] = "Identity";
            values["action"] = "Land";
            values["id"] = IdentityServices.GetSourceName(this);
            values["page"] = null;

            // Get the virtual path from routing
            VirtualPathData pathData = Routes.GetVirtualPath(context.RequestContext, values);

            // Get the absolute virtual path
            string absolutePath = PathHelpers.GenerateClientUrl(context.HttpContext, pathData.VirtualPath);

            // Build the url
            Uri url = context.HttpContext.Request.Url;
            StringBuilder builder = new StringBuilder();
            
            Utilities.BuildUrlRoot(url, builder, !url.IsDefaultPort);

            builder.Append(absolutePath);

            return new Uri(builder.ToString());
        }

        protected internal static IClaimsPrincipal CreateSessionPrincipal(IEnumerable<Claim> claims) {
            IClaimsIdentity identity = new ClaimsIdentity(claims);

            // Create the session principal and return it
            return new ClaimsPrincipal(identity);
        }
    }
}
