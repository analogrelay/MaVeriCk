// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PageViewModelTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PageViewModelTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maverick.Web.Models;

namespace Maverick.Web.Tests.Models {
    [TestClass]
    public class PageViewModelTests {
        [TestMethod]
        public void Constructor_Initializes_Zones_Property() {
            Assert.IsNotNull(new PageViewModel().Zones);
        }

        [TestMethod]
        public void Indexer_Creates_New_Zone_If_Zone_Does_Not_Exist() {
            Assert.IsNotNull(new PageViewModel()["foo"]);
        }

        [TestMethod]
        public void Indexer_Returns_Existing_Zone_If_Zone_Does_Exist() {
            // Arrange
            PageViewModel model = new PageViewModel();
            ZoneViewModel expected = new ZoneViewModel() {ZoneName = "foo"};
            model.Zones.Add(expected);

            // Act
            ZoneViewModel actual = model["foo"];
            
            // Assert
            Assert.AreSame(expected, actual);
        }
    }
}
