// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ActionInvokerBasedControllerAdapter.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MvcControllerAdapter type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Web.Mvc;
using System.Web.Routing;
using Maverick.Web.Properties;

namespace Maverick.Web.ModuleFramework {
    public class MvcControllerAdapter : IModuleController  {
        private Controller _adaptedController;
        private ResultCapturingActionInvoker _actionInvoker;
        
        public MvcControllerAdapter(Controller controller) {
            _adaptedController = controller;
            _actionInvoker = new ResultCapturingActionInvoker();
            _adaptedController.ActionInvoker = _actionInvoker; 
        }

        public void Execute(RequestContext requestContext) {
            if(_adaptedController.ActionInvoker != _actionInvoker) {
                throw new InvalidOperationException(Resources.Error_CouldNotConstructController);
            }
            ((IController)_adaptedController).Execute(requestContext);
        }

        public ActionResult ResultOfLastExecute {
            get { return _actionInvoker.ResultOfLastInvoke; }
        }

        public ControllerContext ControllerContext {
            get { return _adaptedController.ControllerContext; }
        }
    }
}