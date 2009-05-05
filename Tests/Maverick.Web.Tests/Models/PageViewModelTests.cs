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
using Maverick.Web.ModuleFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maverick.Web.Models;
using TestUtilities;

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

        [TestMethod]
        public void AllModules_Returns_Aggregation_Of_All_Modules_In_All_Zones() {
            // Arrange
            PageViewModel model = SetupAllModulesTestModel();

            // Act
            IEnumerable<ModuleRequestResult> moduleResults = model.AllModules;

            // Assert
            Assert.AreEqual(5, moduleResults.Count());
            EnumerableAssert.ElementsMatch(model.Zones.SelectMany(z => z.ModuleResults),
                                           moduleResults,
                                           ReferenceEquals);
        }

        [TestMethod]
        public void AllModules_Returns_ControlPanelModule_First_If_Present() {
            // Arrange
            PageViewModel model = SetupAllModulesTestModel();
            model.ControlPanelResult = new ModuleRequestResult();

            // Act
            IEnumerable<ModuleRequestResult> moduleResults = model.AllModules;

            // Assert
            Assert.AreEqual(6, moduleResults.Count());
            Assert.AreSame(model.ControlPanelResult, moduleResults.First());
        }

        private static PageViewModel SetupAllModulesTestModel() {
            PageViewModel model = new PageViewModel();
            ZoneViewModel zone1 = new ZoneViewModel();
            zone1.ModuleResults.Add(new ModuleRequestResult());
            zone1.ModuleResults.Add(new ModuleRequestResult());
            zone1.ModuleResults.Add(new ModuleRequestResult());
            ZoneViewModel zone2 = new ZoneViewModel();
            zone2.ModuleResults.Add(new ModuleRequestResult());
            zone2.ModuleResults.Add(new ModuleRequestResult());
            model.Zones.Add(zone1);
            model.Zones.Add(zone2);
            return model;
        }
    }
}
