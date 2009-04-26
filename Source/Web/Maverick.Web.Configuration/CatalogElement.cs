// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CatalogElement.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CatalogElement type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Configuration;

namespace Maverick.Web.Configuration {
    public class CatalogElement : TypeElement {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }
    }
}