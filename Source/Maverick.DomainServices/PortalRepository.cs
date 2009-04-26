// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalService.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalService type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Maverick.Models;

namespace Maverick.DomainServices {
    [Export]
    public class PortalRepository : RepositoryBase<Portal> {}
}
