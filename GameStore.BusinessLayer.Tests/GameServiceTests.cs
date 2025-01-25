// <copyright file="GameServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Tests
{
    using System.Text;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.BusinessLayer.Services;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Moq;

    public class GameServiceTests
    {
        private readonly Mock<IGameRepository> gameRepositoryMock;
        private readonly Mock<IGenreRepository> genreRepositoryMock;
        private readonly Mock<IPlatformRepository> platformRepositoryMock;
        private readonly Mock<IPublisherRepository> publisherRepositoryMock;
        private readonly GameService gameService;

        public GameServiceTests()
        {
            this.gameRepositoryMock = new Mock<IGameRepository>();
            this.genreRepositoryMock = new Mock<IGenreRepository>();
            this.platformRepositoryMock = new Mock<IPlatformRepository>();
            this.publisherRepositoryMock = new Mock<IPublisherRepository>();

            this.gameService = new GameService(
                this.gameRepositoryMock.Object,
                this.genreRepositoryMock.Object,
                this.platformRepositoryMock.Object,
                this.publisherRepositoryMock.Object);
        }

        [Fact]
        public async Task AddGameAsync_ValidRequest_ReturnsAddedGame()
        {
            var genreId = Guid.NewGuid();
            var platformId = Guid.NewGuid();
            var publisherId = Guid.NewGuid();

            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(genreId)).ReturnsAsync(new Genre { Id = genreId });
            this.platformRepositoryMock.Setup(repo => repo.GetPlatformByIdAsync(platformId)).ReturnsAsync(new Platform { Id = platformId });
            this.publisherRepositoryMock.Setup(repo => repo.GetPublisherByIdAsync(publisherId)).ReturnsAsync(new Publisher { Id = publisherId });
            this.gameRepositoryMock.Setup(repo => repo.AddGameAsync(It.IsAny<Game>())).ReturnsAsync(new Game());

            var request = new CreateGameRequest
            {
                Game = new GameDto
                {
                    Name = "Test Game",
                    Key = "test_game",
                    Description = "Description of test game",
                    Price = 59.99,
                    UnitInStock = 100,
                    Discount = 10,
                },
                Publisher = publisherId,
                Genres = new List<Guid> { genreId },
                Platforms = new List<Guid> { platformId },
            };

            var addedGame = await this.gameService.AddGameAsync(request);

            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(genreId), Times.Once);
            this.platformRepositoryMock.Verify(repo => repo.GetPlatformByIdAsync(platformId), Times.Once);
            this.publisherRepositoryMock.Verify(repo => repo.GetPublisherByIdAsync(publisherId), Times.Once);
            this.gameRepositoryMock.Verify(repo => repo.AddGameAsync(It.IsAny<Game>()), Times.Once);

            Assert.NotNull(addedGame);
        }

        [Fact]
        public async Task AddGameAsync_GenreNotFound_ThrowsNotFoundException()
        {
            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Genre?)null);

            var request = new CreateGameRequest
            {
                Game = new GameDto
                {
                    Name = "Test Game",
                    Key = "test_game",
                    Description = "Description of test game",
                    Price = 59.99,
                    UnitInStock = 100,
                    Discount = 10,
                },
                Publisher = Guid.NewGuid(),
                Genres = new List<Guid> { Guid.NewGuid() },
                Platforms = new List<Guid> { Guid.NewGuid() },
            };

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.gameService.AddGameAsync(request));

            this.gameRepositoryMock.Verify(repo => repo.AddGameAsync(It.IsAny<Game>()), Times.Never);
        }


        [Fact]
        public async Task AddGameAsync_PublisherNotFound_ThrowsNotFoundException()
        {
            var existingGenreId = Guid.NewGuid();
            var existingPlatformId = Guid.NewGuid();
            var nonExistentPublisherId = Guid.NewGuid();

            var request = new CreateGameRequest
            {
                Game = new GameDto
                {
                    Name = "Test Game",
                    Key = "test_game",
                    Description = "Description of test game",
                    Price = 59.99,
                    UnitInStock = 100,
                    Discount = 10,
                },
                Genres = new List<Guid> { existingGenreId },
                Platforms = new List<Guid> { existingPlatformId },
                Publisher = nonExistentPublisherId,
            };

            this.platformRepositoryMock
                .Setup(repo => repo.GetPlatformByIdAsync(existingPlatformId))
                .ReturnsAsync(new Platform { Id = existingPlatformId, Type = "Test Platform" });

            this.genreRepositoryMock
                .Setup(repo => repo.GetGenreByIdAsync(existingGenreId))
                .ReturnsAsync(new Genre { Id = existingGenreId, Name = "Test Genre" });

            this.publisherRepositoryMock
                .Setup(repo => repo.GetPublisherByIdAsync(nonExistentPublisherId))
                .ReturnsAsync((Publisher?)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.AddGameAsync(request));

            Assert.Equal($"Publisher with ID {nonExistentPublisherId} not found.", exception.Message);

            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(existingGenreId), Times.Once);
            this.platformRepositoryMock.Verify(repo => repo.GetPlatformByIdAsync(existingPlatformId), Times.Once);
            this.publisherRepositoryMock.Verify(repo => repo.GetPublisherByIdAsync(nonExistentPublisherId), Times.Once);
            this.gameRepositoryMock.Verify(repo => repo.AddGameAsync(It.IsAny<Game>()), Times.Never);
        }

        [Fact]
        public async Task AddGameAsync_GameNotFound_ThrowsNotFoundException()
        {
            var nonExistentGenreId = Guid.NewGuid();
            var gameDto = new GameDto
            {
                Name = "Test Game",
                Key = "test_game",
                Description = "Description of test game",
                Price = 59.99,
                UnitInStock = 100,
                Discount = 10,
            };

            var request = new CreateGameRequest
            {
                Game = gameDto,
                Genres = new List<Guid> { nonExistentGenreId },
                Platforms = new List<Guid> { Guid.NewGuid() },
                Publisher = Guid.NewGuid(),
            };

            this.genreRepositoryMock
                .Setup(repo => repo.GetGenreByIdAsync(nonExistentGenreId))
                .ReturnsAsync((Genre?)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.AddGameAsync(request));

            this.gameRepositoryMock.Verify(repo => repo.GetGameByIdAsync(It.IsAny<Guid>()), Times.Never); 
            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(nonExistentGenreId), Times.Once);

            Assert.Equal($"Genre with ID {nonExistentGenreId} not found.", exception.Message);
        }

        [Fact]
        public async Task AddGameAsync_GameGenreAndPlatformNotFound_ThrowsNotFoundException()
        {
            var nonExistentGenreId = Guid.NewGuid();
            var nonExistentPlatformId = Guid.NewGuid();

            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(nonExistentGenreId))
                                .ReturnsAsync((Genre?)null);
            this.platformRepositoryMock.Setup(repo => repo.GetPlatformByIdAsync(nonExistentPlatformId))
                                   .ReturnsAsync((Platform?)null);

            var request = new CreateGameRequest
            {
                Game = new GameDto
                {
                    Name = "Test Game",
                    Key = "test_game",
                    Description = "Description of test game",
                    Price = 59.99,
                    UnitInStock = 100,
                    Discount = 10,
                },
                Genres = new List<Guid> { nonExistentGenreId },
                Platforms = new List<Guid> { nonExistentPlatformId },
                Publisher = Guid.NewGuid(),
            };

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.gameService.AddGameAsync(request));

            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(nonExistentGenreId), Times.Once);
            this.platformRepositoryMock.Verify(repo => repo.GetPlatformByIdAsync(nonExistentPlatformId), Times.Never);
        }

        [Fact]
        public async Task GetGameByKeyAsync_GameNotFound_ThrowsNotFoundException()
        {
            var nonExistentGameKey = "non_existent_game_key";
            this.gameRepositoryMock.Setup(repo => repo.GetGameByKeyAsync(nonExistentGameKey))
                               .ReturnsAsync((Game?)null);

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.gameService.GetGameByKeyAsync(nonExistentGameKey));

            this.gameRepositoryMock.Verify(repo => repo.GetGameByKeyAsync(nonExistentGameKey), Times.Once);
        }

        [Fact]
        public async Task GetGameByKeyAsync_FoundGameByKey_ReturnsGame()
        {
            var existingGameKey = "existing_game_key";
            var expectedGame = new Game
            {
                Id = Guid.NewGuid(),
                Key = existingGameKey,
                Name = "Test Game",
                Description = "Description of Test Game",
                Price = 59.99,
                UnitInStock = 100,
                Discount = 10,
            };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByKeyAsync(existingGameKey))
                               .ReturnsAsync(expectedGame);

            var result = await this.gameService.GetGameByKeyAsync(existingGameKey);

            Assert.NotNull(result);
            Assert.Equal(expectedGame.Id, result.Id);
            Assert.Equal(expectedGame.Key, result.Key);
            Assert.Equal(expectedGame.Name, result.Name);
            Assert.Equal(expectedGame.Description, result.Description);
            Assert.Equal(expectedGame.Price, result.Price);
            Assert.Equal(expectedGame.UnitInStock, result.UnitInStock);
            Assert.Equal(expectedGame.Discount, result.Discount);

            this.gameRepositoryMock.Verify(repo => repo.GetGameByKeyAsync(existingGameKey), Times.Once);
        }

        [Fact]
        public async Task GetGameByIdAsync_FoundGameById_ReturnsGetGameResponse()
        {
            var gameId = Guid.NewGuid();
            var gameIdString = gameId.ToString();

            var expectedGame = new Game
            {
                Id = gameId,
                Key = "existing_game_key",
                Name = "Test Game",
                Description = "Description of Test Game",
                Price = 59.99,
                UnitInStock = 100,
                Discount = 10,
            };

            var expectedResponse = new GetGameResponse
            {
                Id = expectedGame.Id,
                Key = expectedGame.Key,
                Name = expectedGame.Name,
                Description = expectedGame.Description,
                Price = expectedGame.Price,
                UnitInStock = expectedGame.UnitInStock,
                Discount = expectedGame.Discount,
            };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId))
                               .ReturnsAsync(expectedGame);

            var result = await this.gameService.GetGameByIdAsync(gameIdString);

            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Id, result.Id);
            Assert.Equal(expectedResponse.Key, result.Key);
            Assert.Equal(expectedResponse.Name, result.Name);
            Assert.Equal(expectedResponse.Description, result.Description);
            Assert.Equal(expectedResponse.Price, result.Price);
            Assert.Equal(expectedResponse.UnitInStock, result.UnitInStock);
            Assert.Equal(expectedResponse.Discount, result.Discount);

            this.gameRepositoryMock.Verify(repo => repo.GetGameByIdAsync(gameId), Times.Once);
        }

        [Fact]
        public async Task GetGameByIdAsync_GameNotFoundById_ThrowsNotFoundException()
        {
            var nonExistentGameId = Guid.NewGuid().ToString();

            this.gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync((Game?)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
                await this.gameService.GetGameByIdAsync(nonExistentGameId));

            Assert.Equal($"Game with ID {nonExistentGameId} not found.", exception.Message);

            this.gameRepositoryMock.Verify(repo => repo.GetGameByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetGamesByPlatformIdAsync_ExistingPlatformbyIdFound_ReturnsAllListOfGames()
        {
            var platformId = Guid.NewGuid();
            var expectedGames = new List<Game>
    {
        new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "key1", Description = "Description 1", Price = 29.99, UnitInStock = 50, Discount = 5 },
        new Game { Id = Guid.NewGuid(), Name = "Game 2", Key = "key2", Description = "Description 2", Price = 49.99, UnitInStock = 30, Discount = 10 },
    };

            this.gameRepositoryMock.Setup(repo => repo.GetGamesByPlatformIdAsync(platformId))
                              .ReturnsAsync(expectedGames);

            var result = await this.gameService.GetGamesByPlatformIdAsync(platformId);

            Assert.NotNull(result);
            Assert.Equal(expectedGames.Count, result.Count);
            Assert.All(result, gameResponse =>
            {
                var correspondingGame = expectedGames.FirstOrDefault(game => game.Id == gameResponse.Id);
                Assert.NotNull(correspondingGame);
                Assert.Equal(correspondingGame.Name, gameResponse.Name);
                Assert.Equal(correspondingGame.Key, gameResponse.Key);
                Assert.Equal(correspondingGame.Description, gameResponse.Description);
                Assert.Equal(correspondingGame.Price, gameResponse.Price);
                Assert.Equal(correspondingGame.UnitInStock, gameResponse.UnitInStock);
                Assert.Equal(correspondingGame.Discount, gameResponse.Discount);
            });

            this.gameRepositoryMock.Verify(repo => repo.GetGamesByPlatformIdAsync(platformId), Times.Once);
        }

        [Fact]
        public async Task GetGamesByPlatformIdAsync_NotGamesFoundById_ThrowsNotFoundException()
        {
            var platformId = Guid.NewGuid();

            this.gameRepositoryMock.Setup(repo => repo.GetGamesByPlatformIdAsync(platformId))
                               .ReturnsAsync(new List<Game>());

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.gameService.GetGamesByPlatformIdAsync(platformId));

            gameRepositoryMock.Verify(repo => repo.GetGamesByPlatformIdAsync(platformId), Times.Once);
        }

        [Fact]
        public async Task GetGamesByGenreIdAsync_GenreFound_ReturnsListOfMatchingGames()
        {
            var genreId = Guid.NewGuid();
            var expectedGames = new List<Game>
    {
        new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "key1", Description = "Description 1", Price = 29.99, UnitInStock = 50, Discount = 5 },
        new Game { Id = Guid.NewGuid(), Name = "Game 2", Key = "key2", Description = "Description 2", Price = 49.99, UnitInStock = 30, Discount = 10 },
    };

            this.gameRepositoryMock.Setup(repo => repo.GetGamesByGenreIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(expectedGames);

            var result = await this.gameService.GetGamesByGenreIdAsync(genreId);

            Assert.NotNull(result);
            Assert.Equal(expectedGames.Count, result.Count());
            Assert.All(result, gameResponse =>
            {
                var correspondingGame = expectedGames.FirstOrDefault(game => game.Id == gameResponse.Id);
                Assert.NotNull(correspondingGame);
                Assert.Equal(correspondingGame.Name, gameResponse.Name);
                Assert.Equal(correspondingGame.Key, gameResponse.Key);
                Assert.Equal(correspondingGame.Description, gameResponse.Description);
                Assert.Equal(correspondingGame.Price, gameResponse.Price);
                Assert.Equal(correspondingGame.UnitInStock, gameResponse.UnitInStock);
                Assert.Equal(correspondingGame.Discount, gameResponse.Discount);
            });

            this.gameRepositoryMock.Verify(repo => repo.GetGamesByGenreIdAsync(genreId), Times.Once);
        }

        [Fact]
        public async Task GetGamesByGenreIdAsync_GamesNotFoundById_ThrowsNotFoundException()
        {
            var genreId = Guid.NewGuid();
            this.gameRepositoryMock.Setup(repo => repo.GetGamesByGenreIdAsync(genreId))
                              .ReturnsAsync(new List<Game>());

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.gameService.GetGamesByGenreIdAsync(genreId));

            this.gameRepositoryMock.Verify(repo => repo.GetGamesByGenreIdAsync(genreId), Times.Once);
        }

        [Fact]
        public async Task UpdateGameAsync_GameExistsbyId_ReturnsUpdatedGame()
        {
            var gameId = Guid.NewGuid();
            var request = new GameUpdateRequest
            {
                Game = new GameUpdateDto { Id = gameId, Name = "Updated Game", Key = "updated_key", Description = "Updated description" },
                Genres = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                Platforms = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
            };

            var existingGame = new Game
            {
                Id = gameId,
                Name = "Existing Game",
                Key = "existing_key",
                Description = "Existing description",
            };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId))
                              .ReturnsAsync(existingGame);

            this.gameRepositoryMock.Setup(repo => repo.UpdateGameAsync(existingGame))
                              .ReturnsAsync(existingGame);

            this.genreRepositoryMock.SetupSequence(repo => repo.GetGenreByIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(new Genre())
                                .ReturnsAsync(new Genre());

            this.platformRepositoryMock.SetupSequence(repo => repo.GetPlatformByIdAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(new Platform())
                                   .ReturnsAsync(new Platform());

            this.publisherRepositoryMock.Setup(repo => repo.GetPublisherByIdAsync(It.IsAny<Guid>()))
                                    .ReturnsAsync(new Publisher());

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            var result = await gameService.UpdateGameAsync(request);

            Assert.NotNull(result);
            Assert.Equal(request.Game.Name, result.Name);
            Assert.Equal(request.Game.Key, result.Key);
            Assert.Equal(request.Game.Description, result.Description);

            this.gameRepositoryMock.Verify(repo => repo.GetGameByIdAsync(gameId), Times.Once);
            this.gameRepositoryMock.Verify(repo => repo.UpdateGameAsync(existingGame), Times.Once);
        }

        [Fact]
        public async Task UpdateGameAsync_GameNotFound_ThrowsNotFoundException()
        {
            var gameId = Guid.NewGuid();
            var request = new GameUpdateRequest
            {
                Game = new GameUpdateDto { Id = gameId, Name = "Updated Game", Key = "updated_key", Description = "Updated description" },
                Genres = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                Platforms = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
            };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId))
                              .ReturnsAsync((Game?)null);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.gameService.UpdateGameAsync(request));

            this.gameRepositoryMock.Verify(repo => repo.GetGameByIdAsync(gameId), Times.Once);

            this.gameRepositoryMock.Verify(repo => repo.UpdateGameAsync(It.IsAny<Game>()), Times.Never);
        }

        [Fact]
        public async Task UpdateGameAsync_GenresNotFound_ThrowsNotFoundException()
        {
            var gameId = Guid.NewGuid();
            var genreIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var request = new GameUpdateRequest
            {
                Game = new GameUpdateDto { Id = gameId, Name = "Updated Game", Key = "updated_key", Description = "Updated description" },
                Genres = genreIds,
                Platforms = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
            };

            var existingGame = new Game
            {
                Id = gameId,
                Name = "Existing Game",
                Key = "existing_key",
                Description = "Existing description",
            };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId))
                              .ReturnsAsync(existingGame);

            this.genreRepositoryMock.SetupSequence(repo => repo.GetGenreByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(new Genre())
                               .ReturnsAsync((Genre?)null);

            this.platformRepositoryMock.SetupSequence(repo => repo.GetPlatformByIdAsync(It.IsAny<Guid>()))
                                  .ReturnsAsync(new Platform())
                                  .ReturnsAsync(new Platform());

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.UpdateGameAsync(request));

            this.gameRepositoryMock.Verify(repo => repo.GetGameByIdAsync(gameId), Times.Once);

            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(It.IsAny<Guid>()), Times.Exactly(genreIds.Count));
        }

        [Fact]
        public async Task UpdateGameAsync_PlatformsNotFound_ThrowsNotFoundException()
        {
            var gameId = Guid.NewGuid();
            var platformIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var request = new GameUpdateRequest
            {
                Game = new GameUpdateDto { Id = gameId, Name = "Updated Game", Key = "updated_key", Description = "Updated description" },
                Genres = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                Platforms = platformIds,
            };

            var existingGame = new Game
            {
                Id = gameId,
                Name = "Existing Game",
                Key = "existing_key",
                Description = "Existing description",
            };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId))
                              .ReturnsAsync(existingGame);

            this.genreRepositoryMock.SetupSequence(repo => repo.GetGenreByIdAsync(It.IsAny<Guid>()))
                               .ReturnsAsync(new Genre())
                               .ReturnsAsync(new Genre());

            this.platformRepositoryMock.SetupSequence(repo => repo.GetPlatformByIdAsync(It.IsAny<Guid>()))
                                  .ReturnsAsync(new Platform())
                                  .ReturnsAsync((Platform?)null);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.UpdateGameAsync(request));

            this.gameRepositoryMock.Verify(repo => repo.GetGameByIdAsync(gameId), Times.Once);

            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(It.IsAny<Guid>()), Times.Exactly(request.Genres.Count));

            this.platformRepositoryMock.Verify(repo => repo.GetPlatformByIdAsync(It.IsAny<Guid>()), Times.Exactly(platformIds.Count));
        }

        [Fact]
        public async Task UpdateGameAsync_NotEntitiesFound_ThrowsNotFoundException()
        {
            var gameId = Guid.NewGuid();
            var genreIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var platformIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var publisherId = Guid.NewGuid();
            var request = new GameUpdateRequest
            {
                Game = new GameUpdateDto { Id = gameId, Name = "Updated Game", Key = "updated_key", Description = "Updated description" },
                Genres = genreIds,
                Platforms = platformIds,
                Publisher = publisherId,
            };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(gameId))
                               .ReturnsAsync((Game?)null);

            this.genreRepositoryMock.SetupSequence(repo => repo.GetGenreByIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync((Genre?)null)
                                .ReturnsAsync((Genre?)null);

            this.platformRepositoryMock.SetupSequence(repo => repo.GetPlatformByIdAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync((Platform?)null)
                                   .ReturnsAsync((Platform?)null);

            this.publisherRepositoryMock.Setup(repo => repo.GetPublisherByIdAsync(publisherId))
                                    .ReturnsAsync((Publisher?)null);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.gameService.UpdateGameAsync(request));
        }

        [Fact]
        public async Task DeleteGameByKeyAsync_GameFound_DeletesGame()
        {
            var existingKey = "existing_key";
            var existingGame = new Game { Key = existingKey };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByKeyAsync(existingKey))
                               .ReturnsAsync(existingGame);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await gameService.DeleteGameByKeyAsync(existingKey);

            this.gameRepositoryMock.Verify(repo => repo.GetGameByKeyAsync(existingKey), Times.Once);
            this.gameRepositoryMock.Verify(repo => repo.DeleteGameAsync(existingGame), Times.Once);
        }

        [Fact]
        public async Task DeleteGameByKeyAsync_GameNotFound_ThrowsNotFoundException()
        {
            var nonExistentKey = "non_existent_key";

            this.gameRepositoryMock.Setup(repo => repo.GetGameByKeyAsync(nonExistentKey))
                               .ReturnsAsync((Game?)null);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.DeleteGameByKeyAsync(nonExistentKey));

            this.gameRepositoryMock.Verify(repo => repo.GetGameByKeyAsync(nonExistentKey), Times.Once);
            this.gameRepositoryMock.Verify(repo => repo.DeleteGameAsync(It.IsAny<Game>()), Times.Never);
        }

        [Fact]
        public async Task DownloadGameFileAsync_ExistingGameFound_ReturnsJsonByteArray()
        {
            var key = "test_key";
            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = "Test Game",
                Key = key,
                Description = "Description of test game",
            };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByKeyAsync(key))
                              .ReturnsAsync(game);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            var result = await gameService.DownloadGameFileAsync(key);

            Assert.NotNull(result);
            var expectedJsonData = System.Text.Json.JsonSerializer.Serialize(game);
            var expectedByteArray = Encoding.UTF8.GetBytes(expectedJsonData);
            Assert.Equal(expectedByteArray, result);
        }

        [Fact]
        public async Task DownloadGameFileAsync_GameNotFound_ThrowsNotFoundException()
        {
            var key = "non_existing_key";
            gameRepositoryMock.Setup(repo => repo.GetGameByKeyAsync(key))
                               .ReturnsAsync((Game?)null);

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.gameService.DownloadGameFileAsync(key));
        }

        [Fact]
        public async Task GetAllGamesAsync_WhenThreeGamesExist_ReturnsListOfThreeGames()
        {
            var games = new List<Game>
    {
        new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "game_1", Description = "Description of Game 1", Price = 49.99, UnitInStock = 100, Discount = 5, PublisherId = Guid.NewGuid() },
        new Game { Id = Guid.NewGuid(), Name = "Game 2", Key = "game_2", Description = "Description of Game 2", Price = 29.99, UnitInStock = 50, Discount = 10, PublisherId = Guid.NewGuid() },
        new Game { Id = Guid.NewGuid(), Name = "Game 3", Key = "game_3", Description = "Description of Game 3", Price = 19.99, UnitInStock = 200, Discount = 0, PublisherId = Guid.NewGuid() },
    };

            this.gameRepositoryMock.Setup(repo => repo.GetAllGames()).Returns(games.AsQueryable());

            var result = await this.gameService.GetAllGamesAsync();

            Assert.Collection(
                result,
                game =>
                {
                    Assert.Equal(games[0].Id, game.Id);
                    Assert.Equal(games[0].Name, game.Name);
                    Assert.Equal(games[0].Key, game.Key);
                    Assert.Equal(games[0].Description, game.Description);
                    Assert.Equal(games[0].Price, game.Price);
                    Assert.Equal(games[0].Discount, game.Discount);
                    Assert.Equal(games[0].UnitInStock, game.UnitInStock);
                    Assert.Equal(games[0].PublisherId, game.PublisherId);
                },
                game =>
                {
                    Assert.Equal(games[1].Id, game.Id);
                    Assert.Equal(games[1].Name, game.Name);
                    Assert.Equal(games[1].Key, game.Key);
                    Assert.Equal(games[1].Description, game.Description);
                    Assert.Equal(games[1].Price, game.Price);
                    Assert.Equal(games[1].Discount, game.Discount);
                    Assert.Equal(games[1].UnitInStock, game.UnitInStock);
                    Assert.Equal(games[1].PublisherId, game.PublisherId);
                },
                game =>
                {
                    Assert.Equal(games[2].Id, game.Id);
                    Assert.Equal(games[2].Name, game.Name);
                    Assert.Equal(games[2].Key, game.Key);
                    Assert.Equal(games[2].Description, game.Description);
                    Assert.Equal(games[2].Price, game.Price);
                    Assert.Equal(games[2].Discount, game.Discount);
                    Assert.Equal(games[2].UnitInStock, game.UnitInStock);
                    Assert.Equal(games[2].PublisherId, game.PublisherId);
                });
        }

        [Fact]
        public async Task GetAllGamesAsync_WhenNoGamesExist_ReturnsEmptyList()
        {
            this.gameRepositoryMock.Setup(repo => repo.GetAllGameAsync()).ReturnsAsync(new List<Game>());

            var result = await this.gameService.GetAllGamesAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetGenresByGameKeyAsync_GameFoundByKey_ReturnsGenres()
        {
            var key = "test_key";
            var expectedGenres = new List<Genre>
    {
        new Genre { Id = Guid.NewGuid(), Name = "Genre 1" },
        new Genre { Id = Guid.NewGuid(), Name = "Genre 2" },
    };

            this.gameRepositoryMock.Setup(repo => repo.GetGenresByGameKeyAsync(key))
                               .ReturnsAsync(expectedGenres);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            var result = await gameService.GetGenresByGameKeyAsync(key);

            Assert.Collection(
                result,
                item =>
                {
                    Assert.Equal(expectedGenres[0].Id, item.Id);
                    Assert.Equal(expectedGenres[0].Name, item.Name);
                },
                item =>
                {
                    Assert.Equal(expectedGenres[1].Id, item.Id);
                    Assert.Equal(expectedGenres[1].Name, item.Name);
                });
        }

        [Fact]
        public async Task GetGenresByGameKeyAsync_GameNotFound_ThrowsNotFoundException()
        {
            var key = "non_existent_key";
            this.gameRepositoryMock.Setup(repo => repo.GetGenresByGameKeyAsync(key))
                               .ReturnsAsync((List<Genre>?)null);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.GetGenresByGameKeyAsync(key));
        }

        [Fact]
        public async Task GetGenresByGameKeyAsync_NotGenresFound_ThrowsNotFoundException()
        {
            var key = "non_existent_key";
            this.gameRepositoryMock.Setup(repo => repo.GetGenresByGameKeyAsync(key))
                               .ReturnsAsync(new List<Genre>());

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.GetGenresByGameKeyAsync(key));
        }

        [Fact]
        public async Task GetPlatformsByGameKeyAsync_GameFoundByKey_ReturnsPlatforms()
        {
            var key = "test_key";
            var expectedPlatforms = new List<Platform>
    {
        new Platform { Id = Guid.NewGuid(), Type = "Platform 1" },
        new Platform { Id = Guid.NewGuid(), Type = "Platform 2" },
    };

            this.gameRepositoryMock.Setup(repo => repo.GetGameByKeyAsync(key))
                               .ReturnsAsync(new Game());

            this.gameRepositoryMock.Setup(repo => repo.GetPlatformsByGameKeyAsync(key))
                               .ReturnsAsync(expectedPlatforms);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            var result = await gameService.GetPlatformsByGameKeyAsync(key);

            Assert.Collection(
                result,
                item =>
                {
                    Assert.Equal(expectedPlatforms[0].Id, item.Id);
                    Assert.Equal(expectedPlatforms[0].Type, item.Type);
                },
                item =>
                {
                    Assert.Equal(expectedPlatforms[1].Id, item.Id);
                    Assert.Equal(expectedPlatforms[1].Type, item.Type);
                });
        }

        [Fact]
        public async Task GetPlatformsByGameKeyAsync_GameNotFound_ThrowsNotFoundException()
        {
            var key = "non_existing_key";
            gameRepositoryMock.Setup(repo => repo.GetGameByKeyAsync(key))
                               .ReturnsAsync((Game?)null);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.GetPlatformsByGameKeyAsync(key));
        }

        [Fact]
        public async Task GetPlatformsByGameKeyAsync_NotPlatformsFound_ThrowsNotFoundException()
        {
            var key = "test_key";
            this.gameRepositoryMock.Setup(repo => repo.GetGameByKeyAsync(key))
                               .ReturnsAsync(new Game());

            this.gameRepositoryMock.Setup(repo => repo.GetPlatformsByGameKeyAsync(key))
                               .ReturnsAsync(new List<Platform>());

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await gameService.GetPlatformsByGameKeyAsync(key));
        }

        [Fact]
        public async Task GetTotalGamesCountAsync_Exist10Games_ReturnsTotalCount()
        {
            var expectedCount = 10;
            this.gameRepositoryMock.Setup(repo => repo.GetTotalGamesCountAsync())
                               .ReturnsAsync(expectedCount);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            var result = await gameService.GetTotalGamesCountAsync();

            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public async Task GetTotalGamesCountAsync_NoGamesFound_ReturnsZero()
        {
            var expectedCount = 0;
            this.gameRepositoryMock.Setup(repo => repo.GetTotalGamesCountAsync())
                               .ReturnsAsync(expectedCount);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            var result = await gameService.GetTotalGamesCountAsync();

            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public async Task GetGamesByPublisherIdAsync_ValidPublisherId_ReturnsGames()
        {
            var publisherId = Guid.NewGuid();
            var games = new List<Game>
    {
        new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "key1", Description = "Description 1", Price = 49.99, UnitInStock = 100, Discount = 10, PublisherId = publisherId },
        new Game { Id = Guid.NewGuid(), Name = "Game 2", Key = "key2", Description = "Description 2", Price = 59.99, UnitInStock = 150, Discount = 15, PublisherId = publisherId },
    };

            this.gameRepositoryMock.Setup(repo => repo.GetGamesByPublisherIdAsync(publisherId))
                               .ReturnsAsync(games);

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, this.platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            var result = await gameService.GetGamesByPublisherIdAsync(publisherId);

            Assert.Equal(games, result);
        }

        [Fact]
        public async Task GetGamesByPublisherIdAsync_InvalidPublisherId_ReturnsEmptyList()
        {
            var invalidPublisherId = Guid.NewGuid();

            this.gameRepositoryMock.Setup(repo => repo.GetGamesByPublisherIdAsync(invalidPublisherId))
                               .ReturnsAsync(new List<Game>());

            var gameService = new GameService(this.gameRepositoryMock.Object, this.genreRepositoryMock.Object, platformRepositoryMock.Object, this.publisherRepositoryMock.Object);

            var result = await gameService.GetGamesByPublisherIdAsync(invalidPublisherId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
