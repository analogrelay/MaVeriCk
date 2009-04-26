// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SimpleDomainService.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the SimpleDomainService type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using Maverick.Data;
using Maverick.DomainServices.Properties;

namespace Maverick.DomainServices {
    public abstract class RepositoryBase<TModel> where TModel : class {
        private DataContextManager _dataContextManager;

        public virtual DataContextManager DataContextManager {
            get {
                if(_dataContextManager == null) {
                    return DataBatch.DataContextManager;
                }
                return _dataContextManager;
            }
            set { _dataContextManager = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Since this code results in the creation of a database query, it is not appropriate to make it a property")]
        public virtual IEntityQuery<TModel> GetAll() {
            return GetEntitySet(GetContext());
        }

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "This method does not attach a listener, thus it is not covered by this rule")]
        public virtual void Add(TModel toAdd) {
            Arg.NotNull("toAdd", toAdd);

            RunCommand(s => s.InsertOnSave(toAdd));
        }

        public virtual void Delete(TModel toDelete) {
            Arg.NotNull("toDelete", toDelete);

            RunCommand(s => s.DeleteOnSave(toDelete));
        }

        public virtual void Update(TModel modified) {
            Arg.NotNull("modified", modified);

            RunCommand(s => s.UpdateOnSave(modified));
        }

        public virtual void Update(TModel original, TModel modified) {
            Arg.NotNull("original", original);
            Arg.NotNull("modified", modified);

            RunCommand(s => s.UpdateOnSave(original, modified));
        }

        protected virtual IEntitySet<TModel> GetEntitySet(DataContext dataContext) {
            return dataContext.GetEntitySet<TModel>();
        }

        private void RunCommand(Action<IEntitySet<TModel>> command) {
            DataContext context = null;
            
            // Are we in a data batch?
            bool inBatch;
            context = GetContext(out inBatch);

            // Run the command
            command(GetEntitySet(context));

            // If we're not batched, save changes
            if(!inBatch) {
                context.SaveChanges();
            }
        }

        private DataContext GetContext() {
            bool inBatch;
            return GetContext(out inBatch);
        }

        private DataContext GetContext(out bool inBatch) {
            DataContext context;
            if (DataBatch.Current != null) {
                // Use that context
                context = DataBatch.Current.Context;
                inBatch = true;
            } else {
                // Is there a Data Context Manager?
                if(DataContextManager == null) {
                    throw new InvalidOperationException(Resources.Error_NoDataContextManager);
                }

                // Get the current Data Context
                context = DataContextManager.GetCurrentDataContext();
                inBatch = false;
            }

            // Verify the Data Context
            if (context == null) {
                throw new InvalidOperationException(Resources.Error_NoDataContext);
            }

            return context;
        }
    }
}