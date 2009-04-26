// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ComponentCollection`2.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ComponentCollection type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.Composition;

namespace Maverick.ComponentModel {
    public class ComponentCollection<TComponent, TMetadataView> : ComponentCollection<string, TComponent, TMetadataView> where TMetadataView : ComponentMetadata {
        protected override string GetKeyForItem(Export<TComponent, TMetadataView> item) {
            Arg.NotNull("item", item);
            return item.MetadataView.Name;
        }
    }
}