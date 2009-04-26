// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SingleExportComposablePartDefinitionBase.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the SingleExportComposablePartDefinitionBase type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Maverick.Composition {
    public abstract class SingleExportComposablePartDefinitionBase<TInput, TOutput> : ComposablePartDefinition {
        private readonly string _name;
        private readonly TInput _value;
        private List<ExportDefinition> _exportDefinitions;

        protected SingleExportComposablePartDefinitionBase(string name, TInput value) {
            Arg.NotNullOrEmpty("name", name);
            
            _name = name;
            _value = value;
        }

        public override ComposablePart CreatePart() {
            // No contra/co-varience in Generics means we have to wrap the Func<TOutput> in a Func<object> :(
            return new SingleExportComposablePart(new Export(ExportDefinitions.First(), () => CreateGetter()(_value)));
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions {
            get {
                if (_exportDefinitions == null) {
                    _exportDefinitions = new List<ExportDefinition> { LoadExportDefinition() };
                }
                return _exportDefinitions;
            }
        }

        private ExportDefinition LoadExportDefinition() {
            Dictionary<string, object> metadata = new Dictionary<string, object>();
            metadata["ExportTypeIdentity"] = AttributedModelServices.GetTypeIdentity(typeof(TOutput));
            return new ExportDefinition(_name, metadata);
        }

        public override IEnumerable<ImportDefinition> ImportDefinitions {
            get { return Enumerable.Empty<ImportDefinition>(); }
        }

        protected abstract Func<TInput, TOutput> CreateGetter();
    }
}