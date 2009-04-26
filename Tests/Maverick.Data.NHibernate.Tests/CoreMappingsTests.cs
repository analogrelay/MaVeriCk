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
using System.Reflection;
using FluentNHibernate.Cfg;
using Maverick.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Data.NHibernate.Tests {
    [TestClass]
    public class CoreMappingsTests {
        private const string TestFilesPath = "TestFiles";
        private const string MappingFileOutputPath = TestFilesPath + @"\Output\NHibernate\Mappings";
        private const string MappingBaselinePath = TestFilesPath + @"\Input\NHibernate\Baselines";
        private const string MappingBaselineResourceNamespace = MappingBaselineResourceAssembly+"TestFiles.NHibernate.MappingBaselines.";
        private const string MappingBaselineResourceAssembly = "Maverick.Data.NHibernate.Tests.";
        
        [TestMethod]
        public void CoreMappings_Generates_Correct_NHibernate_Mapping_Xml() {
            if(!Directory.Exists(MappingFileOutputPath)) {
                Directory.CreateDirectory(MappingFileOutputPath);
            }

            if (!Directory.Exists(MappingBaselinePath)) {
                Directory.CreateDirectory(MappingBaselinePath);
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
            foreach(string fullResourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
                if(fullResourceName.StartsWith(MappingBaselineResourceNamespace)) {
                    string asmRelativeName = fullResourceName.Substring(MappingBaselineResourceAssembly.Length);
                    string folderRelativeName = fullResourceName.Substring(MappingBaselineResourceNamespace.Length);
                    TestFileManager.ExtractTestFile(asmRelativeName, Path.Combine(MappingBaselinePath, folderRelativeName));
                }
            }
            TestFileManager.CompareFilesAgainstBaseline(MappingFileOutputPath, MappingBaselinePath, "*.hbm.xml", (e, a) => {
                FileAssert.TextFilesAreEqual(e, a, TokenTransformer, "The mapping files did not match");
            });
#endif
            Directory.Delete(TestFilesPath, true);
        }

        private static string TokenTransformer(string arg) {
            return arg.Replace("{{ASSEMBLY}}", typeof(Portal).Assembly.GetName().ToString());
        }
    }
}
