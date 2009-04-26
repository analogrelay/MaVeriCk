// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ConnectionStringsCatalog.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ConnectionStringsCatalog type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using Maverick.Composition;

namespace Maverick.Web.Configuration {
    public class ConnectionStringsCatalog : CatalogBase {
        internal class ConnectionStringComposablePartDefinition : SingleExportComposablePartDefinitionBase<ConnectionStringSettings, Func<DbConnection>> {
            public ConnectionStringComposablePartDefinition(string name, ConnectionStringSettings value) : base(name, value) {}
            protected override Func<ConnectionStringSettings, Func<DbConnection>> CreateGetter() {
                return CreateConnection;
            }

            internal static Func<DbConnection> CreateConnection(ConnectionStringSettings settings) {
                return () => {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
                    DbConnection connection = factory.CreateConnection();
                    connection.ConnectionString = settings.ConnectionString;
                    return connection;
                };
            }
        }

        protected override IQueryable<ComposablePartDefinition> LoadParts() {
            return (from setting in ConfigurationManager.ConnectionStrings.OfType<ConnectionStringSettings>()
                    select new ConnectionStringComposablePartDefinition(setting.Name, setting))
                .Cast<ComposablePartDefinition>()
                .AsQueryable();
                   
        }
    }
}