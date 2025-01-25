namespace GameStore.DataAccessLayer.Repositories
{
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class OrderGameRepository : IOrderGameRepository
    {
        private readonly DataContext _context;

        public OrderGameRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<OrderGame> GetOrderGameByOrderIdAndGameIdAsync(Guid orderId, Guid gameId)
        {
            return await _context.OrderGames
                .FirstOrDefaultAsync(og => og.OrderId == orderId && og.GameId == gameId);
        }

        public async Task AddOrderGameAsync(OrderGame orderGame)
        {
            _context.OrderGames.Add(orderGame);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderGameAsync(OrderGame orderGame)
        {
            _context.OrderGames.Update(orderGame);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOrderGameAsync(OrderGame orderGame)
        {
            _context.OrderGames.Remove(orderGame);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderGame>> GetOrderGamesByOrderIdAsync(Guid orderId)
        {
            return await _context.OrderGames.Where(og => og.OrderId == orderId).ToListAsync();
        }

        public async Task<int> GetTotalQuantityByGameIdAsync(Guid gameId)
        {
            return await _context.OrderGames
                .Where(og => og.GameId == gameId)
                .SumAsync(og => og.Quantity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderGame>> GetAllOrderGamesAsync()
        {
            return await _context.OrderGames
                .Include(og => og.Order)
                .Include(og => og.Game)
                .ToListAsync();
        }

        public async Task<OrderGame> GetOrderGameByIdAsync(Guid orderGameId)
        {
            return await _context.OrderGames
                .Include(og => og.Order)
                .Include(og => og.Game)
                .FirstOrDefaultAsync(og => og.OrderGameId == orderGameId);
        }

        public async Task RemoveOrderGameByIdAsync(Guid orderGameId)
        {
            var orderGame = await GetOrderGameByIdAsync(orderGameId);
            if (orderGame != null)
            {
                _context.OrderGames.Remove(orderGame);
                await _context.SaveChangesAsync();
            }
        }
    }
}
