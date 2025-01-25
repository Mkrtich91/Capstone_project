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

    public class GameRepository : IGameRepository
    {
        private readonly DataContext _dbContext;

        public GameRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetTotalGamesCountAsync()
        {
            return await _dbContext.Games.CountAsync();
        }

        public async Task<Game> AddGameAsync(Game game)
        {
            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();
            return game;
        }

        public async Task<Game> GetGameByKeyAsync(string key)
        {
            return await _dbContext.Games.FirstOrDefaultAsync(g => g.Key == key);
        }

        public async Task<Game> GetGameByIdAsync(Guid id)
        {
            return await _dbContext.Games.FindAsync(id);
        }

        public async Task<List<Game>> GetGamesByPlatformIdAsync(Guid platformId)
        {
            return await _dbContext.GamePlatforms
                .Where(gp => gp.PlatformId == platformId)
                .Select(gp => gp.Game)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByGenreIdAsync(Guid genreId)
        {
            return await _dbContext.Games
                .Where(g => g.GameGenres.Any(gg => gg.GenreId == genreId))
                .ToListAsync();
        }

        public async Task<Game> UpdateGameAsync(Game game)
        {
            _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();
            return game;
        }

        public async Task DeleteGameAsync(Game game)
        {
            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetAllGameAsync()
        {
            return await _dbContext.Games.ToListAsync();
        }

        public async Task<IEnumerable<Genre>> GetGenresByGameKeyAsync(string key)
        {
            var game = await _dbContext.Games
                .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
                .FirstOrDefaultAsync(g => g.Key == key);

            return game?.GameGenres.Select(gg => gg.Genre);
        }

        public async Task<List<Platform>> GetPlatformsByGameKeyAsync(string key)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Key == key);

            var platformIds = await _dbContext.GamePlatforms
                .Where(gp => gp.GameId == game.Id)
                .Select(gp => gp.PlatformId)
                .ToListAsync();

            var platforms = await _dbContext.Platforms
                .Where(p => platformIds.Contains(p.Id))
                .ToListAsync();

            return platforms;
        }

        public async Task<List<Game>> GetGamesByPublisherIdAsync(Guid publisherId)
        {
            return await _dbContext.Games.Where(g => g.PublisherId == publisherId).ToListAsync();
        }

        public async Task IncrementViewCountAsync(IEnumerable<Guid> gameIds)
        {
            var games = await _dbContext.Games
                .Where(g => gameIds.Contains(g.Id))
                .ToListAsync();

            foreach (var game in games)
            {
                game.ViewCount++;
            }

            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Game> GetAllGames()
        {
            var games = _dbContext.Games.ToList();
            games.ForEach(game => game.ViewCount++);

            _dbContext.SaveChanges();

            return games.AsQueryable();
        }
        public async Task<List<Game>> GetGamesByFiltersAsync(string name, string genre, Guid? platformId, double? minPrice, double? maxPrice)
        {
            // Prepare a list to store queries
            var tasks = new List<Task<IQueryable<Game>>>();

            if (!string.IsNullOrEmpty(name))
                tasks.Add(Task.FromResult(_dbContext.Games.Where(g => g.Name.Contains(name))));

            if (!string.IsNullOrEmpty(genre))
                tasks.Add(Task.FromResult(_dbContext.Games.Where(g => g.GameGenres.Any(gg => gg.Genre.Name == genre))));

            if (platformId.HasValue)
                tasks.Add(Task.FromResult(_dbContext.Games.Where(g => g.GamePlatforms.Any(gp => gp.PlatformId == platformId))));

            if (minPrice.HasValue)
                tasks.Add(Task.FromResult(_dbContext.Games.Where(g => g.Price >= minPrice)));

            if (maxPrice.HasValue)
                tasks.Add(Task.FromResult(_dbContext.Games.Where(g => g.Price <= maxPrice)));

            // Wait for all tasks to complete
            var results = await Task.WhenAll(tasks);

            // Combine the results using intersection to ensure all filters are applied
            var finalQuery = results.Aggregate((IQueryable<Game>)_dbContext.Games, (current, query) => current.Intersect(query));

            // Return the combined result as a list
            return await finalQuery.ToListAsync();
        }
    }
}
