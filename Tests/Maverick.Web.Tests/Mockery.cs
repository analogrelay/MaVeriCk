// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Mockery.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Mockery type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.Web.ModuleFramework;
using Moq;

namespace Maverick.Web.Tests {
    internal class Mockery {
        internal static HttpContextBase CreateMockHttpContext() {
            var mockContext = new Mock<HttpContextBase>();
            mockContext.SetupGet(c => c.Items)
                       .Returns(new Dictionary<string, object>());

            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.QueryString)
                       .Returns(new NameValueCollection());

            mockContext.SetupGet(c => c.Request)
                       .Returns(mockRequest.Object);

            return mockContext.Object;
        }

        internal static HttpContextBase CreateMockHttpContext(string requestUrl) {
            HttpContextBase httpContext = CreateMockHttpContext();

            var mockRequest = Mock.Get(httpContext.Request);
            mockRequest.Setup(r => r.Url)
                       .Returns(new Uri(requestUrl));
            mockRequest.Setup(r => r.ApplicationPath)
                       .Returns(String.Empty);
            
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(r => r.ApplyAppPathModifier(It.IsAny<string>()))
                        .Returns<string>(s => s);

            Mock.Get(httpContext)
                .SetupGet(c => c.Response)
                .Returns(mockResponse.Object);

            return httpContext;
        }

        internal static ControllerContext CreateMockControllerContext(string requestUrl) {
            return new ControllerContext(CreateMockHttpContext(requestUrl), new RouteData(), new Mock<ControllerBase>().Object);
        }

        internal static ControllerContext CreateMockControllerContext(ControllerBase controller) {
            return new ControllerContext(CreateMockHttpContext(), new RouteData(), controller);
        }

        internal static ControllerContext CreateMockControllerContext() {
            return CreateMockControllerContext(new Mock<ControllerBase>().Object);
        }

        internal static Export<ModuleApplication, ModuleApplicationMetadata> CreateMockApplicationExport(Guid id) {
            Mock<ModuleApplication> mockApplication = new Mock<ModuleApplication>();
            return new Export<ModuleApplication, ModuleApplicationMetadata>(new Dictionary<string, object> {
                { "Id", id.ToString("N") },
                { "Name", "MockModule" }
            }, () => mockApplication.Object);
        }

        public static PortalRequestContext CreateMockPortalRequestContext() {
            HttpContextBase httpContext = CreateMockHttpContext();
            return new PortalRequestContext(httpContext);
        }
    }
}
