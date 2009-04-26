// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="RequiredMetadataMissingException.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the RequiredMetadataMissingException type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
using System;
using System.Runtime.Serialization;

namespace Maverick.ComponentModel {
    [Serializable]
    public class RequiredMetadataMissingException : Exception {
        public RequiredMetadataMissingException() {}
        public RequiredMetadataMissingException(string message) : base(message) {}
        public RequiredMetadataMissingException(string message, Exception inner) : base(message, inner) {}

        protected RequiredMetadataMissingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }
}
