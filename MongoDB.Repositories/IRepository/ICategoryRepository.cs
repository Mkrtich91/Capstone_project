using GameStore.DataAccessLayer.Interfaces.Entities;
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
    public interface ICategoryRepository
    {

        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category> GetCategoryByIdAsync(int categoryId);
    }

}
