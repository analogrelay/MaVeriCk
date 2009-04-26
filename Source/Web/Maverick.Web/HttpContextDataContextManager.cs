// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HttpContextDataContextManager.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the HttpContextDataContextManager type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Web;
using Maverick.Data;
using Maverick.Web.Properties;

namespace Maverick.Web {
    [Export(typeof(DataContextManager))]
    [DataContextManager("HttpContext", ApplicationInfo.Version, "Manages Database Sessions within a single Http Request", ApplicationInfo.Vendor)]
    public class HttpContextDataContextManager : DataContextManager {
        internal const string ContextKey = "Maverick.Web.HttpContextDataContextManager.DataContext";

        [Import]
        public Func<HttpContextBase> HttpContextSource { get; set; }

        [Import]
        public DataContextBuilder DataContextBuilder { get; set; }

        public override DataContext GetCurrentDataContext() {
            Guard.Against(HttpContextSource == null, Resources.Error_NoHttpContextSource);
            Guard.Against(DataContextBuilder == null, Resources.Error_NoDataContextBuilder);
            
            HttpContextBase context = HttpContextSource();

            if(!context.Items.Contains(ContextKey)) {
                DataContext dataContext = DataContextBuilder.CreateDataContext();
                Guard.Against(dataContext == null, Resources.Error_DataContextBuilderReturnedNull);
                context.Items.Add(ContextKey, dataContext);
            }

            return context.Items[ContextKey] as DataContext;
        }
    }
}