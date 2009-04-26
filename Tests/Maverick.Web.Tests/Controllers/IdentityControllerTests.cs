// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentityControllerTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentityControllerTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using Maverick.Models;
using Maverick.Web.Controllers;
using Maverick.Web.Identity;
using Microsoft.IdentityModel.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Web.Tests.Controllers {
    [TestClass]
    public class IdentityControllerTests {
        private static readonly Uri TestReturnUrl = new Uri("http://www.test.example/");
        private const string UnusedMockIdentitySource = "Foo";
        private const string SelectedMockIdentitySource = "Bar";
        private const string TestViewName = "Foo";
        private const string ExpectedViewName = "Bar/Foo";
        private const string ExpectedMasterName = "Site";
        private const string TestExplicitViewName = "~/Zoop/Zork/Zoink";
        private const string TestUrl = "http://localhost/";
        private const string TestInvalidUrl = "http://localhost:foo/";

        [TestMethod]
        public void Controller_Is_Exported_To_CompositionContainer() {
            // Assert
            CompositionAssert.IsExported(typeof(IdentityController));
        }

        [TestMethod]
        public void Controller_Is_Exported_With_NonShared_CreationPolicy() {
            // Assert
            CompositionAssert.HasCreationPolicy(typeof(IdentityController), CreationPolicy.NonShared);
        }

        [TestMethod]
        public void IdentitySources_Are_Imported_From_CompositionContainer() {
            // Assert
            CompositionAssert.IsImported<IdentityController>(id => id.IdentitySources);
        }

        [TestMethod]
        public void SessionIdentityManager_Is_Imported_From_CompositionContainer() {
            // Assert
            CompositionAssert.IsImported<IdentityController>(id => id.SessionIdentityManager);
        }

        [TestMethod]
        public void Land_Does_Not_Validate_Input() {
            // Assert
            ActionFilterAssert.DoesNotValidateInput<IdentityController>(id => id.Land("foo"));
        }

        [TestMethod]
        public void LoginGet_Is_Action_Named_Login() {
            // Assert
            ActionFilterAssert.HasActionName<IdentityController>(id => id.LoginGet("foo", "bar"), "Login");
        }

        [TestMethod]
        public void LoginGet_Only_Accepts_Get_Verb() {
            // Assert
            ActionFilterAssert.AcceptsVerb<IdentityController>(id => id.LoginGet("foo", "bar"), HttpVerbs.Get);
        }

        [TestMethod]
        public void LoginPost_Is_Action_Named_Login() {
            // Assert
            ActionFilterAssert.HasActionName<IdentityController>(id => id.LoginPost("foo", "bar"), "Login");
        }

        [TestMethod]
        public void LoginPost_Only_Accepts_Post_Verb() {
            // Assert
            ActionFilterAssert.AcceptsVerb<IdentityController>(id => id.LoginPost("foo", "bar"), HttpVerbs.Post);
        }

        [TestMethod]
        public void Land_Calls_OnReturnFromProvider_With_ControllerContext_If_IdentitySource_Found() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            // Act
            controller.Land(SelectedMockIdentitySource);

            // Assert
            mockSource.Verify(s => s.OnReturnFromProvider(controller.ControllerContext));
        }

        [TestMethod]
        public void Land_Calls_GetReturnUrl_With_ControllerContext_If_IdentitySource_Found() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            // Act
            controller.Land(SelectedMockIdentitySource);

            // Assert
            mockSource.Verify(s => s.GetReturnUrl(controller.ControllerContext));
        }

        [TestMethod]
        public void Land_Redirects_To_HomePage_If_No_Return_Url() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            // Act
            ActionResult result = controller.Land(SelectedMockIdentitySource);

            // Assert
            ResultAssert.IsRedirectToRoute(result, new {controller = "Page", action = "View", page = (Page)null});
        }

        [TestMethod]
        public void Land_Redirects_To_ReturnUrl_If_Present() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);
            mockSource.Setup(s => s.GetReturnUrl(It.IsAny<ControllerContext>()))
                      .Returns(TestReturnUrl);

            // Act
            ActionResult result = controller.Land(SelectedMockIdentitySource);

            // Assert
            ResultAssert.IsRedirect(result, TestReturnUrl.ToString());
        }

        [TestMethod]
        public void LoginGet_Calls_Login_With_ControllerContext_And_ReturnUrl_If_IdentitySource_Found() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            // Act
            controller.LoginGet(SelectedMockIdentitySource, TestReturnUrl.ToString());

            // Assert
            mockSource.Verify(s => s.Login(controller.ControllerContext, TestReturnUrl));
        }

        [TestMethod]
        public void LoginGet_Returns_Result_Of_IdentitySource_Login() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            EmptyResult expectedResult = new EmptyResult();
            mockSource.Setup(s => s.Login(It.IsAny<ControllerContext>(), TestReturnUrl))
                      .Returns(expectedResult);

            // Act
            ActionResult actualResult = controller.LoginGet(SelectedMockIdentitySource, TestReturnUrl.ToString());

            // Assert
            Assert.AreSame(expectedResult, actualResult, "Expected that the result of Login would be passed unmodified");
        }

        [TestMethod]
        public void LoginGet_Alters_Master_And_View_Name_If_Result_Is_ViewResult() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            mockSource.Setup(s => s.Login(It.IsAny<ControllerContext>(), TestReturnUrl))
                      .Returns(new ViewResult {ViewName = TestViewName});

            // Act
            ActionResult actualResult = controller.LoginGet(SelectedMockIdentitySource, TestReturnUrl.ToString());

            // Assert
            ResultAssert.IsView(actualResult, ExpectedViewName, ExpectedMasterName);
        }

        [TestMethod]
        public void LoginGet_Does_Not_Alter_ViewName_If_It_Starts_With_Tilde() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            mockSource.Setup(s => s.Login(It.IsAny<ControllerContext>(), TestReturnUrl))
                      .Returns(new ViewResult { ViewName = TestExplicitViewName });

            // Act
            ActionResult actualResult = controller.LoginGet(SelectedMockIdentitySource, TestReturnUrl.ToString());

            // Assert
            ResultAssert.IsView(actualResult, TestExplicitViewName, ExpectedMasterName);
        }

        [TestMethod]
        public void LoginPost_Calls_OnLoginFormSubmit_If_IdentitySource_Found() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            // Act
            ActionResult actualResult = controller.LoginPost(SelectedMockIdentitySource, TestReturnUrl.ToString());

            // Assert
            mockSource.Verify(s => s.OnLoginFormSubmit(controller.ControllerContext, TestReturnUrl));
        }

        [TestMethod]
        public void LoginPost_Returns_Result_Of_OnLoginFormSubmit() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            EmptyResult expectedResult = new EmptyResult();
            mockSource.Setup(s => s.OnLoginFormSubmit(It.IsAny<ControllerContext>(), TestReturnUrl))
                      .Returns(expectedResult);

            // Act
            ActionResult actualResult = controller.LoginPost(SelectedMockIdentitySource, TestReturnUrl.ToString());

            // Assert
            Assert.AreSame(expectedResult, actualResult);
        }

        [TestMethod]
        public void LoginPost_Alters_Master_And_View_Name_If_Result_Is_ViewResult() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            mockSource.Setup(s => s.OnLoginFormSubmit(It.IsAny<ControllerContext>(), TestReturnUrl))
                      .Returns(new ViewResult { ViewName = TestViewName });

            // Act
            ActionResult actualResult = controller.LoginPost(SelectedMockIdentitySource, TestReturnUrl.ToString());

            // Assert
            ResultAssert.IsView(actualResult, ExpectedViewName, ExpectedMasterName);
        }

        [TestMethod]
        public void LoginPost_Does_Not_Alter_ViewName_If_It_Starts_With_Tilde() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            mockSource.Setup(s => s.OnLoginFormSubmit(It.IsAny<ControllerContext>(), TestReturnUrl))
                      .Returns(new ViewResult { ViewName = TestExplicitViewName });

            // Act
            ActionResult actualResult = controller.LoginPost(SelectedMockIdentitySource, TestReturnUrl.ToString());

            // Assert
            ResultAssert.IsView(actualResult, TestExplicitViewName, ExpectedMasterName);
        }

        [TestMethod]
        public void Logout_Removes_Session_Token() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSessionManager = new Mock<ISessionIdentityManager>();
            controller.SessionIdentityManager = mockSessionManager.Object;

            // Act
            controller.Logout();

            // Assert
            mockSessionManager.Verify(s => s.ClearSessionPrincipal());
        }

        [TestMethod]
        public void Logout_Redirects_To_Home_Page() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSessionManager = new Mock<ISessionIdentityManager>();
            controller.SessionIdentityManager = mockSessionManager.Object;

            // Act
            ActionResult result = controller.Logout();

            // Assert
            ResultAssert.IsRedirectToRoute(result, new {controller = "Page", action = "View", page = (Page)null});
        }

        [TestMethod]
        public void Logout_Calls_IdentitySource_To_Logout_If_Token_Contains_IdentifiedBy_Claim_Matching_Valid_IdentitySource() {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);

            var mockSource = controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            var mockSessionManager = new Mock<ISessionIdentityManager>();
            controller.SessionIdentityManager = mockSessionManager.Object;

            Claim claim = new Claim(MaverickClaimTypes.IdentifiedBy, SelectedMockIdentitySource);
            IPrincipal oldPrincipal = Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claim));

            // Act
            controller.Logout();

            // Assert
            mockSource.Verify(s => s.Logout(controller.ControllerContext));

            // Cleanup
            Thread.CurrentPrincipal = oldPrincipal;
        }

        [TestMethod]
        public void Land_Returns_EmptyResult_If_No_IdentitySource_With_Provided_Name() {
            TestExpectedNotFoundResult(c => c.Land("Baz"));
        }

        [TestMethod]
        public void Land_Returns_EmptyResult_If_Name_Empty() {
            TestExpectedNotFoundResult(c => c.Land(String.Empty));
        }

        [TestMethod]
        public void LoginGet_Returns_EmptyResult_If_No_IdentitySource_With_Provided_Name() {
            TestExpectedNotFoundResult(c => c.LoginGet("Baz", TestReturnUrl.ToString()));
        }

        [TestMethod]
        public void LoginGet_Returns_EmptyResult_If_Name_Empty() {
            TestExpectedNotFoundResult(c => c.LoginGet(String.Empty, TestReturnUrl.ToString()));
        }

        [TestMethod]
        public void LoginPost_Returns_EmptyResult_If_No_IdentitySource_With_Provided_Name() {
            TestExpectedNotFoundResult(c => c.LoginPost("Baz", TestReturnUrl.ToString()));
        }

        [TestMethod]
        public void LoginPost_Returns_EmptyResult_If_Name_Empty() {
            TestExpectedNotFoundResult(c => c.LoginPost(String.Empty, TestReturnUrl.ToString()));
        }

        [TestMethod]
        public void ToUriOrNull_Returns_Uri_If_String_Is_Valid_Uri() {
            // Act
            Uri url = IdentityController.ToUriOrNull(TestUrl);

            // Assert
            Assert.AreEqual(TestUrl, url.ToString());
        }

        [TestMethod]
        public void ToUriOrNull_Returns_Null_If_String_Is_Not_Valid_Uri() {
            // Act
            Uri url = IdentityController.ToUriOrNull(TestInvalidUrl);

            // Assert
            Assert.IsNull(url);
        }

        private static void TestExpectedNotFoundResult(Func<IdentityController, ActionResult> action) {
            // Arrange
            var controller = new IdentityController();
            controller.IdentitySources.AddMock(UnusedMockIdentitySource);
            controller.IdentitySources.AddMock(SelectedMockIdentitySource);

            // Act
            ActionResult result = action(controller);

            // Assert
            ResultAssert.IsResourceNotFound(result);
        }
    }
}
