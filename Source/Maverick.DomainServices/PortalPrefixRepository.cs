// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="PortalPrefixService.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the PortalPrefixService type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Maverick.Models;

namespace Maverick.DomainServices {
    [Export]
    public class PortalPrefixRepository : RepositoryBase<PortalPrefix> {
        public virtual PortalPrefix GetLongestPrefixMatch(Uri url) {
            StringBuilder urlBuilder = new StringBuilder();

            // www.domain.com:port
            urlBuilder.Append(url.Host);
            if(url.Port != 80) {
                urlBuilder.Append(":");
                urlBuilder.Append(url.Port);
            }
            urlBuilder.Append(url.AbsolutePath);

            ParameterExpression param = null;
            var whereExpression = ExpressionHelper.BuildPrefixMatchExpression<PortalPrefix>("Prefix",
                                                                                            urlBuilder.ToString(), 
                                                                                            '/', 
                                                                                            false, ref param);

            // Also try without the port if the port is not the default
            // TODO: Build the expression in one pass...
            if (url.Port != 80) {
                // www.domain.com
                urlBuilder = new StringBuilder();
                urlBuilder.Append(url.Host);
                urlBuilder.Append(url.AbsolutePath);

                whereExpression = Expression.Lambda<Func<PortalPrefix, bool>>(
                    Expression.Or(whereExpression.Body,
                                  ExpressionHelper.BuildPrefixMatchExpression<PortalPrefix>("Prefix",
                                                                                                urlBuilder.ToString(),
                                                                                                '/',
                                                                                                false, ref param).Body), param);
            }

            // Run the query
            return GetAll().Include(p => p.Portal)
                           .Where(whereExpression)
                           .ToList() // TODO: Can we fix NHibernate.Linq so we can push this to the DB?
                           .OrderByDescending(p => p.Prefix.Length)
                           .Take(1)
                           .FirstOrDefault();
        }
    }
}
