// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalPrefix.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalPrefix type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace Maverick.Models {
    public class PortalPrefix {
        public virtual string Prefix { get; set; }
        public virtual Portal Portal { get; set; }
        public virtual int Id { get; set; }
    }
}