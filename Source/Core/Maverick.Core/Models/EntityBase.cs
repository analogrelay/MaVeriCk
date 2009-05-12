// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="EntityBase" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the EntityBase type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maverick.Models {
    public abstract class EntityBase {
        // Some data layers have trouble with inherited members, so by making this abstract, it's a little easier to deal with.
        protected internal abstract int? IdValue { get; }
        
        public virtual bool IsNew {
            get {
                return IdValue == null;
            }
        }
    }
}
