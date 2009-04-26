// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PageService.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PageService type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Linq;
using Maverick.Models;

namespace Maverick.DomainServices {
    [Export]
    public class PageRepository : RepositoryBase<Page> {
        public Page GetLongestPrefixMatch(string path) {
            var whereExpression = ExpressionHelper.BuildPrefixMatchExpression<Page>("Path",
                                                                                    path,
                                                                                    '/',
                                                                                    true);

            // Run the query
            return GetAll().Include(p => p.Modules)
                           .Where(whereExpression)
                           .ToList()
                           .OrderByDescending(p => p.Path.Length) // TODO: Can we fix NHibernate.Linq so we can push this to the DB?
                           .Take(1)
                           .FirstOrDefault();
        }
    }
}
