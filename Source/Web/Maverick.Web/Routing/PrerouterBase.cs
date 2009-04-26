// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PrerouterBase.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PrerouterBase type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Routing;

namespace Maverick.Web.Routing {
    public abstract class PrerouterBase : RouteBase {
        private RouteCollection _routeCollection;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Clients must be able to completely override the RouteCollection used")]
        public RouteCollection RouteCollection {
            get {
                if(_routeCollection == null) {
                    // Get the default collection
                    _routeCollection = RouteTable.Routes;
                }
                return _routeCollection;
            }
            set { _routeCollection = value; }
        }

        protected virtual TResult RerouteRequest<TResult>(Func<RouteBase, TResult> request) where TResult : class {
            // NOTE: No need to worry about RouteExistingFiles, as the RouteTable which called us should handle that
            // Lock the collection for reading
            using (RouteCollection.GetReadLock()) {
                bool startRouting = false;
                foreach (RouteBase route in RouteCollection) {
                    // The first route we're going to call is the one after this route
                    if(route == this) {
                        startRouting = true;
                    } else if (startRouting) {
                        TResult result = request(route);
                        if (result != null) {
                            return result;
                        }
                    }
                }
            }
            return null;
        }
    }
}