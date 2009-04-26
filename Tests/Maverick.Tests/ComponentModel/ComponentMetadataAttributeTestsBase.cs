using System;

using Maverick.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maverick.Tests.ComponentModel {
    public abstract class ComponentMetadataAttributeTestsBase<TAttribute> where TAttribute : ComponentMetadataAttribute {
        protected const string TestName = "Zork";
        protected const string TestDescription = "Foo Bar Baz";
        protected const string TestVendor = "Zoop Inc.";
        protected const string TestLogoUrl = "res://./MyImage.png";
        protected const string TestVersion = "1.0.0.0";

        protected internal abstract Func<TAttribute>[] GetConstructors();

        public virtual void Constructor_Sets_Name_Property() {
            TestConstructorsSetProperty(0, TestName, a => a.Name);
        }

        public virtual void Constructor_Sets_Version_Property() {
            TestConstructorsSetProperty(1, TestVersion, a => a.Version);
        }

        public virtual void Constructor_Sets_Description_Property() {
            TestConstructorsSetProperty(2, TestDescription, a => a.Description);
        }

        public virtual void Constructor_Sets_Vendor_Property() {
            TestConstructorsSetProperty(3, TestVendor, a => a.Vendor);
        }

        public virtual void Constructor_Sets_LogoUrl_Property() {
            TestConstructorsSetProperty(4, TestLogoUrl, a => a.LogoUrl);
        }

        private void TestConstructorsSetProperty<T>(int startConstructor, T expected, Func<TAttribute, T> getter) {
            Func<TAttribute>[] constructors = GetConstructors();
            for (int i = 0; i < startConstructor; i++ ) {
                Assert.AreEqual(default(T), getter(constructors[i]()));   
            }
            for (int i = startConstructor; i < constructors.Length; i++) {
                Assert.AreEqual(expected, getter(constructors[i]()));
            }
        }
    }
}