// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TypeElement.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the TypeElement type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Configuration;

namespace Maverick.Web.Configuration {
    public class TypeElement : ConfigurationElement {
        [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
        [TypeConverter(typeof(BuildManagerTypeNameConverter))]
        public Type SpecifiedType {
            get { return (Type)base["type"]; }
            set { base["type"] = value; }
        }
    }
}