namespace Maverick.Web.Tests.Configuration {
    public interface IConfigurationElementAccessor {
        object GetProperty(string name);
        void SetProperty(string name, object value);
    }
}