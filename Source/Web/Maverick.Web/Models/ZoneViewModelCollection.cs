// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ZoneViewModelCollection.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ZoneViewModelCollection type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace Maverick.Web.Models {
    public class ZoneViewModelCollection : KeyedCollection<string, ZoneViewModel> {
        protected override string GetKeyForItem(ZoneViewModel item) {
            return item.ZoneName;
        }
    }
}