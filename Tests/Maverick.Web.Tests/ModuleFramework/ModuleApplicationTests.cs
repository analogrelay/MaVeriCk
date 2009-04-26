// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleApplicationTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleApplicationTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Maverick.Web.ModuleFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestUtilities;

namespace Maverick.Web.Tests.ModuleFramework {
    public partial class ModuleApplicationTests {
        [TestMethod]
        public void ModuleApplication_Is_ContractType_With_MetadataView() {
            CompositionAssert.IsContractType(typeof(ModuleApplication), typeof(ModuleApplicationMetadata));
        }

        [TestMethod]
        public void Init_Is_Called_In_First_ExecuteRequest_And_Not_In_Subsequent_Requests() {
            // Arrange
            ModuleApplication app = CreateTestApplication();
            SetupExecuteRequestCall(app);

            int initCounter = 0;
            Mock.Get(app)
                .Setup(a => a.Init(It.IsAny<MaverickApplication>()))
                .Callback(() => initCounter++);

            // Act
            app.ExecuteRequest(CreateModuleContext(app, "Foo"));
            app.ExecuteRequest(CreateModuleContext(app, "Bar"));

            // Assert
            Assert.AreEqual(1, initCounter, "Expected that init would be called once, and only once");
        }
    }
}
