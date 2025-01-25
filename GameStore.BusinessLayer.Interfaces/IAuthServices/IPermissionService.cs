// <copyright file="IPermissionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IAuthServices
{
    public interface IPermissionService
    {
        Task<IEnumerable<string>> GetPermissionsByRolesAsync(IEnumerable<string> roles);
    }
}
