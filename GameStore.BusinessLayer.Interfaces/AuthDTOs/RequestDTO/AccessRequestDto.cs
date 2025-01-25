// <copyright file="AccessRequestDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO
{
    public class AccessRequestDto
    {
        public string? TargetPage { get; set; }

        public string? TargetId { get; set; }
    }
}
