// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="AttributeAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the AttributeAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public class AttributeAssert {
        public static TAttribute IsDefined<TAttribute>(MemberInfo type) where TAttribute : Attribute {
            return IsDefined<TAttribute>(type, a => true);
        }

        public static TAttribute IsDefined<TAttribute>(MemberInfo type, Predicate<TAttribute> criteria) where TAttribute : Attribute {
            TAttribute attr = type.GetCustomAttributes(typeof(TAttribute), true)
                                  .OfType<TAttribute>()
                                  .FirstOrDefault();

            Assert.IsNotNull(attr, "Expected attribute of type {0} was not defined", typeof(TAttribute).FullName);
            Assert.IsTrue(criteria(attr), "Expected attribute of type {0} was defined, but did not match criteria", typeof(TAttribute).FullName);
            return attr;
        }
    }
}
