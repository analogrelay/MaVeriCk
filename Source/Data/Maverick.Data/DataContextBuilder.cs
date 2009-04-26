// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DataContextBuilder.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DataContextBuilder type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Maverick.ComponentModel;
using Maverick.Data.Properties;

namespace Maverick.Data {
    [Export]
    public class DataContextBuilder {
        private ComponentCollection<DataContextFactory> _contextFactories = new ComponentCollection<DataContextFactory>();
        private string _defaultFactoryName = String.Empty;
        
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "For testing purposes and to ensure MEF is able to set the collection, this property must be writable")]
        [ImportMany(typeof(DataContextFactory))]
        public ComponentCollection<DataContextFactory> ContextFactories {
            get { return _contextFactories; }
            set {
                Arg.NotNull("value", value);
                _contextFactories = value;
            }
        }

        [Import(DataContractNames.DataContextFactoryName)]
        public string DefaultFactoryName {
            get { return _defaultFactoryName; }
            set {
                Arg.NotNull("value", value);
                _defaultFactoryName = value;
            }
        }

        private DataContextFactory DefaultFactory {
            get {
                Export<DataContextFactory, ComponentMetadata> exportedFactory = ContextFactories[DefaultFactoryName];
                Guard.Against(exportedFactory == null, Resources.Error_DataContextFactoryNotImported, DefaultFactoryName);

                DataContextFactory factory = exportedFactory.GetExportedObject();
                Guard.Against(factory == null, Resources.Error_DataContextFactoryWasNull, DefaultFactoryName);
                return factory;
            }
        }

        public virtual DataContext CreateDataContext() {
            Guard.Against(!ContextFactories.Contains(DefaultFactoryName), 
                          Resources.Error_DataContextFactoryNotFound, 
                          DefaultFactoryName,
                          DataContractNames.DataContextFactoryName);

            DataContext dataContext = DefaultFactory.CreateDataContext();
            Guard.Against(dataContext == null, Resources.Error_DataContextFactoryReturnedNull, DefaultFactoryName);
            return dataContext;
        }
    }
}
