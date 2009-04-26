// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SingleExportComposablePart.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the SingleExportComposablePart type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Maverick.Properties;

namespace Maverick.Composition {
    public class SingleExportComposablePart : ComposablePart {
        private readonly Export _export;
        private readonly List<ExportDefinition> _exportDefinitions;

        public SingleExportComposablePart(Export export) {
            Arg.NotNull("export", export);
            _export = export;
            _exportDefinitions = new List<ExportDefinition> { export.Definition };
        }

        public override object GetExportedObject(ExportDefinition definition) {
            Guard.Against(!definition.Equals(_export.Definition),
                          Resources.Error_PartDoesNotContainAnExportForContract,
                          definition.ContractName);

            return _export.GetExportedObject();
        }

        public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports) {
            throw new InvalidOperationException(Resources.Error_PartDoesNotContainAnyImports);
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions {
            get { return _exportDefinitions; }
        }
        public override IEnumerable<ImportDefinition> ImportDefinitions {
            get { return Enumerable.Empty<ImportDefinition>(); }
        }
    }
}