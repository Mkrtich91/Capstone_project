// <copyright file="BanUserRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.RequestDto
{
    public class BanUserRequest
    {
        public string? User { get; set; }

        public string? Duration { get; set; }
    }
}
