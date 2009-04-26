// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="0003_Pages.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the Pages0003 type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using RikMigrations;

namespace Maverick.Data.Migrations
{
    [CLSCompliant(false)]
    [Migration(typeof(Pages0003), 0003, DatabaseModules.Core)]
    public class Pages0003 : IMigration
    {
        public void Up(DbProvider db)
        {
            Table t = db.AddTable("Pages");
            t.AddColumn<int>("Id").PrimaryKey().AutoGenerate().NotNull();
            t.AddColumn<int>("PortalId").References("Portals", "Id").NotNull();
            t.AddColumn<int>("ParentId").References("Pages", "Id");
            t.AddColumn<string>("Path", 1024).NotNull();
            t.AddColumn<string>("Title", 256).NotNull();
            t.Save();
        }

        public void Down(DbProvider db)
        {
            db.DropTable("Pages");
        }
    }
}
