// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Validator" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Validator type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maverick.Validation;

namespace Maverick.Validation {
    public abstract class Validator {
        public abstract IEnumerable<ValidationError> Validate(object obj);

        public virtual void ValidateAndThrowIfInvalid(object target) {
            IList<ValidationError> errors = Validate(target).ToList();
            if(errors.Count > 0) {
                throw new ValidationFailedException();
            }
        }
    }
}
