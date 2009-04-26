// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ComponentMetadata.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ComponentMetadata type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Maverick.Properties;

namespace Maverick.ComponentModel {
    public class ComponentMetadata {
        private const string NameProperty = "Name";
        private const string VendorProperty = "Vendor";
        private const string LogoUrlProperty = "LogoUrl";
        private const string VersionProperty = "Version";
        private const string DescriptionProperty = "Description";
        private readonly IDictionary<string, object> _metadata;

        public ComponentMetadata(IDictionary<string, object> metadata) {
            Arg.NotNull("metadata", metadata);
            _metadata = metadata;
        }

        public string Name {
            get { return GetMetadataProperty<string>(NameProperty); }
        }

        public string Vendor {
            get { return GetMetadataProperty(VendorProperty, String.Empty); }
        }

        public Uri LogoUrl {
            get { return GetMetadataProperty(LogoUrlProperty, (Uri)null); }
        }

        public Version Version {
            get { return GetMetadataProperty(VersionProperty, new Version(1, 0, 0, 0), new VersionConverter()); }
        }

        public string Description {
            get { return GetMetadataProperty(DescriptionProperty, String.Empty); }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is always necessary for required metadata properties since there is no way to infer it")]
        protected internal virtual T GetMetadataProperty<T>(string propertyName) {
            return InternalGetMetadataProperty(propertyName, default(T), true, null);
        }

        protected internal virtual T GetMetadataProperty<T>(string propertyName, T defaultValue) {
            return InternalGetMetadataProperty(propertyName, defaultValue, false, null);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is always necessary for required metadata properties since there is no way to infer it")]
        protected internal virtual T GetMetadataProperty<T>(string propertyName, TypeConverter converter) {
            return InternalGetMetadataProperty(propertyName, default(T), true, converter);
        }

        protected internal virtual T GetMetadataProperty<T>(string propertyName, T defaultValue, TypeConverter converter) {
            return InternalGetMetadataProperty(propertyName, defaultValue, false, converter);
        }

        protected internal virtual string GetMetadataKeyForProperty(string propertyName) {
            return propertyName;
        }

        private static string GetCannotConvertMessage<T>(object obj) {
            return String.Format(CultureInfo.CurrentUICulture,
                                 Resources.Error_CannotConvertMetadataValue,
                                 obj,
                                 typeof(T).FullName);
        }

        private static string GetRequiredMetadataMessage(string propertyName) {
            return String.Format(CultureInfo.CurrentUICulture,
                                 Resources.Error_RequiredMetadataKeyMissing,
                                 propertyName);
        }

        private T InternalGetMetadataProperty<T>(string propertyName, T defaultValue, bool required, TypeConverter converter) {
            string key = GetMetadataKeyForProperty(propertyName);
            if(!_metadata.ContainsKey(key)) {
                if(required) {
                    throw new RequiredMetadataMissingException(GetRequiredMetadataMessage(propertyName));
                }
                return defaultValue;
            }

            // Get the value
            object obj = _metadata[key];
            if(obj == null) {
                if(required) {
                    throw new RequiredMetadataMissingException(GetRequiredMetadataMessage(propertyName));
                }
                return defaultValue;
            }

            // Do we need to convert it?
            T value;
            if(obj is T) {
                value = (T)obj;
            } 
            else {
                // Use the default type converter if no specific one is provided
                if(converter == null) {
                    converter = TypeDescriptor.GetConverter(typeof(T));
                }
                object converted;
                try {
                    converted = converter.ConvertFrom(obj);
                } catch(Exception ex) {
                    throw new InvalidCastException(GetCannotConvertMessage<T>(obj), ex);
                }
                if (converted == null) {
                    throw new InvalidCastException(GetCannotConvertMessage<T>(obj));
                }
                value = (T)converted;
            }

            // TODO: We can do localization on strings here.
            return value;
        }
    }
}
