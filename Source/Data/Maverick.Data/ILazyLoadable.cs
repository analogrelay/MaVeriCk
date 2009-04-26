// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IEntitySet.cs" company="Andrew Nurse">
//   Copyright (c) 2009 Andrew Nurse.  Licensed under the Ms-PL license: http://opensource.org/licenses/ms-pl.html
// </copyright>
// <summary>
//   Defines the ILazyLoadable type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace Maverick.Data {
    public interface ILazyLoadable
    {
        bool IsLoaded { get; }
        void Load();
    }
}