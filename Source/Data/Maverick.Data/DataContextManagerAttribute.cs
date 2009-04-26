// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextManagerAttribute.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextManagerAttribute type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using Maverick.ComponentModel;

namespace Maverick.Data {
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DataContextManagerAttribute : ComponentMetadataAttribute {
        public DataContextManagerAttribute(string name) : base(name) {}
        public DataContextManagerAttribute(string name, string version) : base(name, version) {}
        public DataContextManagerAttribute(string name, string version, string description) : base(name, version, description) {}
        public DataContextManagerAttribute(string name, string version, string description, string vendor) : base(name, version, description, vendor) {}
        public DataContextManagerAttribute(string name, string version, string description, string vendor, string logoUrl) : base(name, version, description, vendor, logoUrl) {}
    }
}