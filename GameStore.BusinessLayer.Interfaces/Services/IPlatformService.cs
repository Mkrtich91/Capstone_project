// <copyright file="IPlatformService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IPlatformService
    {
        Task<Platform> CreatePlatformAsync(CreatePlatformRequest request);

        Task<GetPlatformResponseModel> GetPlatformByIdAsync(Guid id);

        Task<List<GetPlatformResponseModel>> GetAllPlatformsAsync();

        Task<Platform> UpdatePlatformAsync(UpdatePlatformDto updateDto);

        Task DeletePlatformAsync(Guid id);
    }
}
