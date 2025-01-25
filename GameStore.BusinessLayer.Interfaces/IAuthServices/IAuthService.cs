// <copyright file="IAuthService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IAuthServices
{
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.ResponseDTO;
    using Microsoft.AspNetCore.Mvc;

    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);

        Task<IActionResult> HasAccessToPageAsync(AccessRequestDto request);
    }
}
