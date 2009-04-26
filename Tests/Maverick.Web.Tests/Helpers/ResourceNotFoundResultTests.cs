// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ResourceNotFoundResultTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ResourceNotFoundResultTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Maverick.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using System.Web.Mvc;
using Moq;

namespace Maverick.Web.Tests.Helpers {
    [TestClass]
    public class ResourceNotFoundResultTests {
        [TestMethod]
        public void DefaultInnerResultFactory_Creates_EmptyResult_If_No_Default_Set() {
            ResourceNotFoundResult.DefaultInnerResultFactory = null;
            ResultAssert.IsEmpty(ResourceNotFoundResult.DefaultInnerResultFactory());
        }

        [TestMethod]
        public void DefaultInnerResultFactory_Can_Be_Overridden() {
            ResourceNotFoundResult.DefaultInnerResultFactory = () => new HttpUnauthorizedResult();
            ResultAssert.IsUnauthorized(ResourceNotFoundResult.DefaultInnerResultFactory());
            ResourceNotFoundResult.DefaultInnerResultFactory = null;
        }

        [TestMethod]
        public void ExecuteResult_Sets_StatusCode_To_404() {
            // Arrange
            ControllerContext context = Mockery.CreateMockControllerContext();
            ResourceNotFoundResult result = new ResourceNotFoundResult();

            // Act
            result.ExecuteResult(context);

            // Assert
            Mock.Get(context.HttpContext.Response)
                .VerifySet(r => r.StatusCode, 404);
        }

        [TestMethod]
        public void ExecuteResult_Executes_Default_InnerResult_With_Context_If_No_InnerResult_Provided() {
            // Arrange
            ControllerContext context = Mockery.CreateMockControllerContext();
            var mockResult = new Mock<ActionResult>();
            ResourceNotFoundResult.DefaultInnerResultFactory = () => mockResult.Object;
            ResourceNotFoundResult result = new ResourceNotFoundResult();
            
            // Act
            result.ExecuteResult(context);

            // Assert
            mockResult.Verify(r => r.ExecuteResult(context));
        }

        [TestMethod]
        public void ExecuteResult_Executes_Provided_InnerResult_With_Context_If_No_InnerResult_Provided() {
            // Arrange
            ControllerContext context = Mockery.CreateMockControllerContext();
            ResourceNotFoundResult.DefaultInnerResultFactory = () => {
                Assert.Fail("Expected that the default inner result factory would not be used");
                return null;
            };
            var mockResult = new Mock<ActionResult>();
            ResourceNotFoundResult result = new ResourceNotFoundResult() {
                InnerResult = mockResult.Object
            };
            
            // Act
            result.ExecuteResult(context);

            // Assert
            mockResult.Verify(r => r.ExecuteResult(context));
        }
    }
}
