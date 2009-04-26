// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="AppSettingsCatalog.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the AppSettingsCatalog type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Linq;
using Maverick.Composition;

namespace Maverick.Web.Configuration {
    public class AppSettingsCatalog : CatalogBase {
        internal class AppSettingComposablePart : SingleExportComposablePartDefinitionBase<string, string> {
            public AppSettingComposablePart(string name, string value) : base(name, value) {}

            protected override Func<string, string> CreateGetter() {
                return setting => setting;
            }
        }

        protected override IQueryable<ComposablePartDefinition> LoadParts() {
            return (from setting in ConfigurationManager.AppSettings.Keys.Cast<string>()
                    select new AppSettingComposablePart(setting, ConfigurationManager.AppSettings[setting]))
                .Cast<ComposablePartDefinition>()
                .AsQueryable();
        }
    }
}