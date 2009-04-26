// opyright company="Andrew Nurse" file="GlobalSuppressions.cs">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// // This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Assemblies are not being signed in this initial development period")]

[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "Cfg", Scope = "resource", Target = "Maverick.Data.NHibernate.Properties.Resources.resources", Justification = "These terms are used by external assemblies whose namespaces are referenced by the resource.  Therefore, the spelling cannot be corrected")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "Configurer", Scope = "resource", Target = "Maverick.Data.NHibernate.Properties.Resources.resources", Justification = "These terms are used by external assemblies whose namespaces are referenced by the resource.  Therefore, the spelling cannot be corrected")]
