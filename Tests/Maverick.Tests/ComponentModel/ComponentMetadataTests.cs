// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ComponentMetadataViewBaseTests.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ComponentMetadataViewBaseTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Maverick.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using TestUtilities;
using TargetResources = Maverick.Properties.Resources;

namespace Maverick.Tests.ComponentModel {
    [TestClass]
    public class ComponentMetadataTests {
        private static readonly Guid TestGuid = new Guid(TestGuidString);
        private const string TestGuidString = "40464E2E-B487-47B7-A329-56BBA828C036";
        private const string TestKey = "Foo";
        private const string TestValue = "Bar";
        private const string TestUrlString = "http://www.microsoft.com";
        private static readonly Uri TestUrl = new Uri(TestUrlString);

        private class TestMetadataView : ComponentMetadata {
            public TestMetadataView(IDictionary<string, object> metadata) : base(metadata) {}
        }

        [TestMethod]
        public void Constructor_Requires_Non_Null_Dictionary() {
            AutoTester.ArgumentNull<IDictionary<string, object>>(marker => new TestMetadataView(marker));
        }

        [TestMethod]
        public void GetMetadataProperty_OfString_Without_DefaultValue_Throws_If_No_Matching_Key_Present() {
            RunRequiredValueMissingTest(TestKey, m => m.GetMetadataProperty<string>(TestKey));
        }

        [TestMethod]
        public void GetMetadataProperty_OfString_Without_DefaultValue_Returns_Value_If_Key_Present() {
            RunSuccessfulPropertyTest(TestKey, m => m.GetMetadataProperty<string>(TestKey), TestValue, TestValue);
        }

        [TestMethod]
        public void GetMetadataProperty_OfString_With_DefaultValue_Returns_DefaultValue_If_Key_Not_Present() {
            RunSuccessfulDefaultValueTest(m => m.GetMetadataProperty(TestKey, TestValue), TestValue);
        }

        [TestMethod]
        public void GetMetadataProperty_OfGuid_Without_Converter_Returns_Converted_Guid() {
            RunSuccessfulPropertyTest(TestKey, m => m.GetMetadataProperty<Guid>(TestKey), TestGuidString, TestGuid);
        }

        [TestMethod]
        public void GetMetadataProperty_OfGuid_With_Converter_Returns_Value_Converted_With_Provided_Converter() {
            RunSuccessfulPropertyTest(TestKey,
                                      m => m.GetMetadataProperty<Guid>(TestKey, CreateTestConverter()),
                                      TestValue,
                                      TestGuid);
        }

        [TestMethod]
        public void GetMetadataProperty_OfGuid_With_DefaultValue_And_Converter_Returns_DefaultValue_If_Not_Present() {
            RunSuccessfulDefaultValueTest(m => m.GetMetadataProperty(TestKey, TestGuid, CreateTestConverter()),
                                          TestGuid);
        }

        [TestMethod]
        public void GetMetadataProperty_Throws_RequiredMetadataMissing_If_Required_Metadata_Value_Null() {
            // Arrange
            TestMetadataView metadataView = new TestMetadataView(new Dictionary<string, object> {
                {TestKey, null}
            });

            // Act and Assert
            ExceptionAssert.Throws<RequiredMetadataMissingException>(
                () => metadataView.GetMetadataProperty<string>(TestKey),
                String.Format(TargetResources.Error_RequiredMetadataKeyMissing, TestKey));
        }

        [TestMethod]
        public void GetMetadataProperty_Returns_DefaultValue_If_Optional_Metadata_Value_Null() {
            // Arrange
            TestMetadataView metadataView = new TestMetadataView(new Dictionary<string, object> {
                {TestKey, null}
            });

            // Act
            string result = metadataView.GetMetadataProperty<string>(TestKey, "Bar");

            // Assert
            Assert.AreEqual("Bar", result);
        }

        [TestMethod]
        public void GetMetadataProperty_Throws_InvalidCastException_If_Converter_Returns_Null() {
            // Arrange
            TestMetadataView metadataView = new TestMetadataView(new Dictionary<string, object> {
                {TestKey, TestValue}
            });

            var mockConverter = new Mock<TypeConverter>();
            mockConverter.Setup(c => c.ConvertFrom(It.IsAny<ITypeDescriptorContext>(), It.IsAny<CultureInfo>(), TestValue))
                         .Returns(() => null);

            // Act
            ExceptionAssert.Throws<InvalidCastException>(
                () => metadataView.GetMetadataProperty<Guid>(TestKey, mockConverter.Object),
                String.Format(TargetResources.Error_CannotConvertMetadataValue, TestValue, typeof(Guid).FullName));
        }

