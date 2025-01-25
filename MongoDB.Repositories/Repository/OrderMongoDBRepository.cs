using MongoDB.Driver;
using MongoDB.Entities.Converter;
using MongoDB.Entities.Entities;
using MongoDB.Entities.MongoDbContext;
using MongoDB.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repositories.Repository
{
    public class OrderMongoDBRepository : IOrderMongoDBRepository
    {
        private readonly NorthwindDataContext _context;

        public OrderMongoDBRepository(NorthwindDataContext context)
        {
            _context = context;
        }


        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderId);
            return await _context.Orders.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Find(_ => true).ToListAsync();
        }
    }
}

