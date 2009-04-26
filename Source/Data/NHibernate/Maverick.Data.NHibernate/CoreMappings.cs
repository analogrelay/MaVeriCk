// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CoreMappings.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CoreMappings type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using Maverick.Models;

namespace Maverick.Data.NHibernate {
    [Export(typeof(MappingContributor))]
    [CLSCompliant(false)]
    public class CoreMappings : FluentMappingContributor {
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "The only reason these types are public is because FluentNHibernate does not allow consumers to provide a fully constructed IClassMap instance.  They are nested since there is no use for them outside of the CoreMappings class")]
        public class PortalMap : ClassMap<Portal> {
            public PortalMap() {
                WithTable("Portals");
                Id(p => p.Id, "Id");
                Map(p => p.Name, "Name").WithLengthOf(256).Not.Nullable();
                HasMany(p => p.PortalPrefixes).KeyColumnNames.Add("PortalId").Inverse();
                HasMany(p => p.Pages).KeyColumnNames.Add("PageId").Inverse();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "The only reason these types are public is because FluentNHibernate does not allow consumers to provide a fully constructed IClassMap instance.  They are nested since there is no use for them outside of the CoreMappings class")]
        public class PortalPrefixMap : ClassMap<PortalPrefix> {
            public PortalPrefixMap() {
                WithTable("PortalPrefixes");
                Id(p => p.Id, "Id");
                Map(p => p.Prefix, "Prefix").WithLengthOf(256).Not.Nullable();
                References(p => p.Portal, "PortalId").Not.Nullable();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "The only reason these types are public is because FluentNHibernate does not allow consumers to provide a fully constructed IClassMap instance.  They are nested since there is no use for them outside of the CoreMappings class")]
        public class PageMap : ClassMap<Page> {
            public PageMap() {
                WithTable("Pages");
                Id(p => p.Id, "Id");
                Map(p => p.Title, "Title").WithLengthOf(256).Not.Nullable();
                Map(p => p.Path, "Path").WithLengthOf(1024).Not.Nullable();
                References(p => p.ParentPage, "ParentId").Nullable();
                References(p => p.Portal, "PortalId").Not.Nullable();
                HasMany(p => p.ChildPages).KeyColumnNames.Add("ParentId").Inverse();
                HasMany(p => p.Modules).KeyColumnNames.Add("PageId").Inverse();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "The only reason these types are public is because FluentNHibernate does not allow consumers to provide a fully constructed IClassMap instance.  They are nested since there is no use for them outside of the CoreMappings class")]
        public class ModuleMap : ClassMap<Module> {
            public ModuleMap() {
                WithTable("Modules");
                Id(p => p.Id, "Id");
                Map(p => p.Title, "Title").WithLengthOf(256).Not.Nullable();
                Map(p => p.ZoneName, "ZoneName").WithLengthOf(256).Not.Nullable();
                Map(p => p.ModuleApplicationId, "ModuleApplicationId").Not.Nullable();
                References(p => p.Page, "PageId").Not.Nullable();
            }
        }

        public override void ContributeMappings(MappingConfiguration configuration) {
            // TODO: Use FluentNHibernate auto mapping instead
            configuration.FluentMappings.AddFromAssemblyOf<PageMap>();
        }
    }
}
