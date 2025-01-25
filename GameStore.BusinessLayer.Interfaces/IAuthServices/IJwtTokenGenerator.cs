// <copyright file="IJwtTokenGenerator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IAuthServices
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity;

    public interface IJwtTokenGenerator
    {
        Task<string> GenerateTokenAsync(IdentityUser applicationUser, IEnumerable<string> roles, IEnumerable<Claim>? additionalClaims = null);
    }
}
