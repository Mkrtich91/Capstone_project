// <copyright file="CreateRoleRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO
{
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.WrappedDTOs;

    public class CreateRoleRequestDto
    {
        public RoleDetailsDto? Role { get; set; }

        public List<string>? Permissions { get; set; }
    }
}
