// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextFactory.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextFactory type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Maverick.ComponentModel;

namespace Maverick.Data {
    [ContractType(MetadataViewType = typeof(ComponentMetadata))]
    public abstract class DataContextFactory {
        public abstract DataContext CreateDataContext();
    }
}
