// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="BuildManagerTypeNameConverter.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the BuildManagerTypeNameConverter type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Web.Compilation;
using Maverick.Web.Configuration.Properties;

namespace Maverick.Web.Configuration {
    public class BuildManagerTypeNameConverter : ConfigurationConverterBase {
        private Func<string, Type> _typeResolver;

        public Func<string, Type> TypeResolver {
            get {
                if(_typeResolver == null) {
                    _typeResolver = name => BuildManager.GetType(name, false);
                }
                return _typeResolver;
            }
            set { _typeResolver = value; }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            string stringValue = value as string;
            if (stringValue != null) {
                Type type = TypeResolver(stringValue);

                if (type == null) {
                    throw new ArgumentException(String.Format(CultureInfo.CurrentUICulture,
                                                              Resources.Error_CouldNotResolveType,
                                                              value),
                                                "value");
                }
                return type;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            Type type = value as Type;
            if(type != null && destinationType == typeof(string)) {
                return type.AssemblyQualifiedName;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
