using GameStore.DataAccessLayer.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    public interface IOrderGameRepository
    {
        Task<OrderGame> GetOrderGameByOrderIdAndGameIdAsync(Guid orderId, Guid gameId);

        Task AddOrderGameAsync(OrderGame orderGame);

        Task UpdateOrderGameAsync(OrderGame orderGame);

        Task RemoveOrderGameAsync(OrderGame orderGame);

        Task<IEnumerable<OrderGame>> GetOrderGamesByOrderIdAsync(Guid orderId);

        Task<int> GetTotalQuantityByGameIdAsync(Guid gameId);

        Task SaveChangesAsync();

        Task<IEnumerable<OrderGame>> GetAllOrderGamesAsync();

        Task<OrderGame> GetOrderGameByIdAsync(Guid orderGameId);
        Task RemoveOrderGameByIdAsync(Guid orderGameId);
    }
}
