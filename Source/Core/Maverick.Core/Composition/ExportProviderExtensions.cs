// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ExportProviderExtensions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ExportProviderExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Maverick.ComponentModel;

namespace Maverick.Composition {
    public static class ExportProviderExtensions {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is required as there is no reason for a consumer of this method to have an instance of the type to pass in for type inference")]
        public static T GetComponentOrDefaultByName<T>(this ExportProvider container, string name) {
            Arg.NotNullOrEmpty("name", name);

            return
                container.GetExportedObjectOrDefault<T, ComponentMetadata>(
                    m => String.Equals(m.MetadataView.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "The type parameter is required as there is no reason for a consumer of this method to have an instance of the type to pass in for type inference")]
        public static T GetComponentOrDefaultByName<T>(this ExportProvider container, string contractName, string name) {
            Arg.NotNullOrEmpty("contractName", contractName);
            Arg.NotNullOrEmpty("name", name);

            return
                container.GetExportedObjectOrDefault<T, ComponentMetadata>(contractName,
                                                                           m =>
                                                                           String.Equals(m.MetadataView.Name,
                                                                                         name,
                                                                                         StringComparison.
                                                                                             OrdinalIgnoreCase));
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ObjectOr", Justification = "These are intended to be the separate words 'Object' and 'Or' rather than 'Objector'")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The use of a standard .Net delegate, System.Predicate<T>, is preferable to creating a custom generic delegate for this purpose")]
        public static T GetExportedObjectOrDefault<T, TMetadataView>(this ExportProvider container, Predicate<Export<T, TMetadataView>> criteria) {
            Arg.NotNull("criteria", criteria);

            return GetExportedObjectOrDefault(container.GetExport(criteria));
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ObjectOr", Justification = "These are intended to be the separate words 'Object' and 'Or' rather than 'Objector'")]
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The use of a standard .Net delegate, System.Predicate<T>, is preferable to creating a custom generic delegate for this purpose")]
        public static T GetExportedObjectOrDefault<T, TMetadataView>(this ExportProvider container, string contractName, Predicate<Export<T, TMetadataView>> criteria) {
            Arg.NotNullOrEmpty("contractName", contractName);
            Arg.NotNull("criteria", criteria);

            return GetExportedObjectOrDefault(container.GetExport(contractName, criteria));
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The use of a standard .Net delegate, System.Predicate<T>, is preferable to creating a custom generic delegate for this purpose")]
        public static Export<T, TMetadataView> GetExport<T, TMetadataView>(this ExportProvider container, Predicate<Export<T, TMetadataView>> criteria) {
            Arg.NotNull("criteria", criteria);

            return GetExport(criteria, container.GetExports<T, TMetadataView>().AsQueryable());
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "The use of a standard .Net delegate, System.Predicate<T>, is preferable to creating a custom generic delegate for this purpose")]
        public static Export<T, TMetadataView> GetExport<T, TMetadataView>(this ExportProvider container, string contractName, Predicate<Export<T, TMetadataView>> criteria) {
            Arg.NotNullOrEmpty("contractName", contractName);
            Arg.NotNull("criteria", criteria);

            return GetExport(criteria, container.GetExports<T, TMetadataView>(contractName).AsQueryable());
        }

        private static T GetExportedObjectOrDefault<T>(Export<T> selectedExport) {
            return selectedExport == null ? default(T) : selectedExport.GetExportedObject();
        }

        private static Export<T, TMetadataView> GetExport<T, TMetadataView>(Predicate<Export<T, TMetadataView>> criteria, IQueryable<Export<T, TMetadataView>> startingCollection) {
            return (from export in startingCollection
                    where criteria(export)
                    select export).FirstOrDefault();
        }
    }
}