// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalPrefix.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalPrefix type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace Maverick.Models {
    public class PortalPrefix : EntityBase {
        [Range(0, Int32.MaxValue)]
        public virtual int? Id { get; set; }

        [Required]
        [StringLength(256)]
        [RegularExpression(@"[^:#?\s\\/]+(:[0-9]+)*(/([^:#?\s\\/]+))+")]
        public virtual string Prefix { get; set; }

        [Required]
        public virtual Portal Portal { get; set; }

        protected internal override int? IdValue {
            get {
                return Id;
            }
        }
    }
}