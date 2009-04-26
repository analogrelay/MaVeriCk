// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="WSFederationIdentitySourceBase.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the WSFederationIdentitySourceBase type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Web;

namespace Maverick.Web.Identity {
    public abstract class WSFederationIdentitySourceBase : IdentitySource {
        private const string HomeRealmParameter = "whr";

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "All of the places where this value is used require strings")]
        protected internal abstract string IssuerUrl { get; }

        protected internal virtual string HomeRealm {
            get { return null; }
        }

        public override ActionResult Login(ControllerContext controllerContext, Uri returnUrl) {
            Uri landingUrl = GenerateLandingUrl(controllerContext, new { returnUrl });
            WSFederationAuthenticationModule module = new WSFederationAuthenticationModule {
                Realm = landingUrl.ToString(),
                Issuer = IssuerUrl
            };

            SignInRequestMessage signInMessage = module.CreateSignInRequest(Guid.NewGuid().ToString(),
                                                                            landingUrl.ToString(),
                                                                            false);

            if (!String.IsNullOrEmpty(HomeRealm)) {
                signInMessage.Parameters.Add(HomeRealmParameter, HomeRealm);
            }

            AddParametersToSignInMessage(signInMessage);

            return new RedirectResult(signInMessage.RequestUrl);
        }

        protected internal virtual void AddParametersToSignInMessage(SignInRequestMessage signInMessage) { }
    }
}