// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="WSFederationIdentitySourceBaseTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the WSFederationIdentitySourceBaseTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.Web.Identity;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Maverick.Web.Tests.Identity {
    [TestClass]
    public class WSFederationIdentitySourceBaseTests {
        private static readonly Uri TestLandingUrl = new Uri("http://localhost/Zoop/Zork/Zoink");
        private static readonly Uri TestReturnUrl = new Uri("http://localhost/Foo/Bar/Baz");
        private static readonly Uri TestHomeRealm = new Uri("http://login.foo.example");
        private static readonly Uri TestIssuerUrl = new Uri("http://issuer.example");

        [TestMethod]
        public void DefaultHomeRealm_Is_Null() {
            // Arrange
            var mockIdentitySource = CreateIdentitySource();

            // Act
            string homeRealm = mockIdentitySource.Object.HomeRealm;

            // Assert
            Assert.IsNull(homeRealm);
        }

        [TestMethod]
        public void Login_Builds_RedirectResult_To_RequestUrl_Of_SignInMessage() {
            // Arrange
            var mockIdentitySource = CreateIdentitySource();
            mockIdentitySource.Setup(s => s.GenerateLandingUrl(It.IsAny<ControllerContext>(), It.IsAny<RouteValueDictionary>()))
                              .Returns(TestLandingUrl);

            // Not trying to test the Geneva framework, so we'll use AddParametersToSignInRequest to capture the 
            // SignInMessage's request url
            string expectedUrl = null;
            mockIdentitySource.Setup(s => s.AddParametersToSignInMessage(It.IsAny<SignInRequestMessage>()))
                              .Callback<SignInRequestMessage>(m => expectedUrl = m.RequestUrl);

            // Act
            ActionResult result = mockIdentitySource.Object.Login(Mockery.CreateMockControllerContext(), TestReturnUrl);
            
            // Assert
            ResultAssert.IsRedirect(result, expectedUrl);
        }

        [TestMethod]
        public void Login_Adds_Whr_Parameter_If_HomeRealm_Not_Null() {
            // Arrange
            var mockIdentitySource = CreateIdentitySource();
            mockIdentitySource.Setup(s => s.GenerateLandingUrl(It.IsAny<ControllerContext>(), It.IsAny<RouteValueDictionary>()))
                              .Returns(TestLandingUrl);
            mockIdentitySource.SetupGet(s => s.HomeRealm).Returns(TestHomeRealm.ToString());

            SignInRequestMessage message = null;
            mockIdentitySource.Setup(s => s.AddParametersToSignInMessage(It.IsAny<SignInRequestMessage>()))
                              .Callback<SignInRequestMessage>(m => message = m);

            // Act
            mockIdentitySource.Object.Login(Mockery.CreateMockControllerContext(), TestReturnUrl);

            // Assert
            Assert.AreEqual(TestHomeRealm, message.Parameters["whr"]);
        }

        [TestMethod]
        public void Login_Allows_Inheritors_To_Add_Parameters_To_SignInRequest() {
            // Arrange
            var mockIdentitySource = CreateIdentitySource();
            mockIdentitySource.Setup(s => s.GenerateLandingUrl(It.IsAny<ControllerContext>(), It.IsAny<RouteValueDictionary>()))
                              .Returns(TestLandingUrl);
            
            // Act
            mockIdentitySource.Object.Login(Mockery.CreateMockControllerContext(), TestReturnUrl);

            // Assert
            mockIdentitySource.Verify(s => s.AddParametersToSignInMessage(It.IsAny<SignInRequestMessage>()));
        }

        private static Mock<WSFederationIdentitySourceBase> CreateIdentitySource() {
            var mockIdentitySource = new Mock<WSFederationIdentitySourceBase> {CallBase = true};
            mockIdentitySource.SetupGet(s => s.IssuerUrl).Returns(TestIssuerUrl.ToString());
            return mockIdentitySource;
        }
    }
}
