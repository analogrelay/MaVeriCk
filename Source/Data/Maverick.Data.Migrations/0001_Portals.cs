// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="0001_Portals.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Portals0001 type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using RikMigrations;

namespace Maverick.Data.Migrations
{
    [CLSCompliant(false)]
    [Migration(typeof(Portals0001), 0001, DatabaseModules.Core)]
    public class Portals0001 : IMigration
    {
        public void Up(DbProvider db)
        {
            Table t = db.AddTable("Portals");
            t.AddColumn<int>("Id").PrimaryKey().AutoGenerate().NotNull();
            t.AddColumn<string>("Name", 256).NotNull();
            t.Save();
        }

        public void Down(DbProvider db)
        {
            db.DropTable("Portals");
        }
    }
}
