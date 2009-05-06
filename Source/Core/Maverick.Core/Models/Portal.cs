// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Portal.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Portal type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Maverick.Models {
    public class Portal {
        [Range(0, Int32.MaxValue)]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(256)]
        public virtual string Name { get; set; }
    
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Data Layers may wish to set the value of this collection")]
        public virtual IList<PortalPrefix> PortalPrefixes { get; set; }
        
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Data Layers may wish to set the value of this collection")]
        public virtual IList<Page> Pages { get; set; }
    }
}
