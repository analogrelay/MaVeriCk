// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ValidationFailedException" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ValidationFailedException type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Maverick.Properties;

namespace Maverick.Validation {
    [Serializable]
    public class ValidationFailedException : Exception {
        public IList<ValidationError> Errors { get; private set; }
        public string ObjectName { get; private set; }

        public ValidationFailedException(string objectName) : this(objectName, new List<ValidationError>()) {
        }

        public ValidationFailedException(string objectName, IList<ValidationError> errors) : base(ConstructMessage(objectName, errors)) {
            Arg.NotNullOrEmpty("objectName", objectName);

            ObjectName = objectName;
            Errors = errors;
        }

        private static string ConstructMessage(string modelName, IList<ValidationError> errors) {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendFormat(Resources.Error_ValidationFailed, modelName);
            if(errors.Count >0) {
                StringBuilder errorsBuilder = new StringBuilder();
                foreach(ValidationError error in errors) {
                    errorsBuilder.AppendFormat(Resources.Error_ValidationFailedRuleMesssage, error.ErrorMessage);
                    errorsBuilder.AppendLine();
                }
                messageBuilder.AppendFormat(Resources.Error_ValidationFailedRuleMessagesList, errorsBuilder);
            }

            return messageBuilder.ToString();
        }

        protected ValidationFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }
}
