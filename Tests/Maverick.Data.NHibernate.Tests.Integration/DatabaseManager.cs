// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DatabaseManager.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DatabaseManager type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using FluentNHibernate.Cfg.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Maverick.Data.NHibernate.Tests.Integration {
    public static class DatabaseManager {
        // File-based
        public static string GetDatabaseFileName(string testClass, string testName) {
            string directory = Path.Combine("TestDatabases", testClass);
            if(!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            return Path.GetFullPath(Path.Combine(directory,
                                                 String.Format("{0}.db", testName)));
        }

        public static Configuration ConfigureNHibernate(TestContext testContext, Type testClassType) {
            return ConfigureNHibernate(testClassType.Name, testContext.TestName);
        }

        public static Configuration ConfigureNHibernate(string testClass, string testName) {
            return ConfigureNHibernate(GetDatabaseFileName(testClass, testName));
        }

        public static Configuration ConfigureNHibernate(string databaseFile) {
            Configuration cfg = new Configuration();
            SQLiteConfiguration.Standard.UsingFile(databaseFile).ConfigureProperties(cfg);
            return cfg;
        }

        public static void CreateDatabase(Configuration cfg) {
            new SchemaExport(cfg).Create(true, true);
        }

        public static void CleanDatabase(TestContext testContext, Type testClassType) {
            CleanDatabase(testClassType.Name, testContext.TestName);
        }

        public static void CleanDatabase(string testClass, string testName) {
            CleanDatabase(GetDatabaseFileName(testClass, testName));
        }

        public static void CleanDatabase(string databaseFile) {
            File.Delete(databaseFile);
            if(File.Exists(databaseFile)) {
                Assert.Inconclusive("Unable to clean up test file {0}", databaseFile);
            }
        }
    }
}