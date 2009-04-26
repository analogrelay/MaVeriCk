// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CompositionAssert.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the CompositionAssert type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtilities {
    public static class CompositionAssert {
        public static void IsImported<T>(Expression<Func<T, object>> memberExpression) {
            IsImported(ExpressionHelpers.GetMemberFromExpression(memberExpression));
        }

        public static void IsImported<T>(Expression<Func<T, object>> memberExpression, string contractName, Type contractType) {
            IsImported(ExpressionHelpers.GetMemberFromExpression(memberExpression), contractName, contractType);
        }

        public static void IsImported<T>(Expression<Func<T, object>> memberExpression, string contractName) {
            IsImported(ExpressionHelpers.GetMemberFromExpression(memberExpression), contractName);
        }

        public static void IsImported<T>(Expression<Func<T, object>> memberExpression, Type contractType) {
            IsImported(ExpressionHelpers.GetMemberFromExpression(memberExpression), contractType);
        }

        public static void IsImported(MemberInfo member) {
            AttributeAssert.IsDefined<ImportAttribute>(member,
                                                       attr =>
                                                       String.IsNullOrEmpty(attr.ContractName) &&
                                                       attr.ContractType == null);
        }

        public static void IsImported(MemberInfo member, string contractName, Type contractType) {
            AttributeAssert.IsDefined<ImportAttribute>(member, attr => attr.ContractName == contractName &&
                                                                       attr.ContractType == contractType);
        }

        public static void IsImported(MemberInfo member, string contractName) {
            IsImported(member, contractName, null);
        }

        public static void IsImported(MemberInfo member, Type contractType) {
            AttributeAssert.IsDefined<ImportAttribute>(member,
                                                       attr =>
                                                       attr.ContractType == contractType);
        }

        public static void IsImportingConstructor(Expression<Func<object>> constructorExpression) {
            IsImportingConstructor(ExpressionHelpers.GetMemberFromExpression(constructorExpression));
        }

        public static void IsImportingConstructor(MemberInfo member) {
            Assert.IsTrue(member.MemberType == MemberTypes.Constructor);
            AttributeAssert.IsDefined<ImportingConstructorAttribute>(member);
        }

        public static void IsImportedCollection<T>(Expression<Func<T, object>> memberExpression) {
            IsImportedCollection(ExpressionHelpers.GetMemberFromExpression(memberExpression));
        }

        public static void IsImportedCollection<T>(Expression<Func<T, object>> memberExpression, string contractName, Type contractType) {
            IsImportedCollection(ExpressionHelpers.GetMemberFromExpression(memberExpression), contractName, contractType);
        }

        public static void IsImportedCollection<T>(Expression<Func<T, object>> memberExpression, string contractName) {
            IsImportedCollection(ExpressionHelpers.GetMemberFromExpression(memberExpression), contractName, null);
        }

        public static void IsImportedCollection<T>(Expression<Func<T, object>> memberExpression, Type contractType) {
            IsImportedCollection(ExpressionHelpers.GetMemberFromExpression(memberExpression), contractType);
        }

        public static void IsImportedCollection(MemberInfo member) {
            AttributeAssert.IsDefined<ImportManyAttribute>(member,
                                                           attr =>
                                                           String.IsNullOrEmpty(attr.ContractName) &&
                                                           attr.ContractType == null);
        }

        public static void IsImportedCollection(MemberInfo member, string contractName, Type contractType) {
            AttributeAssert.IsDefined<ImportManyAttribute>(member,
                                                           attr =>
                                                           attr.ContractName == contractName &&
                                                           attr.ContractType == contractType);
        }

        public static void IsImportedCollection(MemberInfo member, string contractName) {
            IsImportedCollection(member, contractName, null);
        }

        public static void IsImportedCollection(MemberInfo member, Type contractType) {
            AttributeAssert.IsDefined<ImportManyAttribute>(member,
                                                       attr =>
                                                       attr.ContractType == contractType);
        }

        public static void IsExported<T>(Expression<Func<T, object>> memberExpression, string contractName, Type contractType) {
            IsExported(ExpressionHelpers.GetMemberFromExpression(memberExpression), contractName, contractType);
        }

        public static void IsExported<T>(Expression<Func<T, object>> memberExpression, Type contractType) {
            IsExported(ExpressionHelpers.GetMemberFromExpression(memberExpression), contractType);
        }

        public static void IsExported(MemberInfo member) {
            AttributeAssert.IsDefined<ExportAttribute>(member, attr => String.IsNullOrEmpty(attr.ContractName) && attr.ContractType == null);
        }

        public static void IsExported(MemberInfo member, string contractName, Type contractType) {
            AttributeAssert.IsDefined<ExportAttribute>(member, attr => attr.ContractName == contractName && attr.ContractType == contractType);
        }

        public static void IsExported(MemberInfo member, Type contractType) {
            AttributeAssert.IsDefined<ExportAttribute>(member,
                                                       attr =>
                                                       attr.ContractType == contractType);
        }

        public static void IsContractType(Type type) {
            AttributeAssert.IsDefined<ContractTypeAttribute>(type);
        }

        public static void IsContractType(Type type, Type metadataViewType) {
            AttributeAssert.IsDefined<ContractTypeAttribute>(type, attr => attr.MetadataViewType == metadataViewType);
        }

        public static void HasCreationPolicy<T>(Expression<Func<T, object>> memberExpression, CreationPolicy creationPolicy) {
            HasCreationPolicy(ExpressionHelpers.GetMemberFromExpression(memberExpression), creationPolicy);
        }

        public static void HasCreationPolicy(MemberInfo member, CreationPolicy creationPolicy) {
            AttributeAssert.IsDefined<PartCreationPolicyAttribute>(member, attr => attr.CreationPolicy == creationPolicy);
        }

        public static void IsNotDiscoverable<T>(Expression<Func<T, object>> memberExpression) {
            IsNotDiscoverable(ExpressionHelpers.GetMemberFromExpression(memberExpression));
        }

        public static void IsNotDiscoverable(MemberInfo member) {
            AttributeAssert.IsDefined<PartNotDiscoverableAttribute>(member);
        }

        public static void ExportsAreInherited<T>(Expression<Func<T, object>> memberExpression) {
            ExportsAreInherited(ExpressionHelpers.GetMemberFromExpression(memberExpression));
        }

        public static void ExportsAreInherited(MemberInfo member) {
            AttributeAssert.IsDefined<PartExportsInheritedAttribute>(member);
        }
    }
}