// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IEntitySet.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IEntitySet type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace Maverick.Data {
    /// <summary>
    /// Provides an interface to a set of entities managed by a <see cref="DataContext"/>
    /// </summary>
    /// <typeparam name="T">The type of the entity contained in this set</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IEntitySet<T> : IEntityQuery<T>
    {
        void InsertOnSave(T model);
        void DeleteOnSave(T model);
        void UpdateOnSave(T modified);
        void UpdateOnSave(T original, T modified);
        void Attach(T original);
        void Detach(T model);
    }
}