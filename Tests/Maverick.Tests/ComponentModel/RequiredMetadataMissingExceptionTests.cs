// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="RequiredMetadataMissingExceptionTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the RequiredMetadataMissingExceptionTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Maverick.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Tests.ComponentModel {
    [TestClass]
    public class RequiredMetadataMissingExceptionTests {
        private static readonly Exception TestInnerException = new Exception("Bar");
        private const string TestMessage = "Foo";
        private const string DefaultMessage = "Exception of type 'Maverick.ComponentModel.RequiredMetadataMissingException' was thrown.";

        [TestMethod]
        public void DefaultConstructor_Initializes_Properties_To_Defaults() {
            RequiredMetadataMissingException ex = new RequiredMetadataMissingException();
            Assert.AreEqual(DefaultMessage, ex.Message);
            Assert.IsNull(ex.InnerException);
        }

        [TestMethod]
        public void Constructor_With_Message_Initializes_Message_Property() {
            RequiredMetadataMissingException ex = new RequiredMetadataMissingException(TestMessage);
            Assert.AreEqual(TestMessage, ex.Message);
            Assert.IsNull(ex.InnerException);
        }

        [TestMethod]
        public void Constructor_With_Message_And_InnerException_Initializes_InnerException_Property() {
            RequiredMetadataMissingException ex = new RequiredMetadataMissingException(TestMessage, TestInnerException);
            Assert.AreEqual(TestMessage, ex.Message);
            Assert.AreSame(TestInnerException, ex.InnerException);
        }

        [TestMethod]
        public void Exception_Is_Serializable() {
            // Arrange
            RequiredMetadataMissingException expected = new RequiredMetadataMissingException(TestMessage, TestInnerException);
            RequiredMetadataMissingException actual;

            // Act
            byte[] buffer;
            using (MemoryStream strm = new MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(strm, expected);
                strm.Flush();
                buffer = strm.GetBuffer();
            }

            using(MemoryStream strm = new MemoryStream(buffer)) {
                // Create new formatter for deserialization just to be careful
                BinaryFormatter formatter = new BinaryFormatter();
                actual = (RequiredMetadataMissingException)formatter.Deserialize(strm);
            }

            // Assert
            Assert.AreEqual(expected.Message, actual.Message);
            // The objects aren't actually equal, but the should have been serialized
            Assert.AreEqual(expected.InnerException.Message, actual.InnerException.Message);
        }
    }
}
