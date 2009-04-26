// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Arg.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Arg type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Maverick {
    public static class Arg {
        public static void NotNull(string parameter, object value) {
            if (value == null) {
                throw new ArgumentNullException(parameter);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static void NotNullOrEmpty(string parameter, string value) {
            if (String.IsNullOrEmpty(value)) {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, CommonErrors.StringArgumentNullOrEmpty, parameter), parameter);
            }
        }

        public static void InRange(string parameter, bool rangeCheck) {
            if (!rangeCheck) {
                throw new ArgumentOutOfRangeException(parameter);
            }
        }
    }
}
