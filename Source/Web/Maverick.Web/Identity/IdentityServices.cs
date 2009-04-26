// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IdentityServices.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the IdentityServices type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Maverick.Web.Helpers;

namespace Maverick.Web.Identity {
    public static class IdentityServices {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "There is no use case in which the consumer of this method will have an instance of the type expected by the type parameter")]
        public static string GetSourceName<T>() {
            return GetSourceName(typeof(T));
        }

        public static string GetSourceName(IdentitySource instance) {
            return GetSourceName(instance.GetType());
        }

        public static string GetSourceName(ICustomAttributeProvider type) {
            return (from attr in type.GetCustomAttributes<IdentitySourceAttribute>(true)
                    select attr.Name).SingleOrDefault();
        }
    }
}
