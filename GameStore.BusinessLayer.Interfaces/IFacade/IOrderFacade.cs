// <copyright file="IOrderFacade.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IFacade
{
    using GameStore.BusinessLayer.Interfaces.ResponseDto;

    public interface IOrderFacade
    {
        Task<GetOrderResponse> GetOrderByIdAsync(string orderId);

        Task<IEnumerable<GetOrderResponse>> GetAllOrdersAsync();
    }
}
