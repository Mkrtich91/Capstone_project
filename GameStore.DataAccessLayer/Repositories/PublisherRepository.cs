namespace GameStore.DataAccessLayer.Repositories
{
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class PublisherRepository : IPublisherRepository
    {
        private readonly DataContext _dbContext;

        public PublisherRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Publisher> AddPublisherAsync(Publisher publisher)
        {
            _dbContext.Publishers.Add(publisher);
            await _dbContext.SaveChangesAsync();
            return publisher;
        }

        public async Task<Publisher> GetPublisherByIdAsync(Guid id)
        {
            return await _dbContext.Publishers.FindAsync(id);
        }

        public async Task<Publisher> GetPublisherByNameAsync(string name)
        {
            return await _dbContext.Publishers.FirstOrDefaultAsync(p => p.CompanyName == name);
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
        {
            return await _dbContext.Publishers.ToListAsync();
        }

        public async Task<Publisher> UpdatePublisherAsync(Publisher publisher)
        {
            _dbContext.Publishers.Update(publisher);
            await _dbContext.SaveChangesAsync();
            return publisher;
        }

        public async Task DeletePublisherAsync(Guid id)
        {
            var publisher = await _dbContext.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _dbContext.Publishers.Remove(publisher);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Publisher> GetPublisherByGameKeyAsync(string key)
        {
#pragma warning disable CS8603
            return await this._dbContext.Games.Where(g => g.Key == key).Select(g => g.Publisher).FirstOrDefaultAsync();
#pragma warning restore CS8603
        }
    }
}
