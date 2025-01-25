// <copyright file="UserUpdateRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO
{
    public class UserUpdateRequestDto
    {
        public UserRequestDto? User { get; set; }

        public List<Guid>? Roles { get; set; }

        public string? Password { get; set; }
    }
}
