// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ValidationError" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ValidationError type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maverick.Validation {
    public class ValidationError {
        public string PropertyName { get; private set; }
        public string ErrorMessage { get; private set; }
        public object TargetObject { get; private set; }

        public ValidationError(string propertyName, string errorMessage) {
            Arg.NotNullOrEmpty("propertyName", propertyName);
            Arg.NotNullOrEmpty("errorMessage", errorMessage);

            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public ValidationError(string propertyName, string errorMessage, object targetObject) : this(propertyName, errorMessage) {
            Arg.NotNull("targetObject", targetObject);

            TargetObject = targetObject;
        }
    }
}
