// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ControllerTestBase.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ControllerTestBase type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace Maverick.Web.Tests {
    public abstract class ControllerTestBase<TController> where TController : Controller {
        private ActionFilterAssert<TController> _actionFilterAssert = new ActionFilterAssert<TController>();

        public ActionFilterAssert<TController> ActionFilterAssert {
            get { return _actionFilterAssert; }
            set { _actionFilterAssert = value; }
        }
    }
}