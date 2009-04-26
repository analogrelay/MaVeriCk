// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextManager.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextManager type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Maverick.ComponentModel;

namespace Maverick.Data {
    [ContractType(MetadataViewType = typeof(ComponentMetadata))]
    public abstract class DataContextManager {
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This method is an action whose result should be cached for future usage rather than called again, thus a property is not appropriate")]
        public abstract DataContext GetCurrentDataContext();
    }
}
