// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MockDataRepositoryFactory.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the MockDataRepositoryFactory type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;

namespace Maverick.Data.Tests {
    [Export(typeof(DataContextFactory))]
    [DataContextFactory("Mock", "0.1.0.0")]
    public class MockDataRepositoryFactory : DataContextFactory {
        public override DataContext CreateDataContext() {
            throw new NotImplementedException();
        }
    }
}
