// <copyright file="IGameServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IGameService
    {
        Task<Game> AddGameAsync(CreateGameRequest request);

        Task<GetGameResponse> GetGameByKeyAsync(string key);

        Task<GetGameResponse> GetGameByIdAsync(string id);

        Task<List<GetGameResponse>> GetGamesByPlatformIdAsync(Guid platformId);

        Task<IEnumerable<GetGameResponse>> GetGamesByGenreIdAsync(Guid genreId);

        Task<Game> UpdateGameAsync(GameUpdateRequest request);

        Task DeleteGameByKeyAsync(string key);

        Task<byte[]> DownloadGameFileAsync(string key);

        Task<IEnumerable<GetGameResponse>> GetAllGamesAsync();

        Task<IEnumerable<GetGenreDetailsResponse>> GetGenresByGameKeyAsync(string key);

        Task<List<GetPlatformResponseModel>> GetPlatformsByGameKeyAsync(string key);

        Task<int> GetTotalGamesCountAsync();

        Task<List<Game>> GetGamesByPublisherIdAsync(Guid publisherId);

        Task IncrementViewCountAsync(Guid gameId);

        IEnumerable<string> GetPaginationOptions();

        IEnumerable<string> GetSortingOptions();

        IEnumerable<string> GetPublishDateFilterOptions();

        Task<GetGamesDetailsResponse> GetFilteredSortedPaginatedGamesAsync(GameQueryDto queryDto);

        Task<IEnumerable<GameOverviewDto>> FetchAllGamesAsync();

        Task<List<Game>> GetGamesByFiltersAsync(string name, string genre, Guid? platformId, double? minPrice, double? maxPrice);
    }
}
