// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="InvalidModelStateException" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the InvalidModelStateException type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Maverick.DomainServices.Properties;

namespace Maverick.DomainServices {
    [Serializable]
    public class InvalidModelStateException : Exception {
        public InvalidModelStateException() : base(Resources.Error_ModelStateInvalid) {}
        public InvalidModelStateException(string message) : base(message) {}
        public InvalidModelStateException(string message, Exception inner) : base(message, inner) {}

        protected InvalidModelStateException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }
}
