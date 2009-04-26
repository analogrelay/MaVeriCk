// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleApplicationMetadata.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleApplicationMetadata type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Maverick.ComponentModel;

namespace Maverick.Web.ModuleFramework {
    public class ModuleApplicationMetadata : ComponentMetadata {
        public ModuleApplicationMetadata(IDictionary<string, object> metadata) : base(metadata) {}

        public Guid Id {
            get { return GetMetadataProperty<Guid>("Id"); }
        }
    }
}