// <copyright file="IRoleService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IAuthServices
{
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.ResponseDTO;
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.WrappedDTOs;
    using GameStore.BusinessLayer.Interfaces.DTO;

    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();

        Task<RoleDto> GetRoleByIdAsync(Guid id);

        Task DeleteRoleByIdAsync(Guid id);

        Task<IEnumerable<string>> GetPermissionsAsync();

        Task<IEnumerable<string>> GetRolePermissionsAsync(Guid roleId);

        Task<RoleResponseDto> CreateRoleAsync(CreateRoleRequestDto request);

        Task<RoleResponseDto> UpdateRoleAsync(RoleDtoWrapper role);
    }
}
