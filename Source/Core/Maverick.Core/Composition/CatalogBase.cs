// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CatalogBase.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CatalogBase type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Maverick.Composition {
    public abstract class CatalogBase : ComposablePartCatalog {
        private IQueryable<ComposablePartDefinition> _parts;
        public override IQueryable<ComposablePartDefinition> Parts {
            get {
                if(_parts == null) {
                    _parts = LoadParts();
                }
                return _parts;
            }
        }
        protected abstract IQueryable<ComposablePartDefinition> LoadParts();
    }
}