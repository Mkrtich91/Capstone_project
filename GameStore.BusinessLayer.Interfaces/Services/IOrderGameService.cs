// <copyright file="IOrderGameService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IOrderGameService
    {
        Task AddGameToOrderAsync(Guid orderId, Game game);

        Task<OrderGame> GetOrderGameByOrderIdAndGameIdAsync(Guid orderId, Guid gameId);

        Task RemoveGameFromOrderAsync(Guid orderId, Guid gameId);

        Task<IEnumerable<GetOrderGameResponse>> GetOrderDetailsByOrderIdAsync(string orderId);

        Task<IEnumerable<GetOrderGameResponse>> GetAllOrderGamesAsync(bool includeRecent = false);

        Task UpdateGameQuantityInOrderAsync(string orderGameId, int count);

        Task RemoveOrderGameByIdAsync(string orderGameId);

        Task AddGameToOrderByIdAsync(Guid orderId, string gameKey, int quantity);
    }
}
