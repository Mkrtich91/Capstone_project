namespace GameStore.DataAccessLayer.Tests
{
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using GameStore.DataAccessLayer.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Moq;

    public class PlatformRepositoryTests
    {
        private readonly DataContext _context;
        private readonly IPlatformRepository _platformRepository;

        public PlatformRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{nameof(GenreRepositoryTests)}_{Guid.NewGuid()}")
                .Options;
            _context = new DataContext(options);
            _platformRepository = new PlatformRepository(_context);
        }

        [Fact]
        public async Task CreatePlatformAsync_CreatePlatform_ShouldCreatePlatformSuccessfully()
        {
            var platform = new Platform
            {
                Id = Guid.NewGuid(),
                Type = "Test Platform Type",
            };

            var createdPlatform = await _platformRepository.CreatePlatformAsync(platform);

            Assert.NotNull(createdPlatform);
            Assert.Equal(platform.Id, createdPlatform.Id);
            Assert.Equal("Test Platform Type", createdPlatform.Type);

            var platformFromDb = await _context.Platforms.FindAsync(platform.Id);
            Assert.NotNull(platformFromDb);
            Assert.Equal(platform.Id, platformFromDb.Id);
            Assert.Equal("Test Platform Type", platformFromDb.Type);
        }

        [Fact]
        public async Task CreatePlatformAsync_WhenPlatformIsNull_ShouldNotCreatePlatform()
        {
            Platform platform = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _platformRepository.CreatePlatformAsync(platform);
            });

            var platformFromDb = await _context.Platforms.FindAsync(platform?.Id);
            Assert.Null(platformFromDb);
        }

        [Fact]
        public async Task GetPlatformByIdAsync_WhenPlatformWithIdExistsInDatabase_ReturnsMatchingPlatform()
        {
            var expectedPlatformId = Guid.NewGuid();
            var expectedPlatform = new Platform
            {
                Id = expectedPlatformId,
                Type = "Console",
            };

            _context.Platforms.Add(expectedPlatform);
            await _context.SaveChangesAsync();

            var retrievedPlatform = await _platformRepository.GetPlatformByIdAsync(expectedPlatformId);

            Assert.NotNull(retrievedPlatform);
            Assert.Equal(expectedPlatform.Id, retrievedPlatform.Id);
            Assert.Equal(expectedPlatform.Type, retrievedPlatform.Type);
        }

        [Fact]
        public async Task GetAllPlatformsAsync_NoPlatformsFound_ReturnEmptyList()
        {

            var retrievedPlatforms = await _platformRepository.GetAllPlatformsAsync();

            Assert.NotNull(retrievedPlatforms);
            Assert.Empty(retrievedPlatforms);
        }

        [Fact]
        public async Task GetAllPlatformsAsync_ExistThreePlatforms_ReturnAllPlatforms()
        {
            var expectedPlatforms = new List<Platform>
        {
            new Platform { Id = Guid.NewGuid(), Type = "Console" },
            new Platform { Id = Guid.NewGuid(), Type = "PC" },
            new Platform { Id = Guid.NewGuid(), Type = "Mobile" },
        };

            _context.Platforms.AddRange(expectedPlatforms);
            await _context.SaveChangesAsync();

            var retrievedPlatforms = await _platformRepository.GetAllPlatformsAsync();

            Assert.NotNull(retrievedPlatforms);
            Assert.Equal(expectedPlatforms.Count, retrievedPlatforms.Count);

            for (int i = 0; i < expectedPlatforms.Count; i++)
            {
                Assert.Equal(expectedPlatforms[i].Id, retrievedPlatforms[i].Id);
                Assert.Equal(expectedPlatforms[i].Type, retrievedPlatforms[i].Type);
            }
        }

        [Fact]
        public async Task UpdatePlatformAsync_UpdateExistingPlatform_UpdatePlatformSuccessfully()
        {
            var platformToUpdate = new Platform
            {
                Id = Guid.NewGuid(),
                Type = "Console",
            };

            _context.Platforms.Add(platformToUpdate);
            await _context.SaveChangesAsync();

            platformToUpdate.Type = "Updated Console";

            await _platformRepository.UpdatePlatformAsync(platformToUpdate);

            var updatedPlatform = await _context.Platforms.FindAsync(platformToUpdate.Id);

            Assert.NotNull(updatedPlatform);
            Assert.Equal("Updated Console", updatedPlatform.Type);
        }

        [Fact]
        public async Task UpdatePlatformAsync_ShouldThrowException_WhenPlatformNotFound()
        {
            var nonExistingPlatform = new Platform { Id = Guid.NewGuid(), Type = "Non-existing Platform" };

            var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await _platformRepository.UpdatePlatformAsync(nonExistingPlatform);
            });

            Assert.Contains("does not exist in the store", exception.Message);
        }

        [Fact]
        public async Task UpdatePlatformAsync_WhenUnableToUpdatePlatform_ThrowException_()
        {
            var platformToUpdate = new Platform
            {
                Id = Guid.NewGuid(),
                Type = "Console",
            };

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _platformRepository.UpdatePlatformAsync(platformToUpdate));
        }

        [Fact]
        public async Task DeletePlatformAsync_DeletePlatform_PlatformDeletedSuccessfully()
        {
            var platformToDelete = new Platform
            {
                Id = Guid.NewGuid(),
                Type = "Console",
            };

            _context.Platforms.Add(platformToDelete);
            await _context.SaveChangesAsync();

            await _platformRepository.DeleteplatformAsync(platformToDelete);

            var deletedPlatform = await _context.Platforms.FindAsync(platformToDelete.Id);

            Assert.Null(deletedPlatform);
        }

        [Fact]
        public async Task DeletePlatformAsync_WhenPlatformExists_RemovesPlatformFromDatabase()
        {
            var platformToDelete = new Platform { Id = Guid.NewGuid(), Type = "PlatformToDelete" };
            await _context.Platforms.AddAsync(platformToDelete);
            await _context.SaveChangesAsync();

            await _platformRepository.DeleteplatformAsync(platformToDelete);

            var deletedPlatform = await _context.Platforms.FindAsync(platformToDelete.Id);
            Assert.Null(deletedPlatform);
        }

        [Fact]
        public async Task DeletePlatformAsync_WhenUnableToDeletePlatform_ShouldThrowException()
        {
            var platformToDelete = new Platform
            {
                Id = Guid.NewGuid(),
                Type = "Console",
            };

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _platformRepository.DeleteplatformAsync(platformToDelete));
        }
    }
}
