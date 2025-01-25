using MongoDB.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Repositories.IRepository
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllOrdersAsync();
        Task<IEnumerable<OrderDetail>> FindByOrderIdAsync(int orderId);
    }

}
