// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="VersionConverterTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the VersionConverterTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;
using Maverick.ComponentModel;

namespace Maverick.Tests.ComponentModel {
    [TestClass]
    public class VersionConverterTests {
        private const string TestVersionString = "1.2.345.678";
        private const string TestTooShortVersion = "42";
        private const string TestTooLongVersion = "1.2.3.4.5.6.7.8.9.0";
        private const string TestMajorNegativeVersion = "-43.1.2.3";
        private const string TestMinorNegativeVersion = "1.-43.2.3";
        private const string TestBuildNegativeVersion = "1.2.-43.3";
        private const string TestRevisionNegativeVersion = "1.2.3.-43";
        private const string TestNonIntegerVersion = "a.b.c.d";
        private const string TestLargeComponentVersion = "1.9999999999999999999999999999999999999999999999999999999999999.1.1";

        private const int TestMajor = 1;
        private const int TestMinor = 2;
        private const int TestBuild = 345;
        private const int TestRevision = 678;

        private static readonly Version TestVersion = new Version(TestMajor, TestMinor, TestBuild, TestRevision);
        
        [TestMethod]
        public void ConvertFrom_Successfully_Converts_String_To_Version() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act
            Version converted = (Version)converter.ConvertFrom(TestVersionString);

            // Assert
            Assert.AreEqual(TestMajor, converted.Major);
            Assert.AreEqual(TestMinor, converted.Minor);
            Assert.AreEqual(TestBuild, converted.Build);
            Assert.AreEqual(TestRevision, converted.Revision);
        }

        [TestMethod]
        public void ConvertFrom_Throws_ArgumentException_If_String_Has_Fewer_Than_Two_Version_Components() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            ExceptionAssert.Throws<ArgumentException>(() => converter.ConvertFrom(TestTooShortVersion));
        }

        [TestMethod]
        public void ConvertFrom_Throws_ArgumentException_If_String_Has_More_Than_Four_Version_Components() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            ExceptionAssert.Throws<ArgumentException>(() => converter.ConvertFrom(TestTooLongVersion));
        }

        [TestMethod]
        public void ConvertFrom_Throws_ArgumentOutOfRangeException_If_Major_Is_Negative() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => converter.ConvertFrom(TestMajorNegativeVersion));
        }

        [TestMethod]
        public void ConvertFrom_Throws_ArgumentOutOfRangeException_If_Minor_Is_Negative() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => converter.ConvertFrom(TestMinorNegativeVersion));
        }

        [TestMethod]
        public void ConvertFrom_Throws_ArgumentOutOfRangeException_If_Build_Is_Negative() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => converter.ConvertFrom(TestBuildNegativeVersion));
        }

        [TestMethod]
        public void ConvertFrom_Throws_ArgumentOutOfRangeException_If_Revision_Is_Negative() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            ExceptionAssert.Throws<ArgumentOutOfRangeException>(() => converter.ConvertFrom(TestRevisionNegativeVersion));
        }

        [TestMethod]
        public void ConvertFrom_Throws_FormatException_If_Version_Is_Not_Integer() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            ExceptionAssert.Throws<FormatException>(() => converter.ConvertFrom(TestNonIntegerVersion));
        }

        [TestMethod]
        public void ConvertFrom_Throws_OverflowException_If_Version_Component_Too_Big() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            ExceptionAssert.Throws<OverflowException>(() => converter.ConvertFrom(TestLargeComponentVersion));
        }

        [TestMethod]
        public void ConvertFrom_Throws_NotSupportedException_On_Non_String_Input() {
            // Arrange
            VersionConverter converter = new VersionConverter();
            object toConvert = new object();
            
            // Act and Assert
            ExceptionAssert.Throws<NotSupportedException>(() => converter.ConvertFrom(toConvert));
        }

        [TestMethod]
        public void ConvertFrom_Requires_NonNull_Input() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            AutoTester.ArgumentNull<object>(m => converter.ConvertFrom(m));
        }

        [TestMethod]
        public void ConvertTo_Requires_NonNull_Input() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            AutoTester.ArgumentNull<object>(m => converter.ConvertTo(m, typeof(string)));
        }

        [TestMethod]
        public void ConvertTo_Returns_Version_String_If_Target_Type_Is_String() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act
            string result = (string)converter.ConvertTo(TestVersion,
                                                        typeof(string));

            // Assert
            Assert.AreEqual(TestVersionString, result);
        }

        [TestMethod]
        public void ConvertTo_Throws_NotSupportedException_If_Target_Type_Is_Not_String() {
            // Arrange
            VersionConverter converter = new VersionConverter();

            // Act and Assert
            ExceptionAssert.Throws<NotSupportedException>(() => converter.ConvertTo(TestVersion, typeof(DateTime)));
        }
    }
}
