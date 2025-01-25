using GameStore.DataAccessLayer.Interfaces.Entities;

namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);

        Task<Order> GetOpenOrderByCustomerIdAsync(Guid customerId);

        Task UpdateOrderAsync(Order order);

        Task DeleteOrderAsync(Guid orderId);

        Task<Order> GetOrderByIdAsync(Guid orderId);

        Task<IEnumerable<Order>> GetOrdersByStatusAsync(params OrderStatus[] statuses);

        Task<Order> GetOrCreateOpenOrderByCustomerIdAsync(Guid customerId);

        Task<Order> GetOpenOrderAsync(Guid customerId);

        Task<IEnumerable<OrderGame>> GetOrderGamesByOrderIdAsync(Guid orderId);
    }
}
