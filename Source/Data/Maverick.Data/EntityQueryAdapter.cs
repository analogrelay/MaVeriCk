// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="EntityQueryAdapter.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the EntityQueryAdapter type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Maverick.Data;

namespace Maverick.Models {
    internal class EntityQueryAdapter<T> : IEntityQuery<T> {
        private readonly IQueryable<T> _innerQuery;

        internal IQueryable<T> InnerQuery {
            get { return _innerQuery; }
        }

        public EntityQueryAdapter(IQueryable<T> query) {
            Arg.NotNull("query", query);
            _innerQuery = query;
        }

        #region IEntityQuery<T> Members

        public IEntityQuery<T> Include(string relationshipPropertyName) {
            Arg.NotNullOrEmpty("relationshipPropertyName", relationshipPropertyName);
            return this;
        }

        public IEntityQuery<T> Include(Expression<Func<T, object>> relationshipPropertySelector) {
            Arg.NotNull("relationshipPropertySelector", relationshipPropertySelector);
            return this;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() {
            return InnerQuery.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return InnerQuery.GetEnumerator();
        }

        #endregion

        #region IQueryable Members

        public Type ElementType {
            get { return InnerQuery.ElementType; }
        }

        public Expression Expression {
            get { return InnerQuery.Expression; }
        }

        public IQueryProvider Provider {
            get { return InnerQuery.Provider; }
        }
        #endregion
    }
}