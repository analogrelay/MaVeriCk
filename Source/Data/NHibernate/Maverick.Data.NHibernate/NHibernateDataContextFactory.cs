// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="NHibernateDataContextFactory.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the NHibernateDataContextFactory type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Maverick.Data.NHibernate.Properties;
using NHibernate;
using NHibernate.Cfg;

namespace Maverick.Data.NHibernate {
    [Export(typeof(DataContextFactory))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [DataContextFactory("NHibernate", "0.1.0.0")]
    public class NHibernateDataContextFactory : DataContextFactory {
        private ISessionFactory _sessionFactory;
        private Func<Configuration> _configurationSource;

        private ISessionFactory SessionFactory {
            get {
                if (_sessionFactory == null) {
                    _sessionFactory = CreateSessionFactory();
                }
                return _sessionFactory;
            }
        }
        
        [ImportMany]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Consumers of this class may wish to completely replace the route collection.  Tests also replace this collection")]
        public IList<MappingContributor> MappingContributors { get; set; }

        [Import(NHibernateContractNames.ConfigurationFilePath, AllowDefault = true)]
        public string ConfigurationFilePath { get; set; }

        [Import(NHibernateContractNames.MappingFileOutputPath, AllowDefault = true)]
        public string MappingFileOutputPath { get; set; }

        [Import(NHibernateContractNames.ConfigurationSource, AllowDefault = true)]
        public Func<Configuration> ConfigurationSource {
            get {
                if(_configurationSource == null && !String.IsNullOrEmpty(ConfigurationFilePath)) {
                    _configurationSource = FileBasedConfigurationSource;
                }
                return _configurationSource;
            }
            set { _configurationSource = value; }
        }

        public NHibernateDataContextFactory() {
            MappingContributors = new List<MappingContributor>();
        }

        public override DataContext CreateDataContext() {
            return new NHibernateDataContext(SessionFactory.OpenSession());
        }

        protected internal virtual Configuration CreateConfiguration() {
            Guard.Against(ConfigurationSource == null,
                          Resources.Error_NoConfigurationSource,
                          NHibernateContractNames.ConfigurationFilePath,
                          NHibernateContractNames.ConfigurationSource);
            Configuration cfg = ConfigurationSource();
            Guard.Against(cfg == null, Resources.Error_ConfigurationSourceReturnedNull);

            // Allow the Mapping Contributors to contribute their mappings
            if (MappingContributors != null) {
                foreach(MappingContributor contributor in MappingContributors) {
                    contributor.ContributeMappings(cfg);
                }
            }
            return cfg;
        }

        protected internal virtual ISessionFactory CreateSessionFactory() {
            // Construct and return the session factory
            return CreateSessionFactory(CreateConfiguration());
        }

        protected internal virtual ISessionFactory CreateSessionFactory(Configuration configuration) {
            Arg.NotNull("cfg", configuration);
            return configuration.BuildSessionFactory();
        }

        private Configuration FileBasedConfigurationSource() {
            return new Configuration().Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                              ConfigurationFilePath));
        }
    }
}
