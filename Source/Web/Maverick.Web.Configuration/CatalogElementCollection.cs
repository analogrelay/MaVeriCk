// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CatalogElementCollection.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CatalogElementCollection type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Maverick.Web.Configuration {
    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "ConfigurationElementCollection is the recommended base class for collections of ConfigurationElement objects, additional interfaces are unnecessary")]
    [ConfigurationCollection(typeof(CatalogElement))]
    public class CatalogElementCollection : ConfigurationElementCollection {
        protected override ConfigurationElement CreateNewElement() {
            return new CatalogElement();
        }

        protected override object GetElementKey(ConfigurationElement element) {
            return ((CatalogElement)element).Name;
        }
    }
}