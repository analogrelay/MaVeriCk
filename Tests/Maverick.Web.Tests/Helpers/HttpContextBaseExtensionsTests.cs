// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HttpContextBaseExtensionsTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the HttpContextBaseExtensionsTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maverick.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace Maverick.Web.Tests.Helpers {
    [TestClass]
    public class HttpContextBaseExtensionsTests {
        [TestMethod]
        public void HasPortalContext_Returns_False_If_No_PortalRequestContext_Key_Exists() {
            // Arrange
            HttpContextBase context = Mockery.CreateMockHttpContext();

            // Act and Assert
            Assert.IsFalse(context.HasPortalContext());
        }

        [TestMethod]
        public void HasPortalContext_Returns_False_If_PortalRequestContext_Key_Is_Null() {
            // Arrange
            HttpContextBase context = Mockery.CreateMockHttpContext();
            context.Items.Add(HttpContextBaseExtensions.GetKeyFor<PortalRequestContext>(), null);

            // Act and Assert
            Assert.IsFalse(context.HasPortalContext());
        }

        [TestMethod]
        public void GetPortalContext_Returns_New_PortalRequestContext_If_None_Present() {
            // Arrange
            HttpContextBase context = Mockery.CreateMockHttpContext();
            
            // Act and Assert
            Assert.IsNotNull(context.GetPortalContext());
        }

        [TestMethod]
        public void GetPortalContext_Stores_New_PortalRequestContext_In_HttpContextBase() {
            // Arrange
            HttpContextBase context = Mockery.CreateMockHttpContext();

            // Act
            PortalRequestContext portalContext = context.GetPortalContext();

            // Assert
            Assert.AreSame(portalContext, context.Items[HttpContextBaseExtensions.GetKeyFor<PortalRequestContext>()]);
        }

        [TestMethod]
        public void GetPortalContext_Returns_Stored_PortalRequestContext_If_Present() {
            // Arrange
            HttpContextBase context = Mockery.CreateMockHttpContext();
            PortalRequestContext expected = new PortalRequestContext(context);
            context.Items.Add(HttpContextBaseExtensions.GetKeyFor<PortalRequestContext>(), expected);

            // Act
            PortalRequestContext actual = context.GetPortalContext();

            // Assert
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void GetKeyFor_Prefixes_Full_TypeName_With_MaverickContext_Prefix() {
            // Assert
            Assert.AreEqual(String.Format("__MaverickContext:{0}", typeof(Version).FullName), HttpContextBaseExtensions.GetKeyFor<Version>());
        }
    }
}
