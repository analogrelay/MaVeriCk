// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TestFileManager.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the TestFileManager type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    // When creating helpers inside utility assemblies that take advantage of these helpers, be sure to
    // using Assembly.GetCallingAssembly() to get the assembly context of the caller. Once you call
    // into this code, your utility assembly becomes the result of Assembly.GetCallingAssembly()!!
    public static class TestFileManager {
        public static string GetTestOutputPath() {
            return GetTestOutputPath(".");
        }

        public static string GetTestOutputPath(string testFile) {
            // Find the stack frame for the test method and use it as the context
            return GetTestOutputPath(StackHelper.FindTestMethodCaller(), testFile);
        }

        public static string GetTestOutputPath(StackFrame context, string testFile) {
            MethodBase method = context.GetMethod();
            return GetTestOutputPath(method.DeclaringType.Assembly.GetName().Name, method.DeclaringType.FullName, method.Name, testFile);
        }

        public static string GetTestOutputPath(string testAssembly, string testClass, string testMethod, string testFile) {
            // Strip the assembly name off the front of the type name
            if(testClass.StartsWith(testAssembly)) {
                testClass = testClass.Substring(testAssembly.Length + 1);
            }
            
            // First "_" segment is the class being tested, can safely remove that
            testMethod = testMethod.Substring(testMethod.IndexOf("_") + 1);

            string dir = String.Format("{0}\\{1}\\{2}", testAssembly, testClass, testMethod);
            if(!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            return Path.GetFullPath(String.Format("{0}\\{1}", dir, testFile));
        }

        public static string GetFullTestFilePath(string category, string file) {
            if (!Directory.Exists(category)) {
                Directory.CreateDirectory(category);
            }
            return Path.GetFullPath(String.Format("{0}\\{1}", category, file));
        }

        public static string LoadTestFile(string category, string file) {
            return LoadTestFile(Assembly.GetCallingAssembly(), String.Format("{0}.TestFiles.{1}", category, file));
        }

        public static string LoadTestFile(string fullResourceName) {
            return LoadTestFile(Assembly.GetCallingAssembly(), fullResourceName);
        }

        public static string LoadTestFile(Assembly context, string resourceName) {
            using (Stream strm = OpenTestFile(context, resourceName)) {
                using (StreamReader reader = new StreamReader(strm)) {
                    return reader.ReadToEnd();
                }
            }
        }

        public static Stream OpenTestFile(string category, string file) {
            return OpenTestFile(Assembly.GetCallingAssembly(), String.Format("{0}.TestFiles.{1}", category, file));
        }

        public static Stream OpenTestFile(string resourceName) {
            return OpenTestFile(Assembly.GetCallingAssembly(), resourceName);
        }

        public static Stream OpenTestFile(Assembly context, string resourceName) {
            Assert.AreNotEqual("DotNetNuke.Integrity.Tests.Utilities", context.GetName().Name, "Test Utility assembly does not contain any test files, did you forget to provide the proper context when writing a custom utility function?");
            resourceName = String.Format("{0}.{1}", context.GetName().Name, resourceName);
            Stream strm = context.GetManifestResourceStream(resourceName);
            if (strm == null) {
                throw new FileNotFoundException(String.Format("Could not load test file: {0}", resourceName));
            }

            return strm;
        }

        public static string ExtractTestFile(string category, string file, string targetFile) {
            return ExtractTestFile(Assembly.GetCallingAssembly(), String.Format("{0}.TestFiles.{1}", category, file), targetFile);
        }

        public static string ExtractTestFile(string fullResourceName, string targetFile) {
            return ExtractTestFile(Assembly.GetCallingAssembly(), fullResourceName, targetFile);
        }

        public static string ExtractTestFile(Assembly context, string resourceName, string targetFile) {
            string data = LoadTestFile(context, resourceName);

            string fullTestFileName = Path.GetFullPath(targetFile);
            using (Stream strm = new FileStream(fullTestFileName, FileMode.CreateNew, FileAccess.ReadWrite)) {
                using (StreamWriter writer = new StreamWriter(strm)) {
                    writer.Write(data);
                }
            }

            return fullTestFileName;
        }

        public static string CreateTestFileFromString(string content) {
            string fileName = Path.GetFullPath(Path.GetRandomFileName());
            Assert.IsFalse(File.Exists(fileName), "Random file name existed?!?");
            CreateTestFileFromString(fileName, content);
            return fileName;
        }

        public static void CreateTestFileFromString(string path, string content) {
            using (StreamWriter writer = File.CreateText(path)) {
                writer.Write(content);
            }
        }

        public static void CompareFilesAgainstBaseline(string testOutputDirectory, string baselineDirectory) {
            CompareFilesAgainstBaseline(testOutputDirectory, baselineDirectory, null);
        }

        public static void CompareFilesAgainstBaseline(string testOutputDirectory, string baselineDirectory, string searchPattern) {
            CompareFilesAgainstBaseline(testOutputDirectory, baselineDirectory, searchPattern, (output, baseline) => FileAssert.TextFilesAreEqual(baseline, output));
        }

        public static void CompareFilesAgainstBaseline(string testOutputDirectory, string baselineDirectory, string searchPattern, Action<string, string> asserter) {
            DirectoryInfo testOutputs = new DirectoryInfo(testOutputDirectory);
            DirectoryInfo baselines = new DirectoryInfo(baselineDirectory);

            FileInfo[] files;
            if(String.IsNullOrEmpty(searchPattern)) {
                files = testOutputs.GetFiles();
            } else {
                files = testOutputs.GetFiles(searchPattern);
            }

            foreach(FileInfo file in files) {
                string baselineFile = Path.Combine(baselines.FullName, file.Name);
                Assert.IsTrue(File.Exists(baselineFile), "No baseline file: {0}", baselineFile);
                asserter(baselineFile, file.FullName);
            }
        }
    }
}
