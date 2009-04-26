// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HttpContextDataContextManagerTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the HttpContextDataContextManagerTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Web;
using Maverick.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;
using TargetResources = Maverick.Web.Properties.Resources;

namespace Maverick.Web.Tests {
    [TestClass]
    public class HttpContextDataContextManagerTests {
        [TestMethod]
        public void GetCurrentDataContext_Throws_InvalidOperationException_If_No_HttpContextSource_Specified() {
            // Arrange
            HttpContextDataContextManager manager = new HttpContextDataContextManager();

            // Act and Assert
            ExceptionAssert.Throws<InvalidOperationException>(() => manager.GetCurrentDataContext(), TargetResources.Error_NoHttpContextSource);
        }

        [TestMethod]
        public void GetCurrentDataContext_Throws_InvalidOperationException_If_No_DataContextBuilder_Specified() {
            // Arrange
            HttpContextDataContextManager manager = new HttpContextDataContextManager();
            HttpContextBase context = Mockery.CreateMockHttpContext();
            manager.HttpContextSource = () => context;

            // Act and Assert
            ExceptionAssert.Throws<InvalidOperationException>(() => manager.GetCurrentDataContext(), TargetResources.Error_NoDataContextBuilder);
        }

        [TestMethod]
        public void GetCurrentDataContext_Constructs_New_Context_And_Stores_In_HttpContext_If_No_Current_Context() {
            // Arrange
            HttpContextDataContextManager manager = new HttpContextDataContextManager();
            
            HttpContextBase context = Mockery.CreateMockHttpContext();
            manager.HttpContextSource = () => context;

            DataContext expectedDataContext = new Mock<DataContext>().Object;
            var mockDataContextBuilder = new Mock<DataContextBuilder>();
            mockDataContextBuilder.Setup(b => b.CreateDataContext()).Returns(expectedDataContext);
            manager.DataContextBuilder = mockDataContextBuilder.Object;
            
            // Act
            DataContext actualDataContext = manager.GetCurrentDataContext();

            // Assert
            Assert.AreSame(expectedDataContext, actualDataContext);
            Assert.AreSame(actualDataContext, context.Items[HttpContextDataContextManager.ContextKey]);
        }

        [TestMethod]
        public void GetCurrentDataContext_Returns_DataContext_In_HttpContext_If_Present() {
            // Arrange
            HttpContextDataContextManager manager = new HttpContextDataContextManager();

            HttpContextBase context = Mockery.CreateMockHttpContext();
            manager.HttpContextSource = () => context;

            DataContext expectedDataContext = new Mock<DataContext>().Object;
            context.Items[HttpContextDataContextManager.ContextKey] = expectedDataContext;

            var mockDataContextBuilder = new Mock<DataContextBuilder>();
            mockDataContextBuilder.Never(b => b.CreateDataContext());
            manager.DataContextBuilder = mockDataContextBuilder.Object;
            
            // Act
            DataContext actualDataContext = manager.GetCurrentDataContext();

            // Assert
            Assert.AreSame(expectedDataContext, actualDataContext);
        }
    }
}
