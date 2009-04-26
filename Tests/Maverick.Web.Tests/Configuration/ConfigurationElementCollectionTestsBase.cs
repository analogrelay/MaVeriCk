using System.Configuration;
using Maverick.Web.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maverick.Web.Tests.Configuration {
    public abstract class ConfigurationElementCollectionTestsBase<TElement> where TElement : ConfigurationElement {
        protected void RunGetElementTest<T>(CatalogElement element, T expected) {
            // Act
            T actual = (T)CreateCollectionAccessor().AccessGetElementKey(element);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        protected void RunCreateNewElementTest() {
            Assert.IsInstanceOfType(CreateCollectionAccessor().AccessCreateNewElement(), typeof(TElement));
        }

        protected abstract IConfigurationElementCollectionAccessor CreateCollectionAccessor();
    }
}