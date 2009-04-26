// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ExpressionHelper.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ExpressionHelper type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Text;

namespace Maverick.DomainServices {
    internal class ExpressionHelper {
        public static Expression<Func<T, bool>> BuildPrefixMatchExpression<T>(string prefixParameterName, string path, char separator, bool separatorAtStart) {
            ParameterExpression param = null;
            return BuildPrefixMatchExpression<T>(prefixParameterName, path, separator, separatorAtStart, ref param);
        }

        public static Expression<Func<T, bool>> BuildPrefixMatchExpression<T>(string prefixParameterName, string path, char separator, bool separatorAtStart, ref ParameterExpression lambdaParameter) {
            if (lambdaParameter == null) {
                lambdaParameter = Expression.Parameter(typeof(T), "o");
            }

            string[] segments = path.Split(separator);

            StringBuilder stringBuilder = new StringBuilder();
            Expression queryExpression = null;
            if (separatorAtStart) {
                stringBuilder.Append(separator);
                queryExpression = BuildPrefixMatchBranch(lambdaParameter, prefixParameterName, stringBuilder.ToString());
            }

            for (int i = 0; i < segments.Length; i++) {
                if(String.IsNullOrEmpty(segments[i].Trim())) {
                    continue;
                }

                stringBuilder.Append(segments[i]);

                if (!separatorAtStart) {
                    stringBuilder.Append(separator);
                }

                // Create a query expression based on the url prefix
                Expression branch = BuildPrefixMatchBranch(lambdaParameter, prefixParameterName, stringBuilder.ToString());

                // If there is no query expression
                if (queryExpression == null) {
                    // Make this branch the current query expression
                    queryExpression = branch;
                }
                else {
                    // Append this branch to the current query expression using an OR
                    queryExpression = Expression.OrElse(queryExpression, branch);
                }

                if (separatorAtStart) {
                    stringBuilder.Append(separator);
                }
            }
            return Expression.Lambda<Func<T, bool>>(queryExpression, lambdaParameter);
        }

        private static BinaryExpression BuildPrefixMatchBranch(Expression target, string prefixParameterName, string urlPrefix) {
            return Expression.Equal(Expression.Property(target, prefixParameterName), Expression.Constant(urlPrefix));
        }
    }
}
