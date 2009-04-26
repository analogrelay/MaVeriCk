using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Data.EntityFramework.Tests {
    [TestClass]
    public class EntityFrameworkDataContextTests {
        [TestMethod]
        public void Constructor_Requires_NonNull_DbConnection() {
            AutoTester.ArgumentNull<DbConnection>(marker => new EntityFrameworkDataContext(marker));
        }
    }
}
