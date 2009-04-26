// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="QueryableAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the QueryableAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public static class QueryableAssert {
        public static void Contains<T>(IQueryable<T> queryable, Expression<Func<T, bool>> expr) {
            Assert.AreEqual(1, queryable.Where(expr).Count());
        }
    }
}
