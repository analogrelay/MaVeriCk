// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ArgTests" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ArgTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Maverick.Tests
{
	[TestClass]
	public class ArgTests
	{
	    [TestMethod]
	    public void NotNull_Does_Nothing_If_ArgumentValue_Is_Not_Null() {
	        Arg.NotNull("foo", new object());
	    }

	    [TestMethod]
	    public void NotNull_Throws_ArgumentNullException_Using_ArgumentName_If_Value_Is_Null() {
	        ExceptionAssert.ThrowsArgNull("foo", () => Arg.NotNull("foo", null));
	    }

        [TestMethod]
        public void NotNullOrEmpty_Does_Nothing_If_ArgumentValue_Is_Not_NullOrEmpty() {
            Arg.NotNullOrEmpty("foo", "foobar");
        }

        [TestMethod]
        public void NotNullOrEmpty_Throws_ArgumentException_Using_ArgumentName_If_Value_Is_Null() {
            ExceptionAssert.ThrowsArgNullOrEmpty("foo", () => Arg.NotNullOrEmpty("foo", null));
        }

        [TestMethod]
        public void NotNullOrEmpty_Throws_ArgumentException_Using_ArgumentName_If_Value_Is_Empty() {
            ExceptionAssert.ThrowsArgNullOrEmpty("foo", () => Arg.NotNullOrEmpty("foo", String.Empty));
        }

        [TestMethod]
        public void InRange_Does_Nothing_If_RangeCheck_Is_True() {
            Arg.InRange("foo", true);
        }

        [TestMethod]
        public void InRange_Throws_ArgumentOutOfRangeException_Using_ArgumentName_If_RangeCheck_Is_False() {
            ExceptionAssert.ThrowsArgOutOfRange("foo", () => Arg.InRange("foo", false));
        }
    }
}
