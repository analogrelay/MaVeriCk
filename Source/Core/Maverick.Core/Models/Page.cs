// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Page.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Page type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Maverick.Models {
    public class Page : EntityBase {
        [Range(0, Int32.MaxValue)]
        public virtual int? Id { get; set; }

        [Required]
        [StringLength(256)]
        public virtual string Title { get; set; }

        [Required]
        [StringLength(1024)]
        [RegularExpression(@"(/([^:#?\s\\/]+))+")]
        public virtual string Path { get; set; }

        [Required]
        public virtual Portal Portal { get; set; }

        public virtual Page ParentPage { get; set; }
        
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Data Layers may wish to set the value of this collection")]
        public virtual IList<Page> ChildPages { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Data Layers may wish to set the value of this collection")]
        public virtual IList<Module> Modules { get; set; }

        protected internal override int? IdValue {
            get {
                return Id;
            }
        }
    }
}
