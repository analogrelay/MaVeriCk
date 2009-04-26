// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DatabaseTestBase.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DatabaseTestBase type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using TestUtilities;

namespace Maverick.Data.NHibernate.Tests.Integration {
    [TestClass]
    public abstract class DatabaseTestBase<T> where T : class {
        public TestContext TestContext { get; set; }
        public Configuration Configuration { get; set; }
        public NHibernateDataContextFactory DataContextFactory { get; set; }
        public NHibernateDataContext DataContext { get; set; }

        [TestInitialize]
        public void TestInitialize() {
            // Load the NHibernate configuration and mappings
            Configuration = DatabaseManager.ConfigureNHibernate(TestContext, GetType());
            new CoreMappings().ContributeMappings(Configuration);

            // Create the test database
            DatabaseManager.CreateDatabase(Configuration);

            // Setup the data context
            DataContextFactory = new NHibernateDataContextFactory {
                ConfigurationSource = () => Configuration,
            };
            DataContext = (NHibernateDataContext)DataContextFactory.CreateDataContext();
        }

        [TestCleanup]
        public void TestCleanup() {
            if(DataContext != null) {
                DataContext.Dispose();
            }
            DatabaseManager.CleanDatabase(TestContext, GetType());
        }

        protected abstract T CreateEntity();

        protected abstract T CreateNewDetachedDeleteEntity(object id);

        protected abstract object GetId(T entity);

        protected abstract void ModifyEntity(T entity);

        protected abstract void VerifyInserted(T inserted);

        protected abstract void VerifyUpdated(T updated);

        protected abstract T CopyEntity(T entity);
        
        protected void RunSuccessfulInsertTest() {
            // Arrange
            T entity = CreateEntity();
            IEntitySet<T> entitySet = DataContext.GetEntitySet<T>();

            // Act and Assert
            entitySet.InsertOnSave(entity);
            DataContext.SaveChanges();
            Assert.AreEqual(1, entitySet.Count(), "Object was not inserted");
            VerifyInserted(entitySet.Single());
        }

        protected void RunInsertSetsIdTest<TId>() {
            // Arrange
            T entity = CreateEntity();
            IEntitySet<T> entitySet = DataContext.GetEntitySet<T>();

            // Act
            entitySet.InsertOnSave(entity);
            DataContext.SaveChanges();

            // Assert
            Assert.AreNotEqual(default(TId), GetId(entity));
        }

        protected void RunDeleteAttachedTest() {
            // Arrange
            T entity = CreateEntity();
            IEntitySet<T> entitySet = DataContext.GetEntitySet<T>();
            InsertAndVerify(entitySet, entity);

            // Act and Assert
            DeleteAndVerify(entitySet, entity);
        }

        protected void RunDeleteDetachedTest() {
            // Arrange
            T entity = CreateEntity();
            IEntitySet<T> entitySet = DataContext.GetEntitySet<T>();
            InsertAndVerify(entitySet, entity);
            entitySet.Detach(entity);

            // Act and Assert
            DeleteAndVerify(entitySet, entity);
        }

        protected void RunDeleteNewDetachedTest() {
            // Arrange
            T entity = CreateEntity();
            IEntitySet<T> entitySet = DataContext.GetEntitySet<T>();
            InsertAndVerify(entitySet, entity);
            entitySet.Detach(entity);
            T other = CreateNewDetachedDeleteEntity(GetId(entity));
            
            // Act and Assert
            DeleteAndVerify(entitySet, other);
        }

        protected void RunUpdateModifiedTest() {
            // Arrange
            T entity = CreateEntity();
            IEntitySet<T> entitySet = DataContext.GetEntitySet<T>();
            InsertAndVerify(entitySet, entity);
            entitySet.Detach(entity);

            // Act
            ModifyEntity(entity);
            entitySet.UpdateOnSave(entity);
            DataContext.SaveChanges();

            // Assert
            VerifyUpdated(entitySet.Single());
        }

        protected void RunUpdateOriginalAndModifiedTest() {
            // Arrange
            T entity = CreateEntity();
            IEntitySet<T> entitySet = DataContext.GetEntitySet<T>();
            InsertAndVerify(entitySet, entity);
            entitySet.Detach(entity);

            // Act
            T modified = CopyEntity(entity);
            ModifyEntity(modified);
            entitySet.UpdateOnSave(entity, modified);
            DataContext.SaveChanges();

            // Assert
            VerifyUpdated(entitySet.Single());
        }

        protected void RunDetachTest() {
            IEntitySet<T> entitySet = DataContext.GetEntitySet<T>();
            T fetched = RunDetachTest(entitySet);
        }

        protected void RunAttachTest() {
            // Arrange
            IEntitySet<T> entitySet = DataContext.GetEntitySet<T>();
            T fetched = RunDetachTest(entitySet);

            // Act
            entitySet.Attach(fetched);
            
            // Assert
            Assert.IsNotNull(DataContext.Session.GetCurrentLockMode(fetched));
        }
        
        protected void InsertAndVerify(IEntitySet<T> entitySet, T testEntity) {
            InsertAndVerify(entitySet, testEntity, 1);
        }

        protected void InsertAndVerify(IEntitySet<T> entitySet, T testEntity, int expectedCount) {
            entitySet.InsertOnSave(testEntity);
            DataContext.SaveChanges();
            Assert.AreEqual(1, entitySet.Count());
        }

        protected void DeleteAndVerify(IEntitySet<T> entitySet, T testEntity) {
            DeleteAndVerify(entitySet, testEntity, 0);
        }

        protected void DeleteAndVerify(IEntitySet<T> entitySet, T testEntity, int expectedCount) {
            entitySet.DeleteOnSave(testEntity);
            DataContext.SaveChanges();
            Assert.AreEqual(0, entitySet.Count());
        }

        private T RunDetachTest(IEntitySet<T> entitySet) {
            T entity = CreateEntity();
            InsertAndVerify(entitySet, entity);
            T fetched = entitySet.Single();
            Assert.IsNotNull(DataContext.Session.GetCurrentLockMode(fetched));
            entitySet.Detach(fetched);
            ExceptionAssert.Throws<TransientObjectException>(() => DataContext.Session.GetCurrentLockMode(fetched));
            return fetched;
        }
    }
}
