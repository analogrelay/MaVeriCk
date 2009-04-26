// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="AzureAcsLiveIdentitySource.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the AzureAcsLiveIdentitySource type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Globalization;
using Maverick.Web.Properties;

namespace Maverick.Web.Identity {
    [Export(typeof(IdentitySource))]
    [IdentitySource("AzureAcsLiveId", "0.1.0.0")]
    public class AzureAcsLiveIdentitySource : WSFederationIdentitySourceBase {
        public const string AcsSolutionContractName = "Maverick.Web.Identity.AcsSolutionName";
        private const string IssuerUrlFormat = "https://{0}.accesscontrol.windows.net/passivests/livefederation.aspx";
        private const string LiveIdHomeRealm = "http://login.live.com";
        
        [Import(AcsSolutionContractName)]
        public string AcsSolutionName { get; set; }

        protected internal override string IssuerUrl {
            get {
                Guard.Against(String.IsNullOrEmpty(AcsSolutionName), Resources.Error_NoACSSolution);
                return String.Format(CultureInfo.InvariantCulture, 
                                     IssuerUrlFormat, 
                                     AcsSolutionName);
            }
        }

        protected internal override string HomeRealm {
            get { return LiveIdHomeRealm; }
        }
    }
}
