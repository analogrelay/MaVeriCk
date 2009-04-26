// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CompositionContainerControllerFactory.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CompositionContainerControllerFactory type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;

namespace Maverick.Web.Controllers {
    public class CompositionContainerControllerFactory : DefaultControllerFactory {
        public CompositionContainer Container { get; set; }

        public CompositionContainerControllerFactory(CompositionContainer container) {
            Arg.NotNull("container", container);
            Container = container;
        }

        protected override IController GetControllerInstance(Type controllerType) {
            if(controllerType != null) {
                string contractName = AttributedModelServices.GetContractName(controllerType);
                IController instance = Container.GetExportedObjectOrDefault<object>(contractName) as IController;
                if(instance != null) {
                    return instance;
                }
            }
            return base.GetControllerInstance(controllerType);
        }
    }
}
