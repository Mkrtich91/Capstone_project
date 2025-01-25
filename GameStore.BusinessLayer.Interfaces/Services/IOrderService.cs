// <copyright file="IOrderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IOrderService
    {
        Task AddGameToOrderAsync(string gameKey);

        Task RemoveGameFromOrderAsync(string gameKey);

        Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);

        Task<GetOrderResponse> GetOrderByIdAsync(string orderId);

        Task<IEnumerable<GetOrderResponse>> GetPaidAndCancelledOrdersAsync();

        Task<IEnumerable<CartItemDto>> GetCartAsync();

        Task<CartDetailsDto> GetCartDetailsAsync(Guid customerId);

        Task ShipOrderAsync(Guid orderId);
    }
}
