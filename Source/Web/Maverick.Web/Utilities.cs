// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Utilities.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Utilities type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;

namespace Maverick.Web {
    internal static class Utilities {
        internal static void BuildUrlRoot(Uri url, StringBuilder builder, bool includePort) {
            builder.Append(url.Scheme);
            builder.Append("://");
            builder.Append(url.Host);
            if(includePort) {
                builder.Append(":");
                builder.Append(url.Port);
            }
        }
    }
}
