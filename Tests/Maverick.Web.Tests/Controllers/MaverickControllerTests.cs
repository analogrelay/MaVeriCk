// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MaverickControllerTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MaverickControllerTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;
using Maverick.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Web.Tests.Controllers {
    [TestClass]
    public class MaverickControllerTests {
        private const string TestViewName = "Foo";
        private const string TestMasterName = "Bar";

        [TestMethod]
        public void ResourceNotFound_Requires_Non_NullOrEmpty_ViewName() {
            MaverickController controller = CreateController();
            AutoTester.StringArgumentNullOrEmpty(marker => controller.ResourceNotFound(marker));
            AutoTester.StringArgumentNullOrEmpty(marker => controller.ResourceNotFound(marker, TestMasterName));
        }

        [TestMethod]
        public void ResourceNotFound_Requires_Non_NullOrEmpty_MasterName() {
            MaverickController controller = CreateController();
            AutoTester.StringArgumentNullOrEmpty(marker => controller.ResourceNotFound(TestViewName, marker));
        }

        [TestMethod]
        public void ResourceNotFound_Requires_Non_Null_InnerResult() {
            MaverickController controller = CreateController();
            AutoTester.ArgumentNull<ActionResult>(marker => controller.ResourceNotFound(marker));
        }

        [TestMethod]
        public void ResourceNotFound_With_No_Args_Returns_ResourceNotFoundResult_With_No_InnerResult() {
            // Arrange
            MaverickController controller = CreateController();

            // Act
            ActionResult result = controller.ResourceNotFound();

            // Assert
            ResultAssert.IsResourceNotFound(result);
        }

        [TestMethod]
        public void ResourceNotFound_With_ViewName_Returns_ResourceNotFoundResult_Inner_ViewResult() {
            // Arrange
            MaverickController controller = CreateController();

            // Act
            ActionResult result = controller.ResourceNotFound(TestViewName);

            // Assert
            ResultAssert.IsResourceNotFound(result, TestViewName);
        }

        [TestMethod]
        public void ResourceNotFound_With_View_And_Master_Names_Returns_ResourceNotFoundResult_Inner_ViewResult() {
            // Arrange
            MaverickController controller = CreateController();

            // Act
            ActionResult result = controller.ResourceNotFound(TestViewName, TestMasterName);

            // Assert
            ResultAssert.IsResourceNotFound(result, TestViewName, TestMasterName);
        }

        [TestMethod]
        public void ResourceNotFound_With_ActionResult_Returns_ResourceNotFoundResult_Inner_ActionResult() {
            // Arrange
            MaverickController controller = CreateController();

            // Act
            EmptyResult expected = new EmptyResult();
            ActionResult result = controller.ResourceNotFound(expected);

            // Assert
            ResultAssert.IsResourceNotFound(result, ResultAssert.IsEmpty);
            
        }

        private MaverickController CreateController() {
            return new Mock<MaverickController> {CallBase = true}.Object;
        }
    }
}