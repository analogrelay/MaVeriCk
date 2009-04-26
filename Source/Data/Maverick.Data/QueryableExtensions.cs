// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="QueryableExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the QueryableExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Maverick.Models;

namespace Maverick.Data {
    public static class QueryableExtensions {
        public static IEntityQuery<T> AsEntityQuery<T>(this IEnumerable<T> enumerable) {
            Arg.NotNull("enumerable", enumerable);
            return enumerable.AsQueryable().AsEntityQuery();
        }

        public static IEntityQuery<T> AsEntityQuery<T>(this IQueryable<T> query) {
            Arg.NotNull("query", query);
            return new EntityQueryAdapter<T>(query);
        }
    }
}