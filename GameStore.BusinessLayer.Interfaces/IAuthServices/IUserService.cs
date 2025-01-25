// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IAuthServices
{
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;
    using GameStore.BusinessLayer.Interfaces.DTO;

    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();

        Task<UserDto> GetUserByIdAsync(Guid id);

        Task DeleteUserByIdAsync(Guid id);

        Task<bool> AddUserAsync(UserCreateRequestDto request);

        Task<bool> UpdateUserAsync(UserUpdateRequestDto userRequest);

        Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId);
    }
}
