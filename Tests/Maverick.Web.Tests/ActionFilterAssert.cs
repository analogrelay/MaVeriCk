// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ActionFilterAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ActionFilterAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using TestUtilities;

namespace Maverick.Web.Tests {
    public static class ActionFilterAssert {
        public static void ValidatesInput<T>(Expression<Func<T, object>> actionExpression) {
            AttributeAssert.IsDefined<ValidateInputAttribute>(ExpressionHelpers.GetMemberFromExpression(actionExpression),
                                                              attr => attr.EnableValidation);
        }

        public static void DoesNotValidateInput<T>(Expression<Func<T, object>> actionExpression) {
            AttributeAssert.IsDefined<ValidateInputAttribute>(ExpressionHelpers.GetMemberFromExpression(actionExpression),
                                                              attr => !attr.EnableValidation);
        }

        public static void HasActionName<T>(Expression<Func<T, object>> actionExpression, string actionName) {
            AttributeAssert.IsDefined<ActionNameAttribute>(ExpressionHelpers.GetMemberFromExpression(actionExpression),
                                                           attr =>
                                                           String.Equals(attr.Name, actionName, StringComparison.OrdinalIgnoreCase));
        }

        public static void AcceptsVerb<T>(Expression<Func<T, object>> actionExpression, HttpVerbs verb) {
            AcceptsVerb(actionExpression, new[] { verb.ToString() });
        }

        public static void RequiresRole<T>(Expression<Func<T, object>> actionExpression, string roles) {
            AttributeAssert.IsDefined<AuthorizeAttribute>(ExpressionHelpers.GetMemberFromExpression(actionExpression),
                                                          attr =>
                                                          String.Equals(attr.Roles,
                                                                        roles,
                                                                        StringComparison.OrdinalIgnoreCase));
        }
        
        private static void AcceptsVerb<T>(Expression<Func<T, object>> actionExpression, IEnumerable<string> verbs) {
            AttributeAssert.IsDefined<AcceptVerbsAttribute>(ExpressionHelpers.GetMemberFromExpression(actionExpression),
                                                            attr => (from actualVerb in attr.Verbs
                                                                     where (from expectedVerb in verbs
                                                                            where
                                                                                String.Equals(expectedVerb,
                                                                                              actualVerb,
                                                                                              StringComparison.
                                                                                                  OrdinalIgnoreCase)
                                                                            select true).FirstOrDefault()
                                                                     select actualVerb).Count() == attr.Verbs.Count);
        }
    }

    public class ActionFilterAssert<TController> where TController : Controller {
        public void ValidatesInput(Expression<Func<TController, object>> actionExpression) {
            ActionFilterAssert.ValidatesInput(actionExpression);
        }

        public void DoesNotValidateInput(Expression<Func<TController, object>> actionExpression) {
            ActionFilterAssert.DoesNotValidateInput(actionExpression);
        }

        public void HasActionName(Expression<Func<TController, object>> actionExpression, string actionName) {
            ActionFilterAssert.HasActionName(actionExpression, actionName);
        }

        public void AcceptsVerb(Expression<Func<TController, object>> actionExpression, HttpVerbs verb) {
            ActionFilterAssert.AcceptsVerb(actionExpression, verb);
        }

        public void RequiresRole(Expression<Func<TController, object>> actionExpression, string roles) {
            ActionFilterAssert.RequiresRole(actionExpression, roles);
        }
    }
}
