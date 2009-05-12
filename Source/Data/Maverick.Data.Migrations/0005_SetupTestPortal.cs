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
            int portalId = db.GetLastId("Portals");

            db.InsertDataInto("PortalPrefixes",
                              new {
                                  PortalId = portalId,
                                  Prefix = "localhost/"
                              },
                              false);
            db.InsertDataInto("PortalPrefixes",
                              new {
                                  PortalId = portalId,
                                  Prefix = "localhost/Maverick"
                              },
                              false);
            db.InsertDataInto("Pages",
                              new {
                                  PortalId = portalId,
                                  Path = "/",
                                  Title = "Test Page"
                              },
                              false);
        }

        public void Down(DbProvider db) {
            db.ExecuteScalar("DELETE FROM Modules");
            db.ExecuteScalar("DELETE FROM Pages");
            db.ExecuteScalar("DELETE FROM PortalPrefixes");
            db.ExecuteScalar("DELETE FROM Portals");
        }
    }
}
