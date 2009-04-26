// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Guard.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Guard type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace Maverick {
    public static class Guard {
        public static void Against(bool condition, string message, params object[] args) {
            Against(condition, String.Format(CultureInfo.CurrentUICulture, message, args));
        }
        public static void Against(bool condition, string message) {
            if(condition) {
                throw new InvalidOperationException(message);
            }
        }
    }
}
