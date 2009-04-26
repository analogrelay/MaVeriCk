// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ComponentCollection`3.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ComponentCollection type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;

namespace Maverick.ComponentModel {
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "All three type parameters are required since some ComponentCollection instances use non-string keys and cannot use the 2-parameter version")]
    public abstract class ComponentCollection<TKey, TComponent, TMetadataView> : KeyedCollection<TKey, Export<TComponent, TMetadataView>> where TMetadataView : ComponentMetadata {
    }
}