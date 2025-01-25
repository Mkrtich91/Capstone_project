namespace GameStore.DataAccessLayer.Tests
{
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using GameStore.DataAccessLayer.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Options;
    using Moq;

    public class GameRepositoryTests
    {
        private DbContextOptions<DataContext> _options;

        public GameRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                 .UseInMemoryDatabase($"{nameof(GameRepositoryTests)}_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddGameAsync_ValidGameIsProvided_ShouldReturnAddedGame()
        {
            using var context = new DataContext(_options);
            var game = new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "Key1", Description = "Description 1" };
            var expectedGame = new Game { Id = game.Id, Name = game.Name, Key = game.Key, Description = game.Description };

            var gameRepository = new GameRepository(context);

            var result = await gameRepository.AddGameAsync(game);

            Assert.NotNull(result);
            Assert.Equal(expectedGame.Id, result.Id);
            Assert.Equal(expectedGame.Name, result.Name);
            Assert.Equal(expectedGame.Key, result.Key);
            Assert.Equal(expectedGame.Description, result.Description);
        }

        [Fact]
        public async Task GetGameByKeyAsync_GameKeyExists_ReturnExistingGame()
        {
            using (var context = new DataContext(_options))
            {
                var key = "Key1";
                var genreId = Guid.NewGuid();
                var platformId = Guid.NewGuid();
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Game 1",
                    Key = key,
                    Description = "Description 1",
                };

                context.Games.Add(game);
                context.SaveChanges();

                var gameRepository = new GameRepository(context);

                var result = await gameRepository.GetGameByKeyAsync(key);

                Assert.NotNull(result);
                Assert.Equal(game.Name, result.Name);
                Assert.Equal(game.Key, result.Key);
                Assert.Equal(game.Description, result.Description);

                var gameInContext = await context.Games.FirstOrDefaultAsync(g => g.Key == key);
                Assert.NotNull(gameInContext);
                Assert.Equal(game.Name, gameInContext.Name);
                Assert.Equal(game.Description, gameInContext.Description);
            }
        }


        [Fact]
        public async Task GetGameByKeyAsyn_GameKeyDoesNotExistc_ShouldReturnNull()
        {
            using (var context = new DataContext(_options))
            {
                var key = "NonExistingKey";
                var gameRepository = new GameRepository(context);

                var result = await gameRepository.GetGameByKeyAsync(key);

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetGameByIdAsync_WhenGameExists_ReturnCorrectGame()
        {
            using var context = new DataContext(_options);
            var gameId = Guid.NewGuid();
            var game = new Game { Id = gameId, Name = "Test Game", Key = "TestKey", Description = "Test Description" };

            context.Games.Add(game);
            context.SaveChanges();

            var gameRepository = new GameRepository(context);

            var result = await gameRepository.GetGameByIdAsync(gameId);

            Assert.Equal(game, result);
        }

        [Fact]
        public async Task GetGameByIdAsync_IdDoesNotExist_ShouldReturnNull()
        {
            using var context = new DataContext(_options);
            var nonExistentId = Guid.NewGuid();
            var gameRepository = new GameRepository(context);

            var result = await gameRepository.GetGameByIdAsync(nonExistentId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetGamesByPlatformIdAsync_ThreeGamesExist_ShouldReturnAllThreeGames()
        {
            using var context = new DataContext(_options);
            var platformId = Guid.NewGuid();
            var games = new List<Game>
        {
            new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "Key1", Description = "Description 1" },
            new Game { Id = Guid.NewGuid(), Name = "Game 2", Key = "Key2", Description = "Description 2" },
            new Game { Id = Guid.NewGuid(), Name = "Game 3", Key = "Key3", Description = "Description 3" },
        };
            var gamePlatforms = games.Select(game => new GamePlatform { GameId = game.Id, PlatformId = platformId }).ToList();

            context.Games.AddRange(games);
            context.GamePlatforms.AddRange(gamePlatforms);
            context.SaveChanges();

            var gameRepository = new GameRepository(context);

            var result = await gameRepository.GetGamesByPlatformIdAsync(platformId);

            Assert.Equal(games.Count, result.Count);
            Assert.All(result, game => Assert.Contains(gamePlatforms, gp => gp.GameId == game.Id));
        }

        [Fact]
        public async Task GetGamesByPlatformIdAsync_PlatformIdDoesNotExist_ReturnEmptyList()
        {

            using var context = new DataContext(_options);
            var nonExistentPlatformId = Guid.NewGuid();
            var gameRepository = new GameRepository(context);

            var result = await gameRepository.GetGamesByPlatformIdAsync(nonExistentPlatformId);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetGamesByGenreIdAsync_NoGamesMatchGenreId_ShouldReturnEmptyList()
        {
            using (var context = new DataContext(_options))
            {
                var genreId = Guid.NewGuid();
                var matchingGenreId = Guid.NewGuid();
                var nonMatchingGenreId = Guid.NewGuid();

                var game1 = new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "Key1", Description = "Description 1" };
                var game2 = new Game { Id = Guid.NewGuid(), Name = "Game 2", Key = "Key2", Description = "Description 2" };
                var game3 = new Game { Id = Guid.NewGuid(), Name = "Game 3", Key = "Key3", Description = "Description 3" };

                game1.GameGenres = new List<GameGenre> { new GameGenre { GameId = game1.Id, GenreId = matchingGenreId } };
                game2.GameGenres = new List<GameGenre> { new GameGenre { GameId = game2.Id, GenreId = matchingGenreId } };
                game3.GameGenres = new List<GameGenre> { new GameGenre { GameId = game3.Id, GenreId = nonMatchingGenreId } };

                context.Games.AddRange(new List<Game> { game1, game2, game3 });
                context.SaveChanges();

                var gameRepository = new GameRepository(context);

                var result = await gameRepository.GetGamesByGenreIdAsync(genreId);

                Assert.Equal(0, result.Count());
                Assert.DoesNotContain(result, g => g.Name == "Game 1");
                Assert.DoesNotContain(result, g => g.Name == "Game 2");
            }
        }

        [Fact]
        public async Task GetGamesByGenreIdAsync_GenreIdNotFound_ReturnEmptyList()
        {
            var nonExistentGenreId = Guid.NewGuid();


            using (var context = new DataContext(_options))
            {
                var gameRepository = new GameRepository(context);

                var result = await gameRepository.GetGamesByGenreIdAsync(nonExistentGenreId);

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task UpdateGameAsync_GameIsFound_UpdateExistingGameInDatabase()
        {
            var gameId = Guid.NewGuid();
            var game = new Game
            {
                Id = gameId,
                Name = "Game 1",
                Key = "Key1",
                Description = "Description 1",
            };

            using (var context = new DataContext(_options))
            {
                context.Games.Add(game);
                context.SaveChanges();

                var gameRepository = new GameRepository(context);

                game.Name = "Updated Game";
                game.Key = "Updated Key";
                game.Description = "Updated Description";

                var updatedGame = await gameRepository.UpdateGameAsync(game);

                using (var contextForVerification = new DataContext(_options))
                {
                    var gameFromDatabase = await contextForVerification.Games.FindAsync(gameId);

                    Assert.NotNull(gameFromDatabase);
                    Assert.Equal("Updated Game", gameFromDatabase.Name);
                    Assert.Equal("Updated Description", gameFromDatabase.Description);
                    Assert.Equal("Updated Key", gameFromDatabase.Key);
                }
            }
        }

        [Fact]
        public async Task UpdateGameAsync_GameIsNotFoundById_ShouldThrowException()
        {
            var nonExistentGameId = Guid.NewGuid();
            var updatedGame = new Game
            {
                Id = nonExistentGameId,
                Name = "Updated Game",
                Key = "Updated Key",
                Description = "Updated Description"
            };

            using (var context = new DataContext(_options))
            {
                var gameRepository = new GameRepository(context);

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
                {
                    await gameRepository.UpdateGameAsync(updatedGame);
                });
            }
        }

        [Fact]
        public async Task DeleteGameAsync_GameIsFound_RemoveGameFromDatabase()
        {
            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = "Game 1",
                Key = "Key1",
                Description = "Description 1",
            };

            using (var context = new DataContext(_options))
            {
                context.Games.Add(game);
                context.SaveChanges();

                var gameRepository = new GameRepository(context);

                await gameRepository.DeleteGameAsync(game);

                Assert.DoesNotContain(game, context.Games);
            }
        }

        [Fact]
        public async Task DeleteGameAsync_RemoveGame_AddedGameFromDatabase()
        {
            using (var context = new DataContext(_options))
            {
                var gameRepository = new GameRepository(context);

                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Game",
                    Key = "Test Key",
                    Description = "Test Description"
                };

                await gameRepository.AddGameAsync(game);

                await gameRepository.DeleteGameAsync(game);

                var deletedGame = await gameRepository.GetGameByIdAsync(game.Id);
                Assert.Null(deletedGame);
            }
        }

        [Fact]
        public async Task DeleteGameAsync_GameIsNotFound_ShouldThrowException()
        {
            using (var context = new DataContext(_options))
            {
                var gameRepository = new GameRepository(context);

                var nonExistingGame = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Non-existing Game",
                    Key = "Non-existing Key",
                    Description = "Non-existing Description",
                };

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await gameRepository.DeleteGameAsync(nonExistingGame));
            }
        }

        [Fact]
        public async Task GetTotalGames_MultipleGamesExist_ReturnCorrectGameCount()
        {
            using (var context = new DataContext(_options))
            {
                var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "Key1", Description = "Description 1" },
                new Game { Id = Guid.NewGuid(), Name = "Game 2", Key = "Key2", Description = "Description 2" },
                new Game { Id = Guid.NewGuid(), Name = "Game 3", Key = "Key3", Description = "Description 3" },
            };

                context.Games.AddRange(games);
                context.SaveChanges();

                var gameRepository = new GameRepository(context);

                var result = await gameRepository.GetTotalGamesCountAsync();

                Assert.Equal(games.Count, result);
            }
        }

        [Fact]
        public async Task GetAllAsync_TwoGamesExist_ShouldReturnTwoGames()
        {

            using (var context = new DataContext(_options))
            {
                var game1 = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Game 1",
                    Key = "Key1",
                    Description = "Description 1",
                };

                var game2 = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Game 2",
                    Key = "Key2",
                    Description = "Description 2",
                };

                context.Games.AddRange(game1, game2);
                context.SaveChanges();

                var gameRepository = new GameRepository(context);

                var result = await gameRepository.GetAllGameAsync();

                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.Contains(game1, result);
                Assert.Contains(game2, result);
            }
        }

        [Fact]
        public async Task GetAllAsync_EmptyDatabase_NoGamesFound()
        {
            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();

                var gameRepository = new GameRepository(context);

                var result = await gameRepository.GetAllGameAsync();

                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task GetGenresByGameKeyAsync_ReturnGenresAssociatedWithGame_ByGameKey()
        {
            var gameKey = "Key1";
            var genre1 = new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Genre 1",
            };

            var genre2 = new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Genre 2",
            };

            var expectedGenres = new List<Genre> { genre1, genre2 };

            using (var context = new DataContext(_options))
            {
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Game 1",
                    Key = gameKey,
                    Description = "Description 1",
                };

                context.Games.Add(game);
                context.Genres.AddRange(expectedGenres);
                context.SaveChanges();

                var gameGenres = expectedGenres.Select(genre => new GameGenre
                {
                    GameId = game.Id,
                    GenreId = genre.Id,
                }).ToList();

                context.GameGenres.AddRange(gameGenres);
                context.SaveChanges();

                var gameRepository = new GameRepository(context);

                var genres = await gameRepository.GetGenresByGameKeyAsync(gameKey);

                Assert.NotNull(genres);
                Assert.Equal(2, genres.Count());
                Assert.Contains(genre1, genres);
                Assert.Contains(genre2, genres);
            }
        }

        [Fact]
        public async Task GetGenresByGameKeyAsync_GenresNotFound_ReturnEmptyList()
        {
            using (var context = new DataContext(_options))
            {
                var gameRepository = new GameRepository(context);

                var gameKey = "Non-existing Key";
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Non-existing Game",
                    Key = gameKey,
                    Description = "Non-existing Description",
                };
                context.Games.Add(game);
                await context.SaveChangesAsync();

                var genres = await gameRepository.GetGenresByGameKeyAsync(gameKey);

                Assert.NotNull(genres);
                Assert.Empty(genres);
            }
        }

        [Fact]
        public async Task GetGenresByGameKeyAsync_WhenNoGenresExist_ShouldReturnEmpty()
        {
            var gameKey = "NonExistentKey";


            using (var context = new DataContext(_options))
            {
                context.Genres.RemoveRange(context.Genres);
                context.Games.RemoveRange(context.Games);
                context.GameGenres.RemoveRange(context.GameGenres);
                context.SaveChanges();

                var repository = new GameRepository(context);

                var genres = await repository.GetGenresByGameKeyAsync(gameKey);

                Assert.Null(genres);
            }
        }

        [Fact]
        public async Task GetPlatformsByGameKeyAsync_PlatformsNotFoundByKey_ReturnEmptyList()
        {
            using (var context = new DataContext(_options))
            {
                var gameRepository = new GameRepository(context);

                var gameKey = "Non-existing Key";
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Non-existing Game",
                    Key = gameKey,
                    Description = "Non-existing Description",
                };
                context.Games.Add(game);
                await context.SaveChangesAsync();

                var platforms = await gameRepository.GetPlatformsByGameKeyAsync(gameKey);

                Assert.NotNull(platforms);
                Assert.Empty(platforms);
            }
        }

        [Fact]
        public async Task GetGamesByPublisherIdAsync_ValidPublisherId_ReturnsGames()
        {
            var publisherId = Guid.NewGuid();
            var games = new List<Game>
    {
        new Game { Id = Guid.NewGuid(), Name = "Game 1", Key = "key1", Description = "Description 1", PublisherId = publisherId },
        new Game { Id = Guid.NewGuid(), Name = "Game 2", Key = "key2", Description = "Description 2", PublisherId = publisherId },
    };

            using (var context = new DataContext(_options))
            {
                context.Games.AddRange(games);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var gameRepository = new GameRepository(context);

                var result = await gameRepository.GetGamesByPublisherIdAsync(publisherId);

                Assert.NotNull(result);
                Assert.Equal(games.Count, result.Count);
                Assert.All(result, game => Assert.Equal(publisherId, game.PublisherId));
            }
        }
    }
}
