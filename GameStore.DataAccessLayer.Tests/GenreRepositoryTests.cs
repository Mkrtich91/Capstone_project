namespace GameStore.DataAccessLayer.Tests
{
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using GameStore.DataAccessLayer.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class GenreRepositoryTests
    {

        private readonly DbContextOptions<DataContext> _options;

        public GenreRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{nameof(GenreRepositoryTests)}_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddGenreAsync_ValidGenre_ReturnsAddedGenre()
        {
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Action" };
            Genre addedGenre;

            using (var context = new DataContext(_options))
            {
                var repository = new GenreRepository(context);
                addedGenre = await repository.AddGenreAsync(genre);
            }

            using (var context = new DataContext(_options))
            {
                var result = await context.Genres.FindAsync(addedGenre.Id);
                Assert.NotNull(result);
                Assert.Equal(genre.Name, result.Name);
            }
        }

        [Fact]
        public async Task GetGenreByIdAsync_IfGenreExists_ReturnCorrectGenre()
        {
            var genreId = Guid.NewGuid();
            var expectedName = "Action";
            Genre retrievedGenre;

            using (var context = new DataContext(_options))
            {
                context.Genres.Add(new Genre { Id = genreId, Name = expectedName });
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new GenreRepository(context);
                retrievedGenre = await repository.GetGenreByIdAsync(genreId);
            }

            Assert.NotNull(retrievedGenre);
            Assert.Equal(expectedName, retrievedGenre.Name);
        }

        [Fact]
        public async Task GetGenreByIdAsync_IdNotFound_ReturnNull()
        {
            var nonExistentGenreId = Guid.NewGuid();
            Genre result;

            using (var context = new DataContext(_options))
            {
                var repository = new GenreRepository(context);
                result = await repository.GetGenreByIdAsync(nonExistentGenreId);
            }

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllGenresAsync_ThreeGenresFromDatabase_ReturnAllGenres()
        {
            var expectedGenres = new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Action" },
            new Genre { Id = Guid.NewGuid(), Name = "Adventure" },
            new Genre { Id = Guid.NewGuid(), Name = "Strategy" },
        };
            IEnumerable<Genre> result;

            using (var context = new DataContext(_options))
            {
                context.Genres.AddRange(expectedGenres);
                await context.SaveChangesAsync();

                var repository = new GenreRepository(context);
                result = await repository.GetAllGenresAsync();
            }

            Assert.NotNull(result);
            Assert.Equal(expectedGenres.Count, result.Count());
            Assert.Equal(expectedGenres, result);
        }

        [Fact]
        public async Task GetAllGenresAsync_NoGenresExist_ShouldReturnEmptyList()
        {
            IEnumerable<Genre> result;

            using (var context = new DataContext(_options))
            {
                var repository = new GenreRepository(context);
                result = await repository.GetAllGenresAsync();
            }

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetgenresByParentIdAsync_GenreByParentId_ReturnGenres_()
        {
            var parentId = Guid.NewGuid();
            var expectedGenres = new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Action", ParentGenreId = parentId },
            new Genre { Id = Guid.NewGuid(), Name = "Adventure", ParentGenreId = parentId },
            new Genre { Id = Guid.NewGuid(), Name = "Strategy", ParentGenreId = parentId },
        };
            IEnumerable<Genre> result;

            using (var context = new DataContext(_options))
            {
                context.Genres.AddRange(expectedGenres);
                await context.SaveChangesAsync();

                var repository = new GenreRepository(context);
                result = await repository.GetgenresByParentIdAsync(parentId);
            }

            Assert.NotNull(result);
            Assert.Equal(expectedGenres.Count, result.Count());
            Assert.Equal(expectedGenres, result);
        }

        [Fact]
        public async Task GetgenresByParentIdAsync_IfParentIdNotFound_ReturnEmptyList()
        {
            using (var context = new DataContext(_options))
            {
                var genreRepository = new GenreRepository(context);

                var parentGenreId = Guid.NewGuid();
                var parentGenre = new Genre
                {
                    Id = parentGenreId,
                    Name = "Parent Genre",
                };
                context.Genres.Add(parentGenre);
                await context.SaveChangesAsync();

                var genres = await genreRepository.GetgenresByParentIdAsync(Guid.NewGuid());

                Assert.NotNull(genres);
                Assert.Empty(genres);
            }
        }

        [Fact]
        public async Task UpdateGenreAsync_GenreInDatabase_FailsToUpdate()
        {

            var genreId = Guid.NewGuid();
            var genre = new Genre
            {
                Id = genreId,
                Name = "Action",
            };

            using (var context = new DataContext(_options))
            {
                context.Genres.Add(genre);
                context.SaveChanges();
            }

            using (var context = new DataContext(_options))
            {
                var genreRepository = new GenreRepository(context);

                await genreRepository.UpdateGenreAsync(genre);
            }

            using (var context = new DataContext(_options))
            {
                var updatedGenre = await context.Genres.FindAsync(genre.Id);
                Assert.NotNull(updatedGenre);
                Assert.Equal("Action", updatedGenre.Name);
            }
        }

        [Fact]
        public async Task UpdateGenreAsync_ShouldUpdate_Genre()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new GenreRepository(context);

                var genre = new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Action",
                    ParentGenreId = null
                };

                await repository.AddGenreAsync(genre);

                var updatedGenre = new Genre
                {
                    Id = genre.Id,
                    Name = "Updated Action",
                    ParentGenreId = null
                };

                context.Entry(genre).State = EntityState.Detached;

                await repository.UpdateGenreAsync(updatedGenre);

                var retrievedGenre = await repository.GetGenreByIdAsync(genre.Id);

                Assert.NotNull(retrievedGenre);
                Assert.Equal(updatedGenre.Name, retrievedGenre.Name);
                Assert.Equal(updatedGenre.ParentGenreId, retrievedGenre.ParentGenreId);
            }
        }

        [Fact]
        public async Task UpdateGenreAsync_UpdateExistingGenre_ReturnDbUpdateConcurrencyException()
        {

            using (var context = new DataContext(_options))
            {
                var repository = new GenreRepository(context);

                var genre = new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Action",
                    ParentGenreId = null
                };

                await repository.AddGenreAsync(genre);

                var updatedGenre = new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Updated Action",
                    ParentGenreId = null
                };

                context.Entry(genre).State = EntityState.Detached;

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
                {
                    await repository.UpdateGenreAsync(updatedGenre);
                });
            }
        }

        [Fact]
        public async Task DeleteGameAsync_GameNotFound_NotThrowException()
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

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => gameRepository.DeleteGameAsync(nonExistingGame));
            }
        }

        [Fact]
        public async Task DeleteGenreAsync_DeleteGenreById_ReturnDeleteGenreById()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new GenreRepository(context);

                var genre = new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "Action",
                    ParentGenreId = null
                };

                await repository.AddGenreAsync(genre);

                await repository.DeleteGenreAsync(genre);

                var deletedGenre = await repository.GetGenreByIdAsync(genre.Id);

                Assert.Null(deletedGenre);
            }
        }

    }
}
