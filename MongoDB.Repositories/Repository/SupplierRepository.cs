using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities.Entities;
using MongoDB.Entities.MongoDbContext;
using MongoDB.Repositories.IRepository;

namespace MongoDB.Repositories.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly NorthwindDataContext _context;

        public SupplierRepository(NorthwindDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.Find(_ => true).ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int supplierId)
        {
            return await _context.Suppliers.Find(supplier => supplier.SupplierID == supplierId).FirstOrDefaultAsync();
        }

        public async Task<Supplier> GetSupplierByNameAsync(string supplierName)
        {
            return await _context.Suppliers.Find(supplier => supplier.CompanyName == supplierName).FirstOrDefaultAsync();
        }
    }
}
