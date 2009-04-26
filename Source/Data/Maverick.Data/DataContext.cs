// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContext.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContext type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace Maverick.Data {
    public abstract class DataContext : IDisposable {

        ~DataContext() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is the only necessary parameter and cannot be removed")]
        public abstract IEntitySet<TModel> GetEntitySet<TModel>() where TModel : class;

        public abstract void SaveChanges();

        protected virtual void Dispose(bool disposing) {
        }
    }
}
