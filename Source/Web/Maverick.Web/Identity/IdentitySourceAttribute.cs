// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentitySourceAttribute.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentitySourceAttribute type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using Maverick.ComponentModel;

namespace Maverick.Web.Identity {
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class IdentitySourceAttribute : ComponentMetadataAttribute {
        public IdentitySourceAttribute(string name) : base(name) {}
        public IdentitySourceAttribute(string name, string version) : base(name, version) {}
        public IdentitySourceAttribute(string name, string version, string description) : base(name, version, description) {}
        public IdentitySourceAttribute(string name, string version, string description, string vendor) : base(name, version, description, vendor) {}
        public IdentitySourceAttribute(string name, string version, string description, string vendor, string logoUrl) : base(name, version, description, vendor, logoUrl) {}
    }
}