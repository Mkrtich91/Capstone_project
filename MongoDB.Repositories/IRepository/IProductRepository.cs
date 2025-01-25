using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MongoDB.Repositories.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> FindByProductIdAsync(int productId);
        Task UpdateAsync(Product product);
        Task AddGameKeysToProductsAsync();

    }

}
