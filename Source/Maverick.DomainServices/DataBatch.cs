// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataBatch.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataBatch type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using Maverick.Data;
using Maverick.DomainServices.Properties;

namespace Maverick.DomainServices {
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "The standard disposable implementation is not suitable for this DataBatch object")]
    public class DataBatch : IDisposable {
        public static DataBatch Start() {
            Guard.Against(Current != null, Resources.Error_DataBatchAlreadyRunning);
            Guard.Against(DataContextManager == null, Resources.Error_NoDataContextManager);
            
            DataContext context = DataContextManager.GetCurrentDataContext();
            Guard.Against(context == null, Resources.Error_NoDataContext);

            Current = new DataBatch(context);
            return Current;
        }

        public static DataBatch Current { get; set; }

        public static DataContextManager DataContextManager {
            get; set;
        }

        public static void SaveCurrentBatch() {
            if (Current != null) {
                Current.SaveChanges();
            }
        }

        public DataContext Context { get; protected set; }

        public DataBatch(DataContext context) {
            Arg.NotNull("context", context);
            Context = context;
        }

        public void SaveChanges() {
            if (Context != null) {
                Context.SaveChanges();
            }
            Current = null;
        }

        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification="The standard disposable implementation is not suitable for this DataBatch object")]
        [SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "The standard disposable implementation is not suitable for this DataBatch object")]
        public virtual void Dispose() {
            if (Context != null) {
                Context.Dispose();
            }
            Current = null;
        }
    }
}
