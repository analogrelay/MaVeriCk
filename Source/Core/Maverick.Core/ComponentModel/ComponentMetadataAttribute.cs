// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ComponentMetadataAttribute.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ComponentMetadataAttribute type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;

namespace Maverick.ComponentModel {
    [MetadataAttribute]
    public abstract class ComponentMetadataAttribute : Attribute {
        public string Name { get; private set; }
        public string Version { get; private set; }
        
        public string Vendor { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }

        protected ComponentMetadataAttribute(string name) : this(name, null, null, null, null) { }
        protected ComponentMetadataAttribute(string name, string version) : this(name, version, null, null, null) { }
        protected ComponentMetadataAttribute(string name, string version, string description) : this(name, version, description, null, null) { }
        protected ComponentMetadataAttribute(string name, string version, string description, string vendor) : this(name, version, description, vendor, null) { }
        protected ComponentMetadataAttribute(string name, string version, string description, string vendor, string logoUrl) {
            Arg.NotNullOrEmpty("name", name);
            
            Name = name;
            Version = version;
            Description = description;
            Vendor = vendor;
            LogoUrl = logoUrl;
        }
    }
}
