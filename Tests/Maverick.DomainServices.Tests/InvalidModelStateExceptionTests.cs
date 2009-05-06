// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="InvalidModelStateException" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the InvalidModelStateException type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TargetResources = Maverick.DomainServices.Properties.Resources;

namespace Maverick.DomainServices.Tests {
    [TestClass]
    public class InvalidModelStateExceptionTests {
        [TestMethod]
        public void Constructor_Sets_Default_Message_If_None_Specified() {
            // Arrange/Act
            InvalidModelStateException ex = new InvalidModelStateException();

            // Assert
            Assert.AreEqual(TargetResources.Error_ModelStateInvalid, ex.Message);
        }

        [TestMethod]
        public void Constructor_Sets_Message_If_Specified() {
            // Arrange/Act
            const string expected = "FooBarBaz";
            InvalidModelStateException ex= new InvalidModelStateException(expected);

            // Assert
            Assert.AreEqual(expected, ex.Message);
        }

        [TestMethod]
        public void Constructor_Sets_InnerException_If_Specified() {
            // Arrange/Act
            Exception expected = new Exception();
            InvalidModelStateException ex = new InvalidModelStateException(String.Empty, expected);

            // Assert
            Assert.AreSame(expected, ex.InnerException);
        }
    }
}
