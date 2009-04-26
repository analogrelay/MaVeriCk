// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="AzureAcsLiveIdentitySourceTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the AzureAcsLiveIdentitySourceTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Maverick.Web.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Web.Tests.Identity {
    [TestClass]
    public class AzureAcsLiveIdentitySourceTests {
        private const string ExpectedIssuerUrl = "https://foobar.accesscontrol.windows.net/passivests/livefederation.aspx";
        private const string ExpectedHomeRealm = "http://login.live.com";

        [TestMethod]
        public void IssuerUrl_Is_Azure_AccessControlService_PassiveSTS_For_LiveIDFederation_Using_Current_Solution_Name() {
            // Arrange
            AzureAcsLiveIdentitySource identitySource = new AzureAcsLiveIdentitySource {
                AcsSolutionName = "foobar"
            };

            // Act
            string issuerUrl = identitySource.IssuerUrl;

            // Assert
            Assert.AreEqual(ExpectedIssuerUrl, issuerUrl);
        }

        [TestMethod]
        public void HomeRealm_Is_Windows_Live_ID_Realm() {
            // Arrange
            AzureAcsLiveIdentitySource identitySource = new AzureAcsLiveIdentitySource {
                AcsSolutionName = "foobar"
            };

            // Act
            string homeRealm = identitySource.HomeRealm;

            // Assert
            Assert.AreEqual(ExpectedHomeRealm, homeRealm);
        }

        [TestMethod]
        public void AcsSolutionName_Is_Imported_From_CompositionContainer() {
            // Assert
            CompositionAssert.IsImported<AzureAcsLiveIdentitySource>(o => o.AcsSolutionName,
                                                                     AzureAcsLiveIdentitySource.AcsSolutionContractName);
        }

        [TestMethod]
        public void AzureAcsLiveIdentitySource_Is_IdentitySource_With_Name_AzureAcsLiveId() {
            // Assert
            MiscAssert.HasIdentitySourceName(typeof(AzureAcsLiveIdentitySource), "AzureAcsLiveId");
        }
    }
}
