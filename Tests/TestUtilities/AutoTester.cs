// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="AutoTester.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the AutoTester type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public static class AutoTester {
        public static void ArgumentNull<T>(Expression<Action<T>> expr) where T : class {
            // Run the tests
            RunArgumentTests(expr, (paramName, method, act) => ExceptionAssert.ThrowsArgNull(paramName, () => act(null)));
        }

        public static void StringArgumentNullOrEmpty(Expression<Action<string>> expr) {
            // Run the tests
            RunArgumentTests(expr, (paramName, method, act) => {
                ExceptionAssert.ThrowsArgNullOrEmpty(paramName, () => act(null));
                ExceptionAssert.ThrowsArgNullOrEmpty(paramName, () => act(String.Empty));
            });    
        }

        public static void ArgumentOutOfRange<T>(Expression<Action<T>> expr, T testValue) {
            RunArgumentTests(expr, (paramName, method, act) => ExceptionAssert.ThrowsArgOutOfRange(paramName, () => act(testValue)));
        }

        public static void TestReadWriteProperty<T>(Expression<Func<T>> expr, T testValue) {
            RunPropertyTests(expr, (prop, target) => {
                // Set the value to testValue
                prop.SetValue(target, testValue, new object[0]);

                // Get the value and check that it is equal to test value
                Assert.AreEqual(testValue, prop.GetValue(target, new object[0]));
            });
        }

        public static void PropertyTriggersChangedEvent<T>(Expression<Func<T>> expr, T testValue) {
            RunPropertyTests(expr, (prop, target) => {
                INotifyPropertyChanged targetHook = target as INotifyPropertyChanged;
                Assert.IsNotNull(targetHook, "Expected that the property being tested belonged to an object implementing INotifyPropertyChanged");

                // Hook the event
                bool eventFired = false;
                targetHook.PropertyChanged += (sender, eventArgs) => {
                    eventFired = String.Equals(eventArgs.PropertyName, prop.Name, StringComparison.OrdinalIgnoreCase);
                };

                // Change the property value
                prop.SetValue(target, testValue, new object[0]);

                // Verify that the event fired
                Assert.IsTrue(eventFired, "Expected that the PropertyChanged event would be fired!");
            });
        }

        // Supports read-write properties only :(.  Write-Only properties have to be tested manually
        public static void SetNull<T>(Expression<Func<T>> getter) {
            RunPropertyTests(getter,
                             (prop, target) =>
                                ExceptionAssert.ThrowsArgNull("value", () => prop.SetValue(target, null, new object[0])));
        }

        private static void RunPropertyTests<T>(Expression<Func<T>> expr, Action<PropertyInfo, object> act) {
            // Unpack the lambda
            MemberExpression memberExpr = UnpackLambda<MemberExpression>(expr);

            // Get the property
            PropertyInfo prop = memberExpr.Member as PropertyInfo;
            Debug.Assert(prop != null, "TestReadWriteProperty must be provided with a lambda containing a property access expression");

            // Put the left-side of the property access into a lambda and compile it into a Delegate, then run it to the the target object
            object target = Expression.Lambda(memberExpr.Expression).Compile().DynamicInvoke();
            
            act(prop, target);
        }

        private static TExpected UnpackLambda<TExpected>(LambdaExpression expr) where TExpected : Expression {
            TExpected expected = expr.Body as TExpected;
            Debug.Assert(expected != null, String.Format("Expected a {0} within the lambda", typeof(TExpected).Name));
            return expected;
        }

        private static void RunArgumentTests<TArg>(Expression<Action<TArg>> expr, Action<string, MethodBase, Action<TArg>> tests) {
            string expectedParameter;
            MethodBase expectedMethod;
            ExtractExpectedDetails(expr, out expectedParameter, out expectedMethod);

            // Compile the expression
            Action<TArg> act = expr.Compile();

            tests(expectedParameter, expectedMethod, act);
        }

        private static void ExtractExpectedDetails(Expression expr, out string expectedParameter, out MethodBase expectedMethod) {
            // Expression should be a lambda
            LambdaExpression lambda = ConvertExpression<LambdaExpression>(expr, ExpressionType.Lambda);

            // Get the name of the parameter
            Debug.Assert(lambda.Parameters.Count == 1);
            string param = lambda.Parameters[0].Name;

            // Look for that parameter in the expression
            FindExpectedDetailsUsingLambdaParameter(lambda.Body, param, out expectedParameter, out expectedMethod);
        }

        private static void FindExpectedDetailsUsingLambdaParameter(Expression target, string parameter, out string expectedParameter, out MethodBase expectedMethod) {
            expectedMethod = null;
            expectedParameter = null;
            if (target.NodeType == ExpressionType.New) {
                // Check the arguments
                NewExpression ctor = (NewExpression)target;
                expectedParameter = MatchArgumentsAgainstParameter(parameter, ctor.Arguments, ctor.Constructor.GetParameters());
                expectedMethod = ctor.Constructor;
            }
            else if (target.NodeType == ExpressionType.Call) {
                MethodCallExpression call = target as MethodCallExpression;
                if (call != null) {
                    expectedParameter = MatchArgumentsAgainstParameter(parameter, call.Arguments, call.Method.GetParameters());
                    expectedMethod = call.Method;
                }
            }
            else {
                throw new InvalidOperationException(
                    "Expected that the top-most expression in the lambda would be a constructor or method call");
            }
        }

        private static string MatchArgumentsAgainstParameter(string parameter, IList<Expression> args, ParameterInfo[] parameters) {
            for (int i = 0; i < args.Count; i++) {
                Expression expr = args[i];
                if (expr.NodeType == ExpressionType.Parameter) {
                    if (((ParameterExpression)expr).Name == parameter) {
                        return parameters[i].Name;
                    }
                }
            }
            throw new InvalidOperationException("Expected that the parameter would be used in the top-most constructor or method call expression");
        }

        private static TExpr ConvertExpression<TExpr>(Expression incoming, ExpressionType type) where TExpr : Expression {
            return ConvertExpression<TExpr>(incoming, type, String.Format("Expected an expression of type: {0}", type));
        }

        private static TExpr ConvertExpression<TExpr>(Expression incoming, ExpressionType type, string exceptionMessage) where TExpr : Expression {
            if (incoming.NodeType != type) {
                throw new InvalidOperationException(exceptionMessage);
            }
            TExpr outgoing = incoming as TExpr;
            if (outgoing == null) {
                throw new InvalidCastException(String.Format("Could not cast expression of type {0} to {1}", incoming.NodeType, typeof(TExpr).FullName));
            }
            return outgoing;
        }
    }
}
