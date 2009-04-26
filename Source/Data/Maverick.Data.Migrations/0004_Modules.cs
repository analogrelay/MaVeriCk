// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="0004_Modules.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Modules0004 type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using RikMigrations;

namespace Maverick.Data.Migrations
{
    [CLSCompliant(false)]
    [Migration(typeof(Modules0004), 0004, DatabaseModules.Core)]
    public class Modules0004 : IMigration
    {
        public void Up(DbProvider db)
        {
            Table t = db.AddTable("Modules");
            t.AddColumn<int>("Id").PrimaryKey().AutoGenerate().NotNull();
            t.AddColumn<int>("PageId").References("Pages", "Id").NotNull();
            t.AddColumn<string>("ZoneName", 50).NotNull();
            t.AddColumn<Guid>("ModuleApplicationId").NotNull();
            t.AddColumn<string>("Title", 256).NotNull();
            t.Save();
        }

        public void Down(DbProvider db)
        {
            db.DropTable("Modules");
        }
    }
}
