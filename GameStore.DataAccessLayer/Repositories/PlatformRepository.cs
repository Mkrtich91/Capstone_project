namespace GameStore.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class PlatformRepository : IPlatformRepository
    {
        private readonly DataContext _dbContext;

        public PlatformRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Platform> CreatePlatformAsync(Platform platform)
        {
            await _dbContext.Platforms.AddAsync(platform);
            await _dbContext.SaveChangesAsync();
            return platform;
        }

        public async Task<Platform> GetPlatformByIdAsync(Guid id)
        {
            return await _dbContext.Platforms.FindAsync(id);
        }

        public async Task<List<Platform>> GetAllPlatformsAsync()
        {
            return await _dbContext.Platforms.ToListAsync();
        }

        public async Task UpdatePlatformAsync(Platform platform)
        {
            _dbContext.Platforms.Update(platform);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteplatformAsync(Platform platform)
        {
            _dbContext.Platforms.Remove(platform);
            await _dbContext.SaveChangesAsync();
        }
    }
}
