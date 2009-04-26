// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="FluentMappingContributor.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the FluentMappingContributor type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;

namespace Maverick.Data.NHibernate {
    [CLSCompliant(false)]
    public abstract class FluentMappingContributor : MappingContributor {
        public override void ContributeMappings(Configuration configuration) {
            Arg.NotNull("configuration", configuration);

            MappingConfiguration mappingConfiguration = new MappingConfiguration();
            ContributeMappings(mappingConfiguration);
            mappingConfiguration.Apply(configuration);
        }

        public abstract void ContributeMappings(MappingConfiguration configuration);
    }
}