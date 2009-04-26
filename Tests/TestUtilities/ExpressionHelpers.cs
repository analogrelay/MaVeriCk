// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ExpressionHelpers.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ExpressionHelpers type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TestUtilities {
    public class ExpressionHelpers {
        public static MemberInfo GetMemberFromExpression(LambdaExpression expression) {
            switch (expression.Body.NodeType) {
                case ExpressionType.Call:
                    return ((MethodCallExpression)expression.Body).Method;
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression.Body).Member;
                case ExpressionType.New:
                    return ((NewExpression)expression.Body).Constructor;
                default:
                    throw new InvalidOperationException("Unable to determine imported member from expression");
            }
        }
    }
}
