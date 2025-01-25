// <copyright file="JwtOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Configuration
{
    public class JwtOptions
    {
        public string? Secret { get; set; }

        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        public int ExpirationMinutes { get; set; }
    }
}
