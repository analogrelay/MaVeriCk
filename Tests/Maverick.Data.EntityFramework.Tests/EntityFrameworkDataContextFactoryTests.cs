using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Maverick.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Data.EntityFramework.Tests {
    [TestClass]
    public class EntityFrameworkDataContextFactoryTests {
        [TestMethod]
        public void Constructor_Requires_NonNull_ConnectionSource() {
            AutoTester.ArgumentNull<Func<DbConnection>>(marker => new EntityFrameworkDataContextFactory(marker));
        }

        [TestMethod]
        public void DataContextFactory_Is_Exported_To_CompositionContainer() {
            CompositionAssert.IsExported(typeof(EntityFrameworkDataContextFactory), typeof(DataContextFactory));
        }

        [TestMethod]
        public void Constructor_With_ConnectionSource_Is_Importing_Constructor() {
            Func<DbConnection> connectionSource = () => null;
            CompositionAssert.IsImportingConstructor(() => new EntityFrameworkDataContextFactory(connectionSource));
        }
    }
}
