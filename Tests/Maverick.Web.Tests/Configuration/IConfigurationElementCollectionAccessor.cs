using System.Configuration;

namespace Maverick.Web.Tests.Configuration {
    public interface IConfigurationElementCollectionAccessor {
        ConfigurationElement AccessCreateNewElement();
        object AccessGetElementKey(ConfigurationElement element);
    }
}