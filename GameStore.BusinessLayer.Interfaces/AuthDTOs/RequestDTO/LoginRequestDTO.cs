// <copyright file="LoginRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO
{
    public class LoginRequestDto
    {
        public string? Login { get; set; }

        public string? Password { get; set; }

        public bool InternalAuth { get; set; }
    }
}
