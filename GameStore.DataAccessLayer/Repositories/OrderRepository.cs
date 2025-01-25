namespace GameStore.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOpenOrderByCustomerIdAsync(Guid customerId)
        {
            return await _context.Orders
                .Include(o => o.OrderGames)
                .FirstOrDefaultAsync(o => o.CustomerId == customerId && o.Status == OrderStatus.Open);
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderGames)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(params OrderStatus[] statuses)
        {
            return await _context.Orders
                .Where(o => statuses.Contains(o.Status))
                .ToListAsync();
        }

        public async Task<Order> GetOrCreateOpenOrderByCustomerIdAsync(Guid customerId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.CustomerId == customerId && o.Status == OrderStatus.Open);

            if (order == null)
            {
                order = new Order
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    CustomerId = customerId,
                    Status = OrderStatus.Open,
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }

            return order;
        }

        public async Task<Order> GetOpenOrderAsync(Guid customerId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.CustomerId == customerId && o.Status == OrderStatus.Open);
        }

        public async Task<IEnumerable<OrderGame>> GetOrderGamesByOrderIdAsync(Guid orderId)
        {
            return await _context.OrderGames
                .Where(og => og.OrderId == orderId)
                .ToListAsync();
        }
    }
}
