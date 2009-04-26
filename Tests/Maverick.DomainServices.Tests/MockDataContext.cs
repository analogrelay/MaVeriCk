// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MockDataContext.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MockDataContext type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Maverick.Data;

namespace Maverick.DomainServices.Tests {
    public class MockDataContext : DataContext {
        private IDictionary<Type, Func<object>> _entitySets = new Dictionary<Type, Func<object>>();
        
        public bool ChangesSaved;

        public override IEntitySet<TModel> GetEntitySet<TModel>() {
            return (IEntitySet<TModel>)_entitySets[typeof(TModel)]();
        }

        public override void SaveChanges() {
            ChangesSaved = true;
        }

        public void SetMockEntitySet<TModel>(IEntitySet<TModel> entitySet) {
            _entitySets[typeof(TModel)] = () => entitySet;
        }
    }
}
