using MongoDB.Driver;
using MongoDB.Entities.Entities;
using MongoDB.Entities.MongoDbContext;
using MongoDB.Repositories.IRepository;

namespace MongoDB.Repositories.Repository
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly NorthwindDataContext _context;

        public ShipperRepository(NorthwindDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shipper>> GetAllShippersAsync()
        {
            return await _context.Shippers.Find(_ => true).ToListAsync();
        }

        public async Task<Shipper> GetShipperByIdAsync(int shipperId)
        {
            return await _context.Shippers.Find(shipper => shipper.ShipperID == shipperId).FirstOrDefaultAsync();
        }
    }
}
