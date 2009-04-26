// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Program.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Reflection;
using RikMigrations;

namespace Maverick.Data.Migrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Migrating database to maximum version");
            DbProvider.DefaultConnectionString =
                ConfigurationManager.ConnectionStrings["TargetConnection"].ConnectionString;
            Console.WriteLine("\tConnection String: {0}", DbProvider.DefaultConnectionString);
            MigrationManager.UpgradeMax(Assembly.GetExecutingAssembly());
        }
    }
}
