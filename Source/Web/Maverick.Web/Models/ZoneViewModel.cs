// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ZoneViewModel.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ZoneViewModel type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Maverick.Web.ModuleFramework;

namespace Maverick.Web.Models {
    public class ZoneViewModel {
        private IList<ModuleRequestResult> _moduleResults = new List<ModuleRequestResult>();

        public string ZoneName { get; set; }

        public IList<ModuleRequestResult> ModuleResults {
            get { return _moduleResults; }
        }
    }
}
