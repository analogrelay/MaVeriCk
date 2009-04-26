// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentityServicesTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentityServicesTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using Maverick.Web.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maverick.Web.Tests.Identity {
    [TestClass]
    public class IdentityServicesTests {
        [IdentitySource("TestSource")]
        private class TestSource : IdentitySource {
            public override ActionResult Login(ControllerContext controllerContext, Uri returnUrl) {
                throw new NotImplementedException();
            }
        }

        private class TestSourceWithNoAttribute : IdentitySource {
            public override ActionResult Login(ControllerContext controllerContext, Uri returnUrl) {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void GetSourceName_With_Type_Parameter_Returns_Name_From_IdentitySourceAttribute_On_Type() {
            // Assert
            Assert.AreEqual("TestSource", IdentityServices.GetSourceName<TestSource>());
        }

        [TestMethod]
        public void GetSourceName_With_Type_Argument_Returns_Name_From_IdentitySourceAttribute_On_Type() {
            // Assert
            Assert.AreEqual("TestSource", IdentityServices.GetSourceName(typeof(TestSource)));
        }

        [TestMethod]
        public void GetSourceName_With_IdentitySource_Instance_Returns_Name_From_IdentitySourceAttribute_On_Type() {
            // Assert
            Assert.AreEqual("TestSource", IdentityServices.GetSourceName(new TestSource()));
        }

        [TestMethod]
        public void GetSourceName_With_Type_Parameter_Returns_Null_String_If_No_IdentitySourceAttribute_On_Type() {
            // Assert
            Assert.IsNull(IdentityServices.GetSourceName<TestSourceWithNoAttribute>());
        }

        [TestMethod]
        public void GetSourceName_With_Type_Argument_Returns_Null_String_If_No_IdentitySourceAttribute_On_Type() {
            // Assert
            Assert.IsNull(IdentityServices.GetSourceName(typeof(TestSourceWithNoAttribute)));
        }

        [TestMethod]
        public void GetSourceName_With_IdentitySource_Instance_Returns_Null_String_If_No_IdentitySourceAttribute_On_Type() {
            // Assert
            Assert.IsNull(IdentityServices.GetSourceName(new TestSourceWithNoAttribute()));
        }
    }
}
