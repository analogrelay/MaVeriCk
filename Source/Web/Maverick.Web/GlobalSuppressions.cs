// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="GlobalSuppressions.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   GlobalSuppressions.cs
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Assemblies are not being signed in this initial development period")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Maverick.Web.Modules.ClaimDumper", Justification = "Modules must use their own namespace to avoid collisions when locating Controller classes")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Maverick.Web.Modules.ClaimDumper.Controllers", Justification = "Modules must use their own namespace to avoid collisions when locating Controller classes")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Maverick.Web.Modules.TaskList", Justification = "Modules must use their own namespace to avoid collisions when locating Controller classes")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Maverick.Web.Modules.TaskList.Controllers", Justification = "Modules must use their own namespace to avoid collisions when locating Controller classes")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Maverick.Web.Modules.CurrentTime", Justification = "Modules must use their own namespace to avoid collisions when locating Controller classes")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Maverick.Web.Modules.CurrentTime.Controllers", Justification = "Modules must use their own namespace to avoid collisions when locating Controller classes")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Maverick.Web.Theming", Justification = "The theming engine is in early stages of development and will likely require additional classes in the near future")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Web.Abstractions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", Justification = "Maverick requires .Net 3.5 SP1, but there is no way to specify this target framework in the project properties")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Web.Routing, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", Justification = "Maverick requires .Net 3.5 SP1, but there is no way to specify this target framework in the project properties")]
