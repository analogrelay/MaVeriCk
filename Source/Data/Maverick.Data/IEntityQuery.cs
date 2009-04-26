// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IEntityQuery.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IEntityQuery type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Maverick.Data {
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification="Name is designed to match IQueryable")]
    public interface IEntityQuery<T> : IQueryable<T> {
        // Asks the data session to perform a best-effort attempt to load the relationship specified by the
        // provided property eagerly (at the same time as the object being queried)
        // NOTE: This is a best-effort attempt and may still result in lazy loading, depending on the conditions and data session type
        IEntityQuery<T> Include(string relationshipPropertyName);
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Expression<Func<...>> is the common pattern for specifying LINQ expression types")]
        IEntityQuery<T> Include(Expression<Func<T, object>> relationshipPropertySelector);
    }
}