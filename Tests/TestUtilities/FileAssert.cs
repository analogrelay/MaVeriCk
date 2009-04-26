// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="FileAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the FileAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public static class FileAssert {
        public static void TextFilesAreEqual(string expectedFile, string actualFile) {
            TextFilesAreEqual(expectedFile, actualFile, String.Empty);
        }

        public static void TextFilesAreEqual(string expectedFile, string actualFile, string message) {
            TextFilesAreEqual(expectedFile, actualFile, s => s, message);
        }

        public static void TextFilesAreEqual(string expectedFile, string actualFile, Func<string, string> expectedContentTransform, string message) {
            string expectedContent = File.ReadAllText(expectedFile);
            string actualContent = File.ReadAllText(actualFile);
            Assert.AreEqual(expectedContentTransform(expectedContent), actualContent, message);
        }

        public static void BinaryFilesAreEqual(string expectedFile, string actualFile) {
            BinaryFilesAreEqual(expectedFile, actualFile, String.Empty);
        }

        public static void BinaryFilesAreEqual(string expectedFile, string actualFile, string message) {
            byte[] expectedContent = File.ReadAllBytes(expectedFile);
            byte[] actualContent = File.ReadAllBytes(actualFile);
            EnumerableAssert.ElementsAreEqual(expectedContent, actualContent, message);
        }
    }
}
