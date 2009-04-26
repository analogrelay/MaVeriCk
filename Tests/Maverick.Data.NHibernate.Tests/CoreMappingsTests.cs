// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CoreMappingsTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CoreMappingsTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

// #define GENERATE_BASELINES

using System;
using System.IO;
using FluentNHibernate.Cfg;
using Maverick.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Data.NHibernate.Tests {
    [TestClass]
    public class CoreMappingsTests {
        private const string MappingFileOutputPath = @"TestOutput\NHibernate\Mappings";
        private const string MappingBaselinePath = @"TestFiles\NHibernate\MappingBaselines";

        [TestMethod]
        public void CoreMappings_Generates_Correct_NHibernate_Mapping_Xml() {
            if(!Directory.Exists(MappingFileOutputPath)) {
                Directory.CreateDirectory(MappingFileOutputPath);
            }

            // Arrange
            MappingConfiguration configuration = new MappingConfiguration();
            CoreMappings mappings = new CoreMappings();
            mappings.ContributeMappings(configuration);
            configuration.FluentMappings.ExportTo(MappingFileOutputPath);
            
            // Act
            // NOTE: Exceptions should only occur AFTER export, since Configuration only occurs after export.
            // NOTE: So, any unhandled exceptions here should simply be suppressed.
            try {
                configuration.Apply(null);
            } catch(ApplicationException) {} // All the exceptions we want to suppress are wrapped in ApplicationExceptions

            // Assert
#if !GENERATE_BASELINES
            TestFileManager.CompareFilesAgainstBaseline(MappingFileOutputPath, MappingBaselinePath, "*.hbm.xml", (e, a) => {
                FileAssert.TextFilesAreEqual(e, a, TokenTransformer, "The mapping files did not match");
            });
#endif
        }

        private static string TokenTransformer(string arg) {
            return arg.Replace("{{ASSEMBLY}}", typeof(Portal).Assembly.GetName().ToString());
        }
    }
}
