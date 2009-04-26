// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DictionaryAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DictionaryAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maverick.Web.Tests {
    public static class DictionaryAssert {
        public static void ContainsEntries(object expected, IDictionary<string, object> actual) {
            ContainsEntries(new RouteValueDictionary(expected), actual);
        }

        public static void ContainsEntries(IDictionary<string, object> expected, IDictionary<string, object> actual) {
            foreach (KeyValuePair<string, object> pair in expected) {
                Assert.IsTrue(actual.ContainsKey(pair.Key), "Expected that the dictionary would contain the '{0}' key", pair.Key);
                Assert.AreEqual(pair.Value, actual[pair.Key], "Expected that the value of the dictionary entry '{0}' would be '{1}'", pair.Key, pair.Value);
            }
        }
    }
}
