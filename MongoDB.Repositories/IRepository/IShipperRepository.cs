using MongoDB.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repositories.IRepository
{
    public interface IShipperRepository
    {
        Task<IEnumerable<Shipper>> GetAllShippersAsync();
        Task<Shipper> GetShipperByIdAsync(int shipperId);
    }

}
