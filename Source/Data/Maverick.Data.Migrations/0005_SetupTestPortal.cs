using System;
using RikMigrations;

namespace Maverick.Data.Migrations {
    [CLSCompliant(false)]
    [Migration(typeof(SetupTestPortal0005), 0005, DatabaseModules.Core)]
    public class SetupTestPortal0005 : IMigration {
        public void Up(DbProvider db) {
            db.InsertDataInto("Portals",
                              new {
                                  Name = "Test Portal"
                              },
                              false);
            db.GetLastId("Portals");
            db.InsertDataInto("PortalPrefixes",
                              new {
                                  Id = 1,
                                  PortalId = 1,
                                  Prefix = "localhost/"
                              },
                              true);
            db.InsertDataInto("PortalPrefixes",
                              new {
                                  Id = 2,
                                  PortalId = 1,
                                  Prefix = "localhost/Maverick"
                              },
                              true);
            db.InsertDataInto("Pages",
                              new {
                                  Id = 1,
                                  PortalId = 1,
                                  Path = "/",
                                  Title = "Test Page"
                              },
                              true);
        }

        public void Down(DbProvider db) {
            db.ExecuteScalar("DELETE FROM Modules");
            db.ExecuteScalar("DELETE FROM Pages");
            db.ExecuteScalar("DELETE FROM PortalPrefixes");
            db.ExecuteScalar("DELETE FROM Portals");
        }
    }
}
