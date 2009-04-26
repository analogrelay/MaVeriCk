// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Page.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Page type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Maverick.Models {
    public class Page {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Path { get; set; }
        public virtual Portal Portal { get; set; }
        public virtual Page ParentPage { get; set; }
        
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Data Layers may wish to set the value of this collection")]
        public virtual IList<Page> ChildPages { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Data Layers may wish to set the value of this collection")]
        public virtual IList<Module> Modules { get; set; }
    }
}
