// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="StringExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the StringExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace TestUtilities {
    public static class StringExtensions {
        public static Stream OpenAsStream(this string str) {
            return str.OpenAsStream(Encoding.UTF8);
        }

        public static Stream OpenAsStream(this string str, Encoding encoder) {
            byte[] testData = encoder.GetBytes(str);
            return new MemoryStream(testData);
        }
    }
}
