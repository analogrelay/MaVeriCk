// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DirectoryElement.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DirectoryElement type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Configuration;

namespace Maverick.Web.Configuration {
    public class DirectoryElement : ConfigurationElement {
        [ConfigurationProperty("directory", IsKey = true, IsRequired = true)]
        public string Directory {
            get { return (string)base["directory"]; }
            set { base["directory"] = value; }
        }
    }
}