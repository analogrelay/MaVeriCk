// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ExceptionAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ExceptionAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public static class ExceptionAssert {
        private const string MessageArgumentNullOrEmpty = "Argument '{0}' cannot be null or an empty string{1}Parameter name: {0}";

        public static void Throws<TException>(Action act) where TException : Exception {
            Throws<TException>(act, ex => true);
        }

        public static void Throws<TException>(Action act, string message, params object[] args) where TException : Exception {
            Throws<TException>(act, String.Format(message, args));
        }

        public static void Throws<TException>(Action act, Expression<Func<TException, bool>> checker, string message, params object[] args) where TException : Exception {
            Throws(act, checker, String.Format(message, args));
        }

        public static void Throws<TException>(Action act, string message) where TException : Exception {
            Throws<TException>(act, ex => true, message);
        }

        // NOTE: Using Expressions to get string conversions in the AssertFailedException
        public static void Throws<TException>(Action act, Expression<Func<TException, bool>> checker, string message) where TException : Exception {
            ParameterExpression param = checker.Parameters[0];
            Expression messageBranch = StringEqualExpression(Expression.Property(param, "Message"),
                                                             message,
                                                             StringComparison.OrdinalIgnoreCase);

            Throws(act,
                   Expression.Lambda<Func<TException, bool>>(Expression.AndAlso(messageBranch, 
                                                                                checker.Body), 
                                                             param));
        }

        public static void Throws<TException>(Action act, Expression<Func<TException, bool>> checker) where TException : Exception {
            bool matched = false;
            bool thrown = false;
            try {
                act();
            }
            catch (Exception ex) {
                TException tex = ex as TException;
                if (tex == null) {
                    if (typeof(TException) == typeof(TargetInvocationException)) {
                        // The only place we do special processing is TargetInvocationException, but if that's
                        // what the user expected, we don't do anything
                        throw;
                    }

                    TargetInvocationException tiex = ex as TargetInvocationException;
                    if (tiex == null) {
                        throw;
                    }
                    // Unwrap as many levels of TargetInvocationException as necessary
                    tex = UnwrapTargetInvocationException(tiex) as TException;
                    if (tex == null) {
                        throw;
                    }
                }
                thrown = true;
                matched = checker.Compile()(tex);
                if (!matched) {
                    throw new AssertFailedException(String.Format("Expected exception was thrown, but did not match the condition: {0}.  Actual Exception: {1}", checker, ex));
                }
            }

            if (!thrown) {
                throw new AssertFailedException(String.Format("Expected exception of type '{0}' was not thrown", typeof(TException).FullName));
            }
        }

        private static Exception UnwrapTargetInvocationException(TargetInvocationException tiex) {
            Exception ex = null;
            while (tiex != null) {
                ex = tiex.InnerException;
                tiex = ex as TargetInvocationException;
            }
            return ex;
        }

        public static void Guards(Action act, string message) {
            Throws<InvalidOperationException>(act, message);
        }

        public static void Guards(Action act, string message, params object[] args) {
            Throws<InvalidOperationException>(act, message, args);
        }

        public static void PropertyThrowsArgOutOfRange(Action act) {
            ThrowsArgOutOfRange("value", act);
        }

        public static void ThrowsArgNull(string paramName, Action act) {
            ThrowsArgNull(paramName, null, act);
        }

        public static void ThrowsArgNullOrEmpty(string paramName, Action act) {
            ThrowsArgNullOrEmpty(paramName, null, act);
        }

        public static void ThrowsArgNull(string paramName, MethodBase expectedMethod, Action act) {
            Throws(act, CheckArgumentException<ArgumentNullException>(paramName, expectedMethod));
        }

        public static void ThrowsArgNullOrEmpty(string paramName, MethodBase expectedMethod, Action act) {

            Throws(act,
                   CheckArgumentException<ArgumentException>(paramName, expectedMethod),
                   MessageArgumentNullOrEmpty,
                   paramName,
                   Environment.NewLine);
        }

        public static void ThrowsObjectDisposed(string objectName, Action act) {
            Throws<ObjectDisposedException>(act, ex => String.Equals(ex.ObjectName, objectName, StringComparison.Ordinal), String.Format("Cannot access a disposed object.{1}Object name: '{0}'.", objectName, Environment.NewLine));
        }

        public static void ThrowsArgOutOfRange(string paramName, Action act) {
            Throws<ArgumentOutOfRangeException>(act, ex => String.Equals(ex.ParamName, paramName, StringComparison.Ordinal), String.Format("Specified argument was out of the range of valid values.{1}Parameter name: {0}", paramName, Environment.NewLine));
        }

        private static Expression<Func<T, bool>> CheckArgumentException<T>(string paramName, MethodBase expectedMethod) where T : ArgumentException {
            ParameterExpression ex = Expression.Parameter(typeof(T), "ex");

            Expression body = StringEqualExpression(Expression.Property(ex, "ParamName"),
                                                    paramName,
                                                    StringComparison.Ordinal);

            if(expectedMethod != null) {
                Expression targetSite = Expression.Property(ex, "TargetSite");
                Expression declaringType = Expression.Property(targetSite, "DeclaringType");
                body = Expression.AndAlso(body,
                                          Expression.AndAlso(
                                              StringEqualExpression(Expression.Property(targetSite, "Name"),
                                                                    expectedMethod.Name,
                                                                    StringComparison.Ordinal),
                                              StringEqualExpression(
                                                  Expression.Property(declaringType, "AssemblyQualifiedName"),
                                                  expectedMethod.DeclaringType.AssemblyQualifiedName,
                                                  StringComparison.Ordinal)
                                              )
                    );
                    
            }

            return Expression.Lambda<Func<T, bool>>(body, ex);
       }

        private static Expression StringEqualExpression(Expression left, string right, StringComparison stringComparison) {
            return StringEqualExpression(left, Expression.Constant(right), Expression.Constant(stringComparison));
        }

        private static Expression StringEqualExpression(Expression left, Expression right, Expression stringComparison) {
            return Expression.Call(
                typeof(string),
                "Equals",
                new Type[0],
                left,
                right,
                stringComparison);
        }


    }
}
