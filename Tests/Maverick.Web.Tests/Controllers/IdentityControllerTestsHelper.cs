using System.Collections.Generic;
using System.ComponentModel.Composition;
using Maverick.ComponentModel;
using Maverick.Web.Identity;
using Moq;

namespace Maverick.Web.Tests.Controllers {
    internal static class IdentityControllerTestsHelper {
        public static Mock<IdentitySource> AddMock(this ComponentCollection<IdentitySource> collection, 
                                                   string name) {
            Mock<IdentitySource> mockSource = new Mock<IdentitySource>();
            Add(collection, name, mockSource.Object);
            return mockSource;
        }

        public static void Add(this ComponentCollection<IdentitySource> collection, 
                               string name, 
                               IdentitySource source) {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata.Add("Name", name);
            var export = new Export<IdentitySource, ComponentMetadata>(metadata, () => source);
            collection.Add(export);
        }
    }
}