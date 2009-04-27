// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DebugIdentitySourceTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DebugIdentitySourceTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;
using Maverick.Web.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Claims;

using SysClaimTypes = System.IdentityModel.Claims.ClaimTypes;

namespace Maverick.Web.Tests.Identity {
    [TestClass]
    public class DebugIdentitySourceTests {
        private static readonly Uri TestReturnUrl = new Uri("http://www.test.example");

        [TestMethod]
        public void Login_Requires_Non_Null_ControllerContext() {
            AutoTester.ArgumentNull<ControllerContext>(marker => new DebugIdentitySource().Login(marker, TestReturnUrl));
        }

        [TestMethod]
        public void Login_Returns_Result_Of_RedirectToLastPage() {
            // Arrange
            ControllerContext context = Mockery.CreateMockControllerContext();
            DebugIdentitySource identitySource = CreateIdentitySource();
            ActionResult expected = SetupMockLastPageResult(identitySource, context);

            // Act
            ActionResult actual = identitySource.Login(context, TestReturnUrl);

            // Assert
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void Login_SetsSessionPrincipal_To_User_With_Test_Name_And_SuperUser_Role() {
            // Arrange
            ControllerContext context = Mockery.CreateMockControllerContext();
            DebugIdentitySource identitySource = CreateIdentitySource();
            SetupMockLastPageResult(identitySource, context);

            IClaimsPrincipal actual = null;
            Mock.Get(identitySource.SessionIdentityManager)
                .Setup(s => s.SetSessionPrincipal(It.IsAny<SessionSecurityToken>()))
                .Callback<SessionSecurityToken>(t => actual = t.ClaimsPrincipal);

            // Act
            identitySource.Login(context, TestReturnUrl);

            // Assert
            Assert.IsNotNull(actual);
            ClaimsAssert.HasClaim(actual.Identities[0].Claims, SysClaimTypes.Name, "Maverick Developer");
            ClaimsAssert.HasClaim(actual.Identities[0].Claims, ClaimTypes.Role, "SuperUser");
        }

        private static DebugIdentitySource CreateIdentitySource() {
            var mockIdentitySource = new Mock<DebugIdentitySource>() { CallBase = true };
            DebugIdentitySource identitySource = mockIdentitySource.Object;
            identitySource.SessionIdentityManager = new Mock<ISessionIdentityManager>().Object;
            return identitySource;
        }

        private static ActionResult SetupMockLastPageResult(DebugIdentitySource identitySource, ControllerContext context) {
            ActionResult expected = new EmptyResult();
            Mock.Get(identitySource).Setup(i => i.ReturnToLastPage(context)).Returns(expected);
            return expected;
        }
    }
}
