using MongoDB.Driver;
using MongoDB.Entities.Entities;
using MongoDB.Entities.MongoDbContext;
using MongoDB.Repositories.IRepository;

namespace MongoDB.Repositories.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly NorthwindDataContext _context;

        public ProductRepository(NorthwindDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> FindByProductIdAsync(int productId)
        {
            return await _context.Products.Find(p => p.ProductID == productId).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductID, product.ProductID);
            await _context.Products.ReplaceOneAsync(filter, product);
        }

        public async Task AddGameKeysToProductsAsync()
        {
            var products = await GetAllAsync();
            foreach (var product in products)
            {
                if (string.IsNullOrEmpty(product.GameKey))
                {
                    product.GameKey = $"GAME-{product.ProductID:D3}";
                    await UpdateAsync(product);
                }
            }
        }
    }
}