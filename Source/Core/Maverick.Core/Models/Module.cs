// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Module.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Module type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace Maverick.Models {
    [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Module", Justification = "While VB.Net's Module keyword may conflict with this name, changing it would make it inconsistent with the rest of the models.  Model type names are designed to exactly match database tables (although in singular form)")]
    public class Module {
        [Required]
        [Range(0, Int32.MaxValue)]
        public virtual int Id { get; set; }

        [Required]
        public virtual Page Page { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string ZoneName { get; set; }

        [Required]
        public virtual Guid ModuleApplicationId { get; set; }

        [Required]
        [StringLength(256)]
        public virtual string Title { get; set; }
    }
}
