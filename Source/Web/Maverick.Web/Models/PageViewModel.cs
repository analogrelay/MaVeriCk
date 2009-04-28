// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PageViewModel.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PageViewModel type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using Maverick.Models;

namespace Maverick.Web.Models {
    public class PageViewModel {
        private readonly ZoneViewModelCollection _zones = new ZoneViewModelCollection();

        public Page Page { get; set; }
        public ControlPanelViewModel ControlPanelModel { get; set; }
        public ZoneViewModelCollection Zones {
            get { return _zones; }
        }

        public ZoneViewModel this[string zoneName] {
            get {
                if(!_zones.Contains(zoneName)) {
                    _zones.Add(new ZoneViewModel {ZoneName = zoneName});
                }
                return _zones[zoneName];                
            }
        }
    }
}
