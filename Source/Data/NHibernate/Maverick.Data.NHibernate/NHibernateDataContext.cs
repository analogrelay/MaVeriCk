// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="NHibernateDataContext.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the NHibernateDataContext type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using NHibernate;
using NHibernate.Linq;

namespace Maverick.Data.NHibernate {
    public class NHibernateDataContext : DataContext {
        private readonly ISession _session;

        // If someone _really_ wants to cast their entity set to NHibernateEntitySet<T> and use the Session directly, 
        // I don't see a good reason to stop them.  After all, they can always just connect an new NHibernate session up to the db, 
        // so we may as well allow them to share the cache
        public ISession Session {
            get { return _session; }
        }

        public NHibernateDataContext(ISession session) {
            Arg.NotNull("session", session);
            _session = session;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is the only necessary parameter and cannot be removed")]
        public override IEntitySet<TModel> GetEntitySet<TModel>() {
            return new NHibernateEntitySet<TModel>(_session, _session.Linq<TModel>());
        }

        public override void SaveChanges() {
            _session.Flush();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                _session.Dispose();
            }
        }
    }
}
