// <copyright file="IPublisherFacade.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IFacade
{
    using GameStore.BusinessLayer.Interfaces.ResponseDto;

    public interface IPublisherFacade
    {
        Task<IEnumerable<GetPublisherResponse>> GetAllPublishersAsync();

        Task<GetPublisherResponse> GetPublisherByIdAsync(string id);

        Task<GetPublisherResponse> GetPublisherByNameAsync(string companyName);
    }
}
