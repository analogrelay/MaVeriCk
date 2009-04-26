// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ConfiguredCatalog.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ConfiguredCatalog type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;

namespace Maverick.Web.Configuration {
    public class ConfiguredCatalog : ComposablePartCatalog {
        public const string DefaultComponentsSection = "maverick/components";
        private readonly ComponentsSection _configurationSection;
        private AggregateCatalog _innerCatalog;

        public override IQueryable<ComposablePartDefinition> Parts {
            get {
                EnsureInnerCatalog();
                return _innerCatalog.Parts;
            }
        }

        public IEnumerable<ComposablePartCatalog> Catalogs {
            get {
                EnsureInnerCatalog();
                return _innerCatalog.Catalogs;
            }
        }

        public ConfiguredCatalog() : this(DefaultComponentsSection) {}
        public ConfiguredCatalog(string configurationSectionName) {
            Arg.NotNullOrEmpty("configurationSectionName", configurationSectionName);
            _configurationSection = ConfigurationManager.GetSection(configurationSectionName) as ComponentsSection;
        }
        public ConfiguredCatalog(ComponentsSection configurationSection) {
            _configurationSection = configurationSection;
        }

        private void EnsureInnerCatalog() {
            if (_innerCatalog == null) {
                _innerCatalog = LoadConfiguration(_configurationSection);
            }
        }

        private static AggregateCatalog LoadConfiguration(ComponentsSection componentsSection) {
            // Setup the catalog
            AggregateCatalog catalog = new AggregateCatalog();

            // Get the configuration section
            if(componentsSection == null) {
                return catalog;
            }

            // Load the catalogs
            foreach(CatalogElement catalogElement in componentsSection.Catalogs) {
                // TODO: Log or otherwise record TargetInvocationExceptions
                catalog.Catalogs.Add(Activator.CreateInstance(catalogElement.SpecifiedType) as ComposablePartCatalog);
            }

            // Load the directories
            // TODO: Add Search Pattern parameter to DirectoryElement
            foreach(DirectoryElement directoryElement in componentsSection.Directories) {
                catalog.Catalogs.Add(new DirectoryCatalog(directoryElement.Directory));
            }

            // Load the assemblies
            foreach(AssemblyInfo assemblyElement in componentsSection.Assemblies) {
                // Load the assembly by partial name
                Assembly asm = Assembly.Load(assemblyElement.Assembly);

                // Add a new AssemblyCatalog to the aggregate
                if(asm != null) {
                    catalog.Catalogs.Add(new AssemblyCatalog(asm));
                }
            }

            // Load the types
            if (componentsSection.Types.Count > 0) {
                catalog.Catalogs.Add(new TypeCatalog((from typeElement in componentsSection.Types.OfType<TypeElement>()
                                                      where typeElement.SpecifiedType != null
                                                      select typeElement.SpecifiedType)));
            }

            return catalog;
        }
    }
}