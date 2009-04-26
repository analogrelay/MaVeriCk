// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="AssemblyInfo.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   AssemblyInfo.cs
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Maverick Data Access Layer")]
[assembly: AssemblyDescription("Abstract Data Access Layer for Maverick")]
[assembly: AssemblyConfiguration("")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a5a95c35-fb4d-4307-b8df-9769ebda4d99")]

// "Friend" assemblies
[assembly: InternalsVisibleTo("Maverick.Data.Tests")]