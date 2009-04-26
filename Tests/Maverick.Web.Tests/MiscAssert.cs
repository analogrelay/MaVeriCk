// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MiscAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MiscAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using Maverick.Web.Identity;
using TestUtilities;

namespace Maverick.Web.Tests {
    public class MiscAssert {
        public static void HasIdentitySourceName(Type identitySourceType, string name) {
            AttributeAssert.IsDefined<IdentitySourceAttribute>(identitySourceType,
                                                               attr =>
                                                               String.Equals(attr.Name,
                                                                             name,
                                                                             StringComparison.OrdinalIgnoreCase));
        }
    }
}
