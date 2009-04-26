// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalRequestContextTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalRequestContextTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition.Hosting;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using System.ComponentModel.Composition;

namespace Maverick.Web.Tests {
    [TestClass]
    public class PortalRequestContextTests {
        [Export(WebContractNames.AppDefaultTheme)]
        public static readonly string TestTheme = "Foo";

        [TestMethod]
        public void Constructor_Requires_Non_Null_HttpContext() {
            AutoTester.ArgumentNull<HttpContextBase>(marker => new PortalRequestContext(marker));
        }

        [TestMethod]
        public void Constructor_Sets_HttpContext_Property() {
            HttpContextBase context = Mockery.CreateMockHttpContext();
            Assert.AreSame(context, new PortalRequestContext(context).HttpContext);
        }

        [TestMethod]
        public void CurrentTheme_Returns_Null_If_No_Container() {
            // Arrange
            MaverickApplication.Container = null;
            PortalRequestContext context = Mockery.CreateMockPortalRequestContext();

            // Act
            string theme = context.CurrentTheme;

            // Assert
            Assert.IsNull(theme);
        }

        [TestMethod]
        public void CurrentTheme_Returns_Null_If_Container_Does_Not_Have_Export() {
            // Arrange
            MaverickApplication.Container = new CompositionContainer();
            PortalRequestContext context = Mockery.CreateMockPortalRequestContext();

            // Act
            string theme = context.CurrentTheme;

            // Assert
            Assert.IsNull(theme);

            MaverickApplication.Container = null;
        }

        [TestMethod]
        public void CurrentTheme_Returns_Imported_Value_If_Container_Does_Have_Export() {
            // Arrange
            MaverickApplication.Container = new CompositionContainer(new TypeCatalog(typeof(PortalRequestContextTests)));
            PortalRequestContext context = Mockery.CreateMockPortalRequestContext();

            // Act
            string theme = context.CurrentTheme;

            // Assert
            Assert.AreEqual(TestTheme, theme);

            MaverickApplication.Container = null;
        }

        [TestMethod]
        public void CurrentTheme_Returns_Provided_Value_If_Overridden() {
            // Arrange
            PortalRequestContext context = Mockery.CreateMockPortalRequestContext();
            context.CurrentTheme = TestTheme;

            // Act
            string theme = context.CurrentTheme;

            // Assert
            Assert.AreEqual(TestTheme, theme);
        }
    }
}
