// <copyright file="RoleDtoWrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.AuthDTOs.WrappedDTOs
{
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;

    public class RoleDtoWrapper
    {
        public RoleRequestDto? Role { get; set; }

        public List<string>? Permissions { get; set; }
    }
}
