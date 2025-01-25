namespace GameStore.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext _dbContext;

        public GenreRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Genre> AddGenreAsync(Genre genre)
        {
            _dbContext.Genres.Add(genre);
            await _dbContext.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre> GetGenreByIdAsync(Guid id)
        {
            return await _dbContext.Genres.FindAsync(id);
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _dbContext.Genres.ToListAsync();
        }

        public async Task<List<Genre>> GetgenresByParentIdAsync(Guid parentId)
        {
            return await _dbContext.Genres
                .Where(g => g.ParentGenreId == parentId)
                .ToListAsync();
        }

        public async Task UpdateGenreAsync(Genre genre)
        {
            _dbContext.Entry(genre).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteGenreAsync(Genre genre)
        {
            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();
        }
    }
}
