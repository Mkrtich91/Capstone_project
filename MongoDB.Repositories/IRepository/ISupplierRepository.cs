using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repositories.IRepository
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierByIdAsync(int supplierId);
        Task<Supplier> GetSupplierByNameAsync(string supplierName);
    }

}
