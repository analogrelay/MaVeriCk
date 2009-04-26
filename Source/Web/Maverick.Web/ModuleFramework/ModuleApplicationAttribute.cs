// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ModuleApplicationAttribute.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ModuleApplicationAttribute type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Maverick.ComponentModel;

namespace Maverick.Web.ModuleFramework {
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "The argument 'guid' is accessed via the property 'Id', the differing parameter name is to indicate to consumers that a guid is expected, since Guids are not valid Attribute constructor arguments")]
    public sealed class ModuleApplicationAttribute : ComponentMetadataAttribute {
        public ModuleApplicationAttribute(string guid, string name) : base(name) {
            Id = guid;
        }

        public ModuleApplicationAttribute(string guid, string name, string version) : base(name, version) {
            Id = guid;
        }

        public ModuleApplicationAttribute(string guid, string name, string version, string description) : base(name, version, description) {
            Id = guid;
        }

        public ModuleApplicationAttribute(string guid, string name, string version, string description, string vendor) : base(name, version, description, vendor) {
            Id = guid;
        }

        public ModuleApplicationAttribute(string guid, string name, string version, string description, string vendor, string logoUrl) : base(name, version, description, vendor, logoUrl) {
            Id = guid;
        }

        public string Id { get; private set; }
    }
}
