// <copyright file="UserRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO
{
    public class UserRequestDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
    }
}
