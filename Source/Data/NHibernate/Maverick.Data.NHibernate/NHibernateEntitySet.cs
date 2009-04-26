// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="NHibernateEntitySet.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the NHibernateEntitySet type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Maverick.Data.NHibernate.Properties;
using NHibernate;
using NHibernate.Linq;

namespace Maverick.Data.NHibernate {
    [CLSCompliant(false)]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "The naming of this class is designed to match the interface: IEntitySet")]
    public class NHibernateEntitySet<T> : IEntitySet<T> where T : class {
        private readonly ISession _session;
        private readonly INHibernateQueryable<T> _query;

        internal ISession Session { get { return _session; }}
        internal INHibernateQueryable<T> Query { get { return _query; } }

        public NHibernateEntitySet(ISession session, INHibernateQueryable<T> query) {
            Arg.NotNull("session", session);
            Arg.NotNull("query", query);

            _session = session;
            _query = query;
        }

        public IEnumerator<T> GetEnumerator() {
            return _query.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _query.GetEnumerator();
        }

        public Expression Expression {
            get { return _query.Expression; }
        }
        
        public Type ElementType {
            get { return _query.ElementType; }
        }
        
        public IQueryProvider Provider {
            get { return _query.Provider; }
        }

        public IEntityQuery<T> Include(string relationshipPropertyName) {
            _query.Expand(relationshipPropertyName);
            return this;
        }

        public IEntityQuery<T> Include(Expression<Func<T, object>> relationshipPropertySelector) {
            _query.Expand(GetPropertyNameFromExpression(relationshipPropertySelector));
            return this;
        }

        public void InsertOnSave(T model) {
            _session.Save(model);
        }

        public void DeleteOnSave(T model) {
            _session.Delete(model);
        }

        public void UpdateOnSave(T modified) {
            _session.Update(modified);
        }

        public void UpdateOnSave(T original, T modified) {
            _session.Update(modified);
        }

        public void Attach(T original) {
            _session.Lock(original, LockMode.None);
        }

        public void Detach(T model) {
            _session.Evict(model);
        }

        private static string GetPropertyNameFromExpression(Expression<Func<T, object>> expression) {
            // The body must be a member access expression
            Guard.Against(expression.Body.NodeType != ExpressionType.MemberAccess, Resources.Error_ExpressionWasNotMemberAccess);

            // Get the name from the expression
            MemberExpression memberExpression = (MemberExpression)expression.Body;
            return memberExpression.Member.Name;
        }
    }
}