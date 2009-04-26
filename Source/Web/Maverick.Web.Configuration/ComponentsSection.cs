// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ComponentsSection.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ComponentsSection type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Web.Configuration;

namespace Maverick.Web.Configuration {
    public class ComponentsSection : ConfigurationSection {
        public const string SectionName = "components";

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "The setter is used by the Configuration infrastructure to initialize the value of this property")]
        [ConfigurationProperty("catalogs", IsKey = false, IsRequired = false)]
        public CatalogElementCollection Catalogs {
            get {
                return (CatalogElementCollection)base["catalogs"];
            }
            set {
                base["catalogs"] = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "The setter is used by the Configuration infrastructure to initialize the value of this property")]
        [ConfigurationProperty("directories", IsKey = false, IsRequired = false)]
        public DirectoryElementCollection Directories {
            get {
                return (DirectoryElementCollection)base["directories"];
            }
            set {
                base["directories"] = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "The setter is used by the Configuration infrastructure to initialize the value of this property")]
        [ConfigurationProperty("assemblies", IsKey = false, IsRequired = false)]
        public AssemblyCollection Assemblies {
            get {
                return (AssemblyCollection)base["assemblies"];
            }
            set {
                base["assemblies"] = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "The setter is used by the Configuration infrastructure to initialize the value of this property")]
        [ConfigurationProperty("types", IsKey = false, IsRequired = false)]
        public TypeElementCollection Types {
            get {
                return (TypeElementCollection)base["types"];
            }
            set {
                base["types"] = value;
            }
        }
    }
}
