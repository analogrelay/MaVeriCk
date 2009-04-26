// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="DelegateDisposable" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the DelegateDisposable type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestUtilities {
    public class DelegateDisposable : IDisposable {
        public Action OnDispose { get; protected set; }

        public DelegateDisposable(Action onDispose) {
            OnDispose = onDispose;
        }

        public void Dispose() {
            OnDispose();
        }
    }
}
