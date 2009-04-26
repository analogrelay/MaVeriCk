// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CustomAttributeProviderExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CustomAttributeProviderExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Maverick.Web.Helpers {
    public static class CustomAttributeProviderExtensions {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "There is no use case where the consumer will have an instance of the type expected in the type parameter")]
        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider type, bool inherit) where T : Attribute {
            return type.GetCustomAttributes(typeof(T), inherit).OfType<T>();
        }
    }
}