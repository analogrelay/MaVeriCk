// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CompositionContainerControllerFactoryTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CompositionContainerControllerFactoryTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.Composition;
using Maverick.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Web.Tests.Controllers {
    [TestClass]
    public class CompositionContainerControllerFactoryTests {
        private class TestController : Controller {}

        private class TestControllerFactory : CompositionContainerControllerFactory {
            public TestControllerFactory(CompositionContainer container) : base(container) {}

            public IController GetControllerInstanceFromType(Type controllerType) {
                return this.GetControllerInstance(controllerType);
            }
        }

        [TestMethod]
        public void Constructor_Requires_Non_Null_CompositionContainer() {
            AutoTester.ArgumentNull<CompositionContainer>(marker => new CompositionContainerControllerFactory(marker));
        }

        [TestMethod]
        public void Constructor_Initializes_Container_Property_To_Provided_Container() {
            CompositionContainer container = new CompositionContainer();
            Assert.AreSame(container, new CompositionContainerControllerFactory(container).Container);
        }

        [TestMethod]
        public void GetControllerInstance_Throws_404_HttpException_If_Controller_Type_Null() {
            // Arrange
            TestControllerFactory factory = SetupControllerFactory();

            // Act and Assert
            // (Exception message comes from MVC, so it's tricky to verify it)
            ExceptionAssert.Throws<HttpException>(() => factory.GetControllerInstanceFromType(null),
                                                  ex => ex.GetHttpCode() == 404);
        }

        [TestMethod]
        public void GetControllerInstance_Constructs_Type_Directly_If_Not_Present_In_CompositionContainer() {
            // Arrange
            TestControllerFactory factory = SetupControllerFactory();

            // Act
            IController controller = factory.GetControllerInstanceFromType(typeof(PageController));

            // Assert
            Assert.IsInstanceOfType(controller, typeof(PageController));
        }

        [TestMethod]
        public void GetControllerInstance_Imports_Controller_From_CompositionContainer_If_Present() {
            // Arrange
            TestControllerFactory factory = SetupControllerFactory();
            CompositionBatch batch = new CompositionBatch();
            TestController expected = new TestController();
            batch.AddPart(
                new SingleExportComposablePart(
                    new Export(AttributedModelServices.GetContractName(typeof(TestController)),
                               new Dictionary<string, object>(),
                               () => expected)));
            factory.Container.Compose(batch);

            // Act
            IController actual = factory.GetControllerInstanceFromType(typeof(TestController));

            // Assert
            Assert.AreSame(expected, actual);
        }

        private TestControllerFactory SetupControllerFactory() {
            CompositionContainer container = new CompositionContainer();
            return new TestControllerFactory(container) {
                RequestContext = new RequestContext(Mockery.CreateMockHttpContext("http://localhost"), new RouteData())
            };
        }
    }
}