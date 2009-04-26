// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextFactoryCollectionExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextFactoryCollectionExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Maverick.ComponentModel;

namespace Maverick.Data.Tests {
    public static class DataContextFactoryCollectionExtensions {
        // use extension methods because MEF gets confused if put this directly on the collection type
        public static void Add<T, M>(this ComponentCollection<T, M> factoryCollection, string name, Func<T> componentGetter) where M : ComponentMetadata {
            Dictionary<string, object> metadata = new Dictionary<string, object> { { "Name", name } };
            factoryCollection.Add(new Export<T, M>(metadata, componentGetter));
        }
    }
}