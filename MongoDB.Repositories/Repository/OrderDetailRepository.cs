using GameStore.BusinessLayer.Interfaces.Exceptions;
using MongoDB.Driver;
using MongoDB.Entities.Entities;
using MongoDB.Entities.MongoDbContext;
using MongoDB.Repositories.IRepository;
namespace MongoDB.Repositories.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly NorthwindDataContext _context;

        public OrderDetailRepository(NorthwindDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrdersAsync()
        {
            return await _context.OrderDetails.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> FindByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails.Find(detail => detail.OrderID == orderId).ToListAsync();
        }


    }
}
