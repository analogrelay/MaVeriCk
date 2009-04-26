// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleApplicationCollection.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleApplicationCollection type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using Maverick.ComponentModel;

namespace Maverick.Web.ModuleFramework {
    public class ModuleApplicationCollection : ComponentCollection<Guid, ModuleApplication, ModuleApplicationMetadata> {
        protected override Guid GetKeyForItem(Export<ModuleApplication, ModuleApplicationMetadata> item) {
            return item.MetadataView.Id;
        }
    }
}
