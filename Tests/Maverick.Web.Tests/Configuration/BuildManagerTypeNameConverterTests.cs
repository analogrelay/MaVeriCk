using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Maverick.Web.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestUtilities;

using TargetResources = Maverick.Web.Configuration.Properties.Resources;

namespace Maverick.Web.Tests.Configuration {
    [TestClass]
    public class BuildManagerTypeNameConverterTests {
        private const string TestTypeName = "Foo.Bar.Baz";

        [TestMethod]
        public void ConvertFrom_Throws_NotSupportedException_If_Value_Is_Not_String() {
            // Arrange
            BuildManagerTypeNameConverter converter = new BuildManagerTypeNameConverter();

            // Act and Assert
            ExceptionAssert.Throws<NotSupportedException>(() => converter.ConvertFrom(new object()));
        }

        [TestMethod]
        public void ConvertTo_Throws_NotSupportedException_If_DestinationType_Is_Not_String() {
            // Arrange
            BuildManagerTypeNameConverter converter = new BuildManagerTypeNameConverter();

            // Act and Assert
            ExceptionAssert.Throws<NotSupportedException>(() => converter.ConvertTo(typeof(object), typeof(DateTime)));
        }

        [TestMethod]
        public void ConvertTo_Throws_NotSupportedException_If_Value_Is_Not_Type() {
            // Arrange
            BuildManagerTypeNameConverter converter = new BuildManagerTypeNameConverter();

            // Act and Assert
            ExceptionAssert.Throws<NotSupportedException>(() => converter.ConvertTo(new object(), typeof(DateTime)));
        }

        [TestMethod]
        public void ConvertTo_Returns_AssemblyQualifiedName_If_Value_Is_Type_And_Destination_Is_String() {
            // Arrange
            BuildManagerTypeNameConverter converter = new BuildManagerTypeNameConverter();

            // Act
            string actual = (string)converter.ConvertTo(typeof(Version), typeof(string));

            // Assert
            Assert.AreEqual(typeof(Version).AssemblyQualifiedName, actual);
        }

        [TestMethod]
        public void ConvertFrom_Throws_ArgumentException_If_TypeResolver_Cannot_Resolve_Type() {
            // Arrange
            BuildManagerTypeNameConverter converter = new BuildManagerTypeNameConverter();
            converter.TypeResolver = s => null;
            
            // Act and Assert
            ExceptionAssert.Throws<ArgumentException>(() => converter.ConvertFrom(TestTypeName),
                                                      ex => ex.ParamName == "value",
                                                      String.Format(CultureInfo.CurrentUICulture,
                                                                    TargetResources.Error_CouldNotResolveType,
                                                                    TestTypeName) + Environment.NewLine + "Parameter name: value");
        }

        [TestMethod]
        public void ConvertFrom_Returns_Type_From_Resolver() {
            // Arrange
            BuildManagerTypeNameConverter converter = new BuildManagerTypeNameConverter();
            converter.TypeResolver = s => s == TestTypeName ? typeof(string) : typeof(int);
            
            // Act
            Type type = (Type)converter.ConvertFrom(TestTypeName);

            // Assert
            Assert.AreEqual(typeof(string), type);
        }

        [TestMethod]
        public void ConvertFrom_Returns_CorrectType_If_Resolver_Is_Default() {
            // Arrange
            BuildManagerTypeNameConverter converter = new BuildManagerTypeNameConverter();
            
            // Act
            Type type = (Type)converter.ConvertFrom(typeof(Version).AssemblyQualifiedName);

            // Assert
            Assert.AreEqual(typeof(Version), type);
        }
    }
}
