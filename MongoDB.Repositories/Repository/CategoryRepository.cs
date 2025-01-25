using AutoMapper;
using GameStore.DataAccessLayer.Interfaces.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NorthwindDataContext _context;

        public CategoryRepository(NorthwindDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.Find(c => true).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.Find(c => c.CategoryID == categoryId).FirstOrDefaultAsync();
        }
    }
}
