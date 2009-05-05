// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PageViewModel.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PageViewModel type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Maverick.Models;
using Maverick.Web.ModuleFramework;
using System.Linq;

namespace Maverick.Web.Models {
    public class PageViewModel {
        private readonly ZoneViewModelCollection _zones = new ZoneViewModelCollection();

        public Page Page { get; set; }
        public ZoneViewModelCollection Zones {
            get { return _zones; }
        }

        public ModuleRequestResult ControlPanelResult { get; set; }

        public ZoneViewModel this[string zoneName] {
            get {
                if(!_zones.Contains(zoneName)) {
                    _zones.Add(new ZoneViewModel {ZoneName = zoneName});
                }
                return _zones[zoneName];                
            }
        }

        public IEnumerable<ModuleRequestResult> AllModules {
            get {
                return new[] {ControlPanelResult}.Where(r => r != null)
                                                 .Concat(Zones.SelectMany(z => z.ModuleResults));
            }
        }
    }
}
