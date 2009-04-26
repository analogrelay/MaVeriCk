// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MappingContributor.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MappingContributor type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using NHibernate.Cfg;

namespace Maverick.Data.NHibernate {
    public abstract class MappingContributor {
        public abstract void ContributeMappings(Configuration configuration);
    }
}