// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleService.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleService type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Maverick.Models;

namespace Maverick.DomainServices {
    [Export]
    public class ModuleRepository : RepositoryBase<Module> {}
}
