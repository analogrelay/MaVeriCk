// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MockEntitySet.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MockEntitySet type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Maverick.Data;

namespace Maverick.Tests {
    public class MockEntitySet<T> : IEntitySet<T> {
        private readonly IQueryable<T> _queryable;

        public class Modification {
            public T Original;
            public T Modified;
        }

        public List<T> Models { get; private set; }
        public List<T> Inserted { get; private set; }
        public List<T> Deleted { get; private set; }
        public List<Modification> Updated { get; private set; }
        public List<string> Included { get; private set; }
        public List<T> Attached { get; private set; }
        public List<T> Detached { get; private set; }
        
        public MockEntitySet() {
            Models = new List<T>();
            Inserted = new List<T>();
            Deleted = new List<T>();
            Updated = new List<Modification>();
            Included = new List<string>();
            Attached = new List<T>();
            Detached = new List<T>();
            _queryable = Models.AsQueryable();
        }

        public IEnumerator<T> GetEnumerator() {
            return Models.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public Expression Expression { get { return _queryable.Expression; } }
        public Type ElementType { get { return _queryable.ElementType; } }
        public IQueryProvider Provider { get { return _queryable.Provider; } }

        public void Add(IEnumerable<T> models) {
            foreach (T model in models) {
                Add(model);
            }
        }

        public void Add(T model) {
            Models.Add(model);
        }

        public string ModelSetName { get; set; }
        public string ModelContainerName { get; set; }
        
        public void InsertOnSave(T model) {
            Inserted.Add(model);
        }
        
        public void DeleteOnSave(T model) {
            Deleted.Add(model);
        }
        
        public void UpdateOnSave(T modified) {
            Updated.Add(new Modification { Original = modified, Modified = modified });
        }

        public void UpdateOnSave(T original, T modified) {
            Updated.Add(new Modification { Original = original, Modified = modified });
        }

        public void Attach(T original) {
            Attached.Add(original);
        }

        public void Detach(T model) {
            Detached.Add(model);
        }

        public IEntityQuery<T> Include(string relationshipPropertyName) {
            Included.Add(relationshipPropertyName);
            return this;
        }

        public IEntityQuery<T> Include(Expression<Func<T, object>> relationshipPropertySelector) {
            MemberExpression me = relationshipPropertySelector.Body as MemberExpression;
            if (me == null)
                throw new NotSupportedException("MemberException expected.");

            if (me.Expression.NodeType != ExpressionType.Parameter)
                throw new NotSupportedException("Paramter expected");

            if (relationshipPropertySelector.Parameters[0] != me.Expression)
                throw new NotSupportedException("Invalid parameter.");

            return Include(me.Member.Name);
        }
    }
}
