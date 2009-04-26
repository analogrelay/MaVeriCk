using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Web.Tests.Configuration {
    public abstract class ConfigurationElementTestsBase<TElement> where TElement : ConfigurationElement, IConfigurationElementAccessor {
        protected abstract TElement CreateAccessor();

        protected void RunPropertyGetterTest<T>(string propertyName, T testValue, Func<TElement, T> getter) {
            // Arrange
            TElement element = CreateAccessor();
            
            // Act
            element.SetProperty(propertyName, testValue);

            // Assert
            Assert.AreSame(testValue, getter(element));
        }

        // Helper to simplify lambdas if the property is not write-only
        protected void RunPropertySetterTest<T>(string propertyName, T testValue, Expression<Func<TElement, T>> propertyGetterExpression) {
            PropertyInfo property = (PropertyInfo)ExpressionHelpers.GetMemberFromExpression(propertyGetterExpression);
            RunPropertySetterTest(propertyName, testValue, (e, v) => property.SetValue(e, v, new object[0]));
        }

        protected void RunPropertySetterTest<T>(string propertyName, T testValue, Action<TElement, T> setter) {
            // Arrange
            TElement element = CreateAccessor();
            
            // Act
            setter(element, testValue);

            // Assert
            Assert.AreSame(testValue, element.GetProperty(propertyName));
        }
    }
}