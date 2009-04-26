// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="0002_PortalPrefixes.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalPrefixes0002 type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using RikMigrations;

namespace Maverick.Data.Migrations
{
    [CLSCompliant(false)]
    [Migration(typeof(PortalPrefixes0002), 0002, DatabaseModules.Core)]
    public class PortalPrefixes0002 : IMigration
    {
        public void Up(DbProvider db)
        {
            Table t = db.AddTable("PortalPrefixes");
            t.AddColumn<int>("Id").PrimaryKey().AutoGenerate().NotNull();
            t.AddColumn<int>("PortalId").References("Portals", "Id").NotNull();
            t.AddColumn<string>("Prefix", 256).NotNull();
            t.Save();
        }

        public void Down(DbProvider db)
        {
            db.DropTable("PortalPrefixes");
        }
    }
}
