// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HeaderContributingViewResultAdapterTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the HeaderContributingViewResultAdapterTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Maverick.Web.ModuleFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Web.Tests.ModuleFramework {
    [TestClass]
    public class HeaderContributingViewResultAdapterTests {
        [TestMethod]
        public void Constructor_Requires_Non_Null_ViewResultBase() {
            AutoTester.ArgumentNull<ViewResultBase>(marker => new HeaderContributingViewResultAdapter(marker));
        }

        [TestMethod]
        public void Constructor_Initializes_HeaderViewNameFormat_Property() {
            ViewResult innerResult = new ViewResult();
            Assert.AreEqual("{0}.header", new HeaderContributingViewResultAdapter(innerResult).HeaderViewNameFormat);
        }

        [TestMethod]
        public void Constructor_Initializes_InnerResult_Property() {
            ViewResult expected = new ViewResult();
            Assert.AreSame(expected, new HeaderContributingViewResultAdapter(expected).InnerResult);
        }

        [TestMethod]
        public void ExecuteResult_Requires_Non_Null_Context() {
            AutoTester.ArgumentNull<ControllerContext>(marker => CreateAdapter().ExecuteResult(marker));
        }

        [TestMethod]
        public void ExecuteResult_Runs_InnerResult_Execute() {
            // Arrange
            var mockResult = new Mock<ViewResultBase>();
            ActionResult result = new HeaderContributingViewResultAdapter(mockResult.Object);
            ControllerContext context = Mockery.CreateMockControllerContext();
            
            // Act
            result.ExecuteResult(context);

            // Assert
            mockResult.Verify(r => r.ExecuteResult(context));
        }

        [TestMethod]
        public void ExecuteHeader_Requires_Non_Null_Context() {
            AutoTester.ArgumentNull<ControllerContext>(marker => CreateAdapter().ExecuteHeader(marker));
        }

        [TestMethod]
        public void ExecuteHeader_Executes_HeaderResult_For_ViewName_With_Header_Suffix() {
            // Arrange
            ViewResult innerResult = new ViewResult() {ViewName = "foo"};
            ControllerContext context = Mockery.CreateMockControllerContext();
            var mockAdapter = new Mock<HeaderContributingViewResultAdapter>(innerResult) { CallBase = true };
            mockAdapter.Setup(a => a.CreateHeaderResult(context, "foo.header"))
                       .Returns(new EmptyResult());
            
            // Act
            mockAdapter.Object.ExecuteHeader(context);

            // Assert
            mockAdapter.Verify(a => a.CreateHeaderResult(context, "foo.header"));
        }

        
        [TestMethod]
        public void GetHeaderViewName_Uses_HeaderViewNameFormat_To_Determine_HeaderViewName() {
            // Arrange
            ViewResult innerResult = new ViewResult() { ViewName = "foo" };
            HeaderContributingViewResultAdapter adapter = new HeaderContributingViewResultAdapter(innerResult) {
                HeaderViewNameFormat = "Zoop{0}Zork"
            };
            ControllerContext context = Mockery.CreateMockControllerContext();

            // Act
            string actual = adapter.GetHeaderViewName(context);

            // Assert
            Assert.AreEqual("ZoopfooZork", actual);
        }

        [TestMethod]
        public void GetHeaderViewName_Uses_Current_Action_Name_If_ViewName_Empty() {
            // Arrange
            ViewResult innerResult = new ViewResult() { ViewName = String.Empty };
            HeaderContributingViewResultAdapter adapter = new HeaderContributingViewResultAdapter(innerResult);
            ControllerContext context = Mockery.CreateMockControllerContext();
            context.RouteData.Values["action"] = "foo";

            // Act
            string actual = adapter.GetHeaderViewName(context);

            // Assert
            Assert.AreEqual("foo.header", actual);
        }

        [TestMethod]
        public void CreateHeaderResult_Requires_Non_Null_Context() {
            AutoTester.ArgumentNull<ControllerContext>(marker => CreateAdapter().CreateHeaderResult(marker, "Foo"));
        }

        [TestMethod]
        public void CreateHeaderResult_Requires_Non_NullOrEmpty_HeaderViewName() {
            AutoTester.StringArgumentNullOrEmpty(marker => CreateAdapter().CreateHeaderResult(Mockery.CreateMockControllerContext(), marker));
        }

        [TestMethod]
        public void CreateHeaderResult_Creates_PartialViewResult_Copying_All_But_MasterName_And_ViewName_From_InnerResult() {
            // Arrange
            ViewResult innerResult = CreateFullViewResult();
            HeaderContributingViewResultAdapter adapter = new HeaderContributingViewResultAdapter(innerResult);
            ControllerContext context = Mockery.CreateMockControllerContext();

            // Act
            PartialViewResult result = adapter.CreateHeaderResult(context, "View.Header") as PartialViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(innerResult.TempData, result.TempData);
            Assert.AreSame(innerResult.ViewData, result.ViewData);
            Assert.AreSame(innerResult.ViewEngineCollection, result.ViewEngineCollection);
            Assert.AreNotEqual(innerResult.ViewName, result.ViewName);
            Assert.AreNotEqual(innerResult.View, result.View);
        }

        [TestMethod]
        public void CreateHeaderResult_Creates_PartialViewResult_With_HeaderViewName_As_ViewName_And_No_View() {
            // Arrange
            ViewResult innerResult = CreateFullViewResult();
            HeaderContributingViewResultAdapter adapter = new HeaderContributingViewResultAdapter(innerResult);
            ControllerContext context = Mockery.CreateMockControllerContext();

            // Act
            PartialViewResult result = adapter.CreateHeaderResult(context, "View.Header") as PartialViewResult;

            // Assert
            Assert.IsNull(result.View);
            Assert.AreEqual("View.Header", result.ViewName);
        }

        private static ViewResult CreateFullViewResult() {
            return new ViewResult() {
                ViewName = "View",
                MasterName = "Master",
                TempData = new TempDataDictionary(),
                View = new Mock<IView>().Object,
                ViewData = new ViewDataDictionary(),
                ViewEngineCollection = new ViewEngineCollection()
            };
        }

        private static HeaderContributingViewResultAdapter CreateAdapter() {
            return new HeaderContributingViewResultAdapter(new ViewResult());
        }
    }
}
