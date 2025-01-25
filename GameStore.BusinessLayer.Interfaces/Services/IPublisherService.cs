// <copyright file="IPublisherService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IPublisherService
    {
        Task<Publisher> AddPublisherAsync(PublisherRequest publisher);

        Task<GetPublisherResponse> GetPublisherByIdAsync(string id);

        Task<GetPublisherResponse> GetPublisherByNameAsync(string name);

        Task<IEnumerable<GetPublisherResponse>> GetAllPublishersAsync();

        Task<PublisherUpdateDto> UpdatePublisherAsync(UpdatePublisherRequest request);

        Task DeletePublisherAsync(Guid id);

        Task<GetPublisherResponse> GetPublisherByGameKeyAsync(string key);

        Task<List<PublisherGameDto>> GetGamesByPublisherNameAsync(string companyName);
    }
}
