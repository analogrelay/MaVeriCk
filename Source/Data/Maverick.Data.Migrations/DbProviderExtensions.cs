// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DbProviderExtensions" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DbProviderExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RikMigrations;
using RikMigrations.Providers;

namespace Maverick.Data.Migrations {
    internal static class DbProviderExtensions {
        public static int GetLastId(this DbProvider provider, string tableName) {
            int? id = null;
            if(provider is MssqlProvider) {
                // Consumer of this is the migration assembly, so no real need for SQL Injection protection :)
                // For some reason, we have to cast through decimal, probably because the object is actually returned as a decimal
                // So we have to cast to decimal to unbox it, then convert to an integer
                id = (int)(decimal)provider.ExecuteScalar(String.Format("SELECT IDENT_CURRENT('{0}')", tableName));
            }
            if(provider is SqliteProvider) {
                // TODO: Verify this code
                id = (int)provider.ExecuteScalar("SELECT last_insert_rowid");
            }
            if (id == null) {
                throw new InvalidOperationException(
                    String.Format("GetLastId is not implemented for providers of type: {0}", provider.GetType().FullName));
            }
            return id.Value;
        }
    }
}
