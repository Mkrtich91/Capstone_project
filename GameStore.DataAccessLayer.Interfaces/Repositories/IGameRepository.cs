namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IGameRepository
    {
        Task<int> GetTotalGamesCountAsync();

        Task<Game> AddGameAsync(Game game);

        Task<Game> GetGameByKeyAsync(string key);

        Task<Game> GetGameByIdAsync(Guid id);

        Task<List<Game>> GetGamesByPlatformIdAsync(Guid platformId);

        Task<IEnumerable<Game>> GetGamesByGenreIdAsync(Guid genreId);

        Task<Game> UpdateGameAsync(Game game);

        Task DeleteGameAsync(Game game);

        Task<IEnumerable<Game>> GetAllGameAsync();

        Task<IEnumerable<Genre>> GetGenresByGameKeyAsync(string key);

        Task<List<Platform>> GetPlatformsByGameKeyAsync(string key);

        Task<List<Game>> GetGamesByPublisherIdAsync(Guid publisherId);

        IQueryable<Game> GetAllGames();

        Task IncrementViewCountAsync(IEnumerable<Guid> gameIds);
        Task<List<Game>> GetGamesByFiltersAsync(string name, string genre, Guid? platformId, double? minPrice, double? maxPrice);
    }
}
