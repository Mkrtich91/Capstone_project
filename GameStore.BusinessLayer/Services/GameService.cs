namespace GameStore.BusinessLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using GameStore.BusinessLayer.Filters;
    using GameStore.BusinessLayer.Interfaces.DataProvider;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using GameStore.BusinessLayer.Pagination;
    using GameStore.BusinessLayer.Sorting;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IPlatformRepository _platformRepository;
        private readonly IPublisherRepository _publisherRepository;
        private static readonly GamePipeline _pipeline;

        static GameService()
        {
            _pipeline = new GamePipeline()
                .AddStep(new GenreFilterStep())
                .AddStep(new PlatformFilterStep())
                .AddStep(new PublisherFilterStep())
                .AddStep(new PriceFilterStep())
                .AddStep(new PublishDateFilterStep())
                .AddStep(new NameFilterStep())
                .AddStep(new GameSortStep())
                .AddStep(new GamePaginationStep());
        }

        public GameService(IGameRepository gameRepository, IGenreRepository genreRepository, IPlatformRepository platformRepository, IPublisherRepository publisherRepository)
        {
            _gameRepository = gameRepository;
            _genreRepository = genreRepository;
            _platformRepository = platformRepository;
            _publisherRepository = publisherRepository;
        }

        public async Task<Game> AddGameAsync(CreateGameRequest request)
        {
            foreach (var genreId in request.Genres)
            {
                var genre = await _genreRepository.GetGenreByIdAsync(genreId);
                if (genre == null)
                {
                    throw new NotFoundException($"Genre with ID {genreId} not found.");
                }
            }

            foreach (var platformId in request.Platforms)
            {
                var platform = await _platformRepository.GetPlatformByIdAsync(platformId);
                if (platform == null)
                {
                    throw new NotFoundException($"Platform with ID {platformId} not found.");
                }
            }

            var publisher = await _publisherRepository.GetPublisherByIdAsync(request.Publisher);
            if (publisher == null)
            {
                throw new NotFoundException($"Publisher with ID {request.Publisher} not found.");
            }

            var gameGenres = request.Genres.Select(genreId => new GameGenre { GenreId = genreId }).ToList();
            var gamePlatforms = request.Platforms.Select(platformId => new GamePlatform { PlatformId = platformId }).ToList();

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = request.Game.Name,
                Key = request.Game.Key,
                Description = request.Game.Description,
                Price = request.Game.Price,
                UnitInStock = request.Game.UnitInStock,
                Discount = request.Game.Discount,
                PublisherId = request.Publisher,
                GameGenres = gameGenres,
                GamePlatforms = gamePlatforms,
                PublishedDate = DateTime.UtcNow,
            };

            return await _gameRepository.AddGameAsync(game);
        }

        public async Task<GetGameResponse> GetGameByKeyAsync(string key)
        {
            var game = await _gameRepository.GetGameByKeyAsync(key);
            if (game == null)
            {
                throw new NotFoundException($"Game with Key {key} not found.");
            }

            game.ViewCount++;
            await _gameRepository.UpdateGameAsync(game);
            return new GetGameResponse
            {
                Id = game.Id,
                Name = game.Name,
                Key = game.Key,
                Description = game.Description,
                Price = game.Price,
                Discount = game.Discount,
                UnitInStock = game.UnitInStock,
                PublisherId = game.PublisherId,
                PublishedDate = game.PublishedDate,
                ViewCount = game.ViewCount,
            };
        }

        public async Task<GetGameResponse> GetGameByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var gameId))
            {
                throw new NotFoundException("Invalid game ID format.");
            }

            var game = await _gameRepository.GetGameByIdAsync(gameId);
            if (game == null)
            {
                throw new NotFoundException($"Game with ID {id} not found.");
            }

            game.ViewCount++;
            await _gameRepository.UpdateGameAsync(game);

            return new GetGameResponse
            {
                Id = game.Id,
                Description = game.Description,
                Key = game.Key,
                Name = game.Name,
                Price = game.Price,
                Discount = game.Discount,
                UnitInStock = game.UnitInStock,
                PublisherId = game.PublisherId,
                PublishedDate = game.PublishedDate,
                ViewCount = game.ViewCount,
            };
        }

        public async Task<List<GetGameResponse>> GetGamesByPlatformIdAsync(Guid platformId)
        {
            var games = await _gameRepository.GetGamesByPlatformIdAsync(platformId);
            if (games == null || games.Count == 0)
            {
                throw new NotFoundException($"No games found for platform with ID {platformId}.");
            }

            return games.Select(game => new GetGameResponse
            {
                Id = game.Id,
                Description = game.Description,
                Key = game.Key,
                Name = game.Name,
                Price = game.Price,
                Discount = game.Discount,
                UnitInStock = game.UnitInStock,
                PublisherId = game.PublisherId,
            }).ToList();
        }

        public async Task<IEnumerable<GetGameResponse>> GetGamesByGenreIdAsync(Guid genreId)
        {
            var games = await _gameRepository.GetGamesByGenreIdAsync(genreId);

            if (games == null || !games.Any())
            {
                throw new NotFoundException($"No games found for genre with ID {genreId}.");
            }

            return games.Select(game => new GetGameResponse
            {
                Id = game.Id,
                Description = game.Description,
                Key = game.Key,
                Name = game.Name,
                Price = game.Price,
                Discount = game.Discount,
                UnitInStock = game.UnitInStock,
                PublisherId = game.PublisherId,
            }).ToList();
        }

        public async Task<Game> UpdateGameAsync(GameUpdateRequest request)
        {
            var existingGame = await _gameRepository.GetGameByIdAsync(request.Game.Id);
            if (existingGame == null)
            {
                throw new NotFoundException("Game not found");
            }

            foreach (var genreId in request.Genres)
            {
                var genre = await _genreRepository.GetGenreByIdAsync(genreId);
                if (genre == null)
                {
                    throw new NotFoundException($"Genre with ID {genreId} not found.");
                }
            }

            foreach (var platformId in request.Platforms)
            {
                var platform = await _platformRepository.GetPlatformByIdAsync(platformId);
                if (platform == null)
                {
                    throw new NotFoundException($"Platform with ID {platformId} not found.");
                }
            }

            var publisher = await _publisherRepository.GetPublisherByIdAsync(request.Game.PublisherId);
            if (publisher == null)
            {
                throw new NotFoundException($"Publisher with ID {request.Game.PublisherId} not found.");
            }

            existingGame.Name = request.Game.Name;
            existingGame.Key = request.Game.Key;
            existingGame.Description = request.Game.Description;
            existingGame.Price = request.Game.Price;
            existingGame.UnitInStock = request.Game.UnitInStock;
            existingGame.Discount = request.Game.Discount;
            existingGame.PublisherId = request.Game.PublisherId;
            existingGame.PublishedDate = request.Game.PublishedDate;
            existingGame.ViewCount=request.Game.ViewCount;

            existingGame.GameGenres = new List<GameGenre>();
            foreach (var genreId in request.Genres)
            {
                existingGame.GameGenres.Add(new GameGenre { GenreId = genreId, GameId = existingGame.Id });
            }

            existingGame.GamePlatforms = new List<GamePlatform>();
            foreach (var platformId in request.Platforms)
            {
                existingGame.GamePlatforms.Add(new GamePlatform { PlatformId = platformId, GameId = existingGame.Id });
            }

            return await _gameRepository.UpdateGameAsync(existingGame);
        }

        public async Task DeleteGameByKeyAsync(string key)
        {
            var game = await _gameRepository.GetGameByKeyAsync(key);
            if (game == null)
            {
                throw new NotFoundException($"Game with Key {key} not found.");
            }

            await _gameRepository.DeleteGameAsync(game);
        }

        public async Task<byte[]> DownloadGameFileAsync(string key)
        {
            var game = await _gameRepository.GetGameByKeyAsync(key);
            if (game == null)
            {
                throw new NotFoundException("Game not found.");
            }

            var jsonData = JsonSerializer.Serialize(game);

            var byteArray = Encoding.UTF8.GetBytes(jsonData);

            return byteArray;
        }

        public async Task<IEnumerable<GetGameResponse>> GetAllGamesAsync()
            {
                var games = _gameRepository.GetAllGames();

                return games.Select(game => new GetGameResponse
                {
                    Id = game.Id,
                    Description = game.Description,
                    Key = game.Key,
                    Name = game.Name,
                    Price = game.Price,
                    Discount = game.Discount,
                    UnitInStock = game.UnitInStock,
                    PublisherId = game.PublisherId,
                    ViewCount = game.ViewCount,
                    PublishedDate = game.PublishedDate,
                }).ToList();
            }

        public async Task<IEnumerable<GetGenreDetailsResponse>> GetGenresByGameKeyAsync(string key)
        {
            var genres = await _gameRepository.GetGenresByGameKeyAsync(key);
            if (genres == null || !genres.Any())
            {
                throw new NotFoundException($"Genres not found for the provided game key {key}.");
            }

            return genres.Select(genre => new GetGenreDetailsResponse
            {
                Id = genre.Id,
                Name = genre.Name,
            });
        }

        public async Task<List<GetPlatformResponseModel>> GetPlatformsByGameKeyAsync(string key)
        {
            var game = await _gameRepository.GetGameByKeyAsync(key);

            if (game == null)
            {
                throw new NotFoundException($"No game found for game key: {key}");
            }

            var platforms = await _gameRepository.GetPlatformsByGameKeyAsync(key);
            if (platforms == null || platforms.Count == 0)
            {
                throw new NotFoundException($"No platforms found for game key: {key}");
            }

            var responseModels = platforms.Select(platform => new GetPlatformResponseModel
            {
                Id = platform.Id,
                Type = platform.Type,
            }).ToList();

            return responseModels;
        }

        public async Task<int> GetTotalGamesCountAsync()
        {
            return await _gameRepository.GetTotalGamesCountAsync();
        }

        public async Task<List<Game>> GetGamesByPublisherIdAsync(Guid publisherId)
        {
            return await _gameRepository.GetGamesByPublisherIdAsync(publisherId);
        }

        public async Task IncrementViewCountAsync(Guid gameId)
        {
            var gameIds = new List<Guid> { gameId };

            await _gameRepository.IncrementViewCountAsync(gameIds);
        }

        public async Task<GetGamesDetailsResponse> GetFilteredSortedPaginatedGamesAsync(GameQueryDto queryDto)
        {
            var gamesQuery = _pipeline.Execute(_gameRepository.GetAllGames(), queryDto);

            var totalCount = await gamesQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / queryDto.PageSize);
            var currentPage = queryDto.PageNumber;

            var games = await gamesQuery
                .Skip(currentPage * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .Select(g => new GameResponse
                {
                    Id = g.Id,
                    Name = g.Name,
                    Key = g.Key,
                    Description = g.Description,
                    Price = g.Price,
                    Discount = g.Discount,
                    UnitInStock = g.UnitInStock,
                })
                .ToListAsync();

            var gameIds = games.Select(g => g.Id).ToList();
            await _gameRepository.IncrementViewCountAsync(gameIds);

            var response = new GetGamesDetailsResponse
            {
                Games = games,
                TotalPages = totalPages,
                CurrentPage = currentPage,
            };

            return response;
        }

        public IEnumerable<string> GetPaginationOptions()
        {
            var options = new List<string>
    {
        PaginationOptions.Ten,
        PaginationOptions.Twenty,
        PaginationOptions.Fifty,
        PaginationOptions.Hundred,
        PaginationOptions.All,
    };

            return options;
        }

        public IEnumerable<string> GetSortingOptions()
        {
            var options = new List<string>
    {
        SortingOptions.MostPopular,
        SortingOptions.MostCommented,
        SortingOptions.PriceAsc,
        SortingOptions.PriceDesc,
        SortingOptions.New,
    };

            return options;
        }

        public IEnumerable<string> GetPublishDateFilterOptions()
        {
            var options = new List<string>
    {
        PublishDateFilterOptions.LastWeek,
        PublishDateFilterOptions.LastMonth,
        PublishDateFilterOptions.LastYear,
        PublishDateFilterOptions.TwoYears,
        PublishDateFilterOptions.ThreeYears,
    };

            return options;
        }

        public async Task<IEnumerable<GameOverviewDto>> FetchAllGamesAsync()
        {
            var games = await _gameRepository.GetAllGameAsync();

            return games.Select(game => new GameOverviewDto
            {
                Id = game.Id,
                Description = game.Description,
                Key = game.Key,
                Name = game.Name,
                Price = game.Price,
                Discount = game.Discount,
                UnitInStock = game.UnitInStock,
            });
        }
        public async Task<List<Game>> GetGamesByFiltersAsync(string name, string genre, Guid? platformId, double? minPrice, double? maxPrice)
        {
            return await _gameRepository.GetGamesByFiltersAsync(name, genre, platformId, minPrice, maxPrice);
        }
    }
}
