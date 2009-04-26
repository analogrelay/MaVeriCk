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
    }
}