        [TestMethod]
        public void GetMetadataProperty_Throws_InvalidCastException_If_Converter_Throws_NotSupportedException() {
            // Arrange
            TestMetadataView metadataView = new TestMetadataView(new Dictionary<string, object> {
                {TestKey, TestValue}
            });

            var mockConverter = new Mock<TypeConverter>();
            NotSupportedException expectedInnerException = new NotSupportedException();
            mockConverter.Setup(c => c.ConvertFrom(It.IsAny<ITypeDescriptorContext>(), It.IsAny<CultureInfo>(), TestValue))
                         .Throws(expectedInnerException);

            // Act
            ExceptionAssert.Throws<InvalidCastException>(
                () => metadataView.GetMetadataProperty<Guid>(TestKey, mockConverter.Object),
                ex => ReferenceEquals(ex.InnerException, expectedInnerException),
                String.Format(TargetResources.Error_CannotConvertMetadataValue, TestValue, typeof(Guid).FullName));
        }

        [TestMethod]
        public void Name_Returns_Name_Metadata_Value() {
            RunSuccessfulPropertyTest("Name", m => m.Name, TestValue, TestValue);
        }

        [TestMethod]
        public void Name_Is_Required() {
            RunRequiredValueMissingTest("Name", m => m.Name);
        }

        [TestMethod]
        public void Version_Returns_Version_Metadata_Value() {
            RunSuccessfulPropertyTest("Version", m => m.Version, "1.2.3.4", new Version(1, 2, 3, 4));
        }

        [TestMethod]
        public void Version_Returns_1Point0_As_Default() {
            RunSuccessfulDefaultValueTest(m => m.Version, new Version(1, 0, 0, 0));
        }

        [TestMethod]
        public void Vendor_Returns_Vendor_Metadata_Value() {
            RunSuccessfulPropertyTest("Vendor", m => m.Vendor, TestValue, TestValue);
        }

        [TestMethod]
        public void Vendor_Returns_Empty_String_As_Default() {
            RunSuccessfulDefaultValueTest(m => m.Vendor, String.Empty);
        }

        [TestMethod]
        public void Description_Returns_Description_Metadata_Value() {
            RunSuccessfulPropertyTest("Description", m => m.Description, TestValue, TestValue);
        }

        [TestMethod]
        public void Description_Returns_Empty_String_As_Default() {
            RunSuccessfulDefaultValueTest(m => m.Description, String.Empty);
        }

        [TestMethod]
        public void LogoUrl_Returns_LogoUrl_Metadata_Value() {
            RunSuccessfulPropertyTest("LogoUrl", m => m.LogoUrl, TestUrlString, TestUrl);
        }

        [TestMethod]
        public void LogoUrl_Returns_Null_Url_As_Default() {
            RunSuccessfulDefaultValueTest(m => m.LogoUrl, (Uri)null);
        }

        private static void RunSuccessfulDefaultValueTest<T>(Func<ComponentMetadata, object> getter, T expectedValue) {
            TestMetadataView metadataView = new TestMetadataView(new Dictionary<string, object>());

            // Act and Assert
            Assert.AreEqual(expectedValue, getter(metadataView));
        }

        private static void RunRequiredValueMissingTest(string metadataKey, Func<ComponentMetadata, object> getter) {
            // Arrange
            TestMetadataView metadataView = new TestMetadataView(new Dictionary<string, object>());

            // Act and Assert
            ExceptionAssert.Throws<RequiredMetadataMissingException>(
                () => getter(metadataView),
                String.Format(TargetResources.Error_RequiredMetadataKeyMissing, metadataKey));
        }

        private static void RunSuccessfulPropertyTest<T>(string metadataKey, Func<ComponentMetadata, T> getter, string metadataValue, T expectedValue) {
            TestMetadataView metadataView = new TestMetadataView(new Dictionary<string, object> {
                {metadataKey, metadataValue}
            });

            // Act and Assert
            Assert.AreEqual(expectedValue, getter(metadataView));
        }

        private static TypeConverter CreateTestConverter() {
            return CreateTestConverter(s => s.Returns(TestGuid));
        }

        private static TypeConverter CreateTestConverter(Action<ISetup<object>> convertFromBehavior) {
            var mockConverter = new Mock<TypeConverter>();
            convertFromBehavior(
                mockConverter.Setup(c => c.ConvertFrom(It.IsAny<ITypeDescriptorContext>(), 
                                                       It.IsAny<CultureInfo>(), 
                                                       TestValue)));
            return mockConverter.Object;
        }
    }
}
