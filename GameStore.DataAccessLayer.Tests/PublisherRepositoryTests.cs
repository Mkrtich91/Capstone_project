namespace GameStore.DataAccessLayer.Tests
{
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class PublisherRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public PublisherRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{nameof(PublisherRepositoryTests)}_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddPublisherAsync_ValidPublisher_ReturnAddedPublisher()
        {
            var publisher = new Publisher
            {
                Id = Guid.NewGuid(),
                CompanyName = "Test Publisher",
                HomePage = "https://test.com",
                Description = "Test description",
            };

            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var addedPublisher = await repository.AddPublisherAsync(publisher);

                Assert.NotNull(addedPublisher);
                Assert.Equal(publisher.Id, addedPublisher.Id);
                Assert.Equal(publisher.CompanyName, addedPublisher.CompanyName);
                Assert.Equal(publisher.HomePage, addedPublisher.HomePage);
                Assert.Equal(publisher.Description, addedPublisher.Description);

                var retrievedPublisher = await context.Publishers.FirstOrDefaultAsync(p => p.Id == publisher.Id);
                Assert.NotNull(retrievedPublisher);
                Assert.Equal(publisher.CompanyName, retrievedPublisher.CompanyName);
            }
        }

        [Fact]
        public async Task AddPublisherAsync_InvalidPublisher_ThrowsException()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var invalidPublisher = new Publisher
                {
                    Id = Guid.NewGuid(),
                    CompanyName = null,
                    HomePage = "https://test.com",
                    Description = "Test description",
                };

                await Assert.ThrowsAsync<DbUpdateException>(async () =>
                {
                    await repository.AddPublisherAsync(invalidPublisher);
                    await context.SaveChangesAsync();
                });
            }
        }

        [Fact]
        public async Task GetPublisherByIdAsync_ExistingId_ReturnPublisher()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var publisher = new Publisher
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Test Publisher",
                    HomePage = "https://test.com",
                    Description = "Test description",
                };

                await context.Publishers.AddAsync(publisher);
                await context.SaveChangesAsync();

                var retrievedPublisher = await repository.GetPublisherByIdAsync(publisher.Id);

                Assert.NotNull(retrievedPublisher);
                Assert.Equal(publisher.Id, retrievedPublisher.Id);
                Assert.Equal(publisher.CompanyName, retrievedPublisher.CompanyName);
                Assert.Equal(publisher.HomePage, retrievedPublisher.HomePage);
                Assert.Equal(publisher.Description, retrievedPublisher.Description);
            }
        }

        [Fact]
        public async Task GetPublisherByIdAsync_NonExistingId_ReturnNull()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var publisher = new Publisher
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Test Publisher",
                    HomePage = "https://test.com",
                    Description = "Test description",
                };

                await context.Publishers.AddAsync(publisher);
                await context.SaveChangesAsync();

                var nonExistingId = Guid.NewGuid();

                var retrievedPublisher = await repository.GetPublisherByIdAsync(nonExistingId);

                Assert.Null(retrievedPublisher);
            }
        }

        [Fact]
        public async Task GetPublisherByNameAsync_ExistingName_ReturnPublisher()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var publisher = new Publisher
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Test Publisher",
                    HomePage = "https://test.com",
                    Description = "Test description",
                };

                await context.Publishers.AddAsync(publisher);
                await context.SaveChangesAsync();

                var retrievedPublisher = await repository.GetPublisherByNameAsync(publisher.CompanyName);

                Assert.NotNull(retrievedPublisher);
                Assert.Equal(publisher.Id, retrievedPublisher.Id);
                Assert.Equal(publisher.CompanyName, retrievedPublisher.CompanyName);
                Assert.Equal(publisher.HomePage, retrievedPublisher.HomePage);
                Assert.Equal(publisher.Description, retrievedPublisher.Description);
            }
        }

        [Fact]
        public async Task GetPublisherByNameAsync_NonExistingName_ReturnNull()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var publisher = new Publisher
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Test Publisher",
                    HomePage = "https://test.com",
                    Description = "Test description"
                };

                await context.Publishers.AddAsync(publisher);
                await context.SaveChangesAsync();

                var nonExistingName = "Non-existing Publisher";

                var retrievedPublisher = await repository.GetPublisherByNameAsync(nonExistingName);

                Assert.Null(retrievedPublisher);
            }
        }

        [Fact]
        public async Task GetAllPublishersAsync_ReturnAllPublishers()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var publisher1 = new Publisher
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Publisher 1",
                    HomePage = "https://publisher1.com",
                    Description = "Publisher 1 description",
                };

                var publisher2 = new Publisher
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Publisher 2",
                    HomePage = "https://publisher2.com",
                    Description = "Publisher 2 description",
                };

                await context.Publishers.AddRangeAsync(publisher1, publisher2);
                await context.SaveChangesAsync();

                var allPublishers = await repository.GetAllPublishersAsync();

                Assert.NotNull(allPublishers);
                Assert.Equal(2, allPublishers.Count());

                var retrievedPublisher1 = allPublishers.FirstOrDefault(p => p.Id == publisher1.Id);
                Assert.NotNull(retrievedPublisher1);
                Assert.Equal(publisher1.CompanyName, retrievedPublisher1.CompanyName);
                Assert.Equal(publisher1.HomePage, retrievedPublisher1.HomePage);
                Assert.Equal(publisher1.Description, retrievedPublisher1.Description);

                var retrievedPublisher2 = allPublishers.FirstOrDefault(p => p.Id == publisher2.Id);
                Assert.NotNull(retrievedPublisher2);
                Assert.Equal(publisher2.CompanyName, retrievedPublisher2.CompanyName);
                Assert.Equal(publisher2.HomePage, retrievedPublisher2.HomePage);
                Assert.Equal(publisher2.Description, retrievedPublisher2.Description);
            }
        }

        [Fact]
        public async Task UpdatePublisherAsync_ValidPublisher_ReturnUpdatedPublisher()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var publisher = new Publisher
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Test Publisher",
                    HomePage = "https://test.com",
                    Description = "Test description",
                };

                await context.Publishers.AddAsync(publisher);
                await context.SaveChangesAsync();

                var updatedPublisher = new Publisher
                {
                    Id = publisher.Id,
                    CompanyName = "Updated Publisher",
                    HomePage = "https://updated.com",
                    Description = "Updated description",
                };

                context.Entry(publisher).State = EntityState.Detached;
                var result = await repository.UpdatePublisherAsync(updatedPublisher);

                Assert.NotNull(result);
                Assert.Equal(updatedPublisher.Id, result.Id);
                Assert.Equal(updatedPublisher.CompanyName, result.CompanyName);
                Assert.Equal(updatedPublisher.HomePage, result.HomePage);
                Assert.Equal(updatedPublisher.Description, result.Description);

                var retrievedPublisher = await context.Publishers.FindAsync(updatedPublisher.Id);
                Assert.NotNull(retrievedPublisher);
                Assert.Equal(updatedPublisher.CompanyName, retrievedPublisher.CompanyName);
                Assert.Equal(updatedPublisher.HomePage, retrievedPublisher.HomePage);
                Assert.Equal(updatedPublisher.Description, retrievedPublisher.Description);
            }
        }

        [Fact]
        public async Task UpdatePublisherAsync_InvalidPublisher_ThrowsDbUpdateException()
        {
            var publisherId = Guid.NewGuid();
            var invalidPublisher = new Publisher
            {
                Id = publisherId,
                CompanyName = "", // Invalid CompanyName
                HomePage = "https://test.com",
                Description = "Test description",
            };

            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
                {
                    await repository.UpdatePublisherAsync(invalidPublisher);
                });

                Assert.NotNull(exception);
            }
        }

        [Fact]
        public async Task DeletePublisherAsync_PublisherIdExists_DeletesPublisherById()
        {
            var publisherId = Guid.NewGuid();
            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);
                var publisher = new Publisher
                {
                    Id = publisherId,
                    CompanyName = "Test Publisher",
                    HomePage = "https://test.com",
                    Description = "Test description",
                };
                await context.Publishers.AddAsync(publisher);
                await context.SaveChangesAsync();

                await repository.DeletePublisherAsync(publisherId);

                var deletedPublisher = await context.Publishers.FindAsync(publisherId);
                Assert.Null(deletedPublisher);
            }
        }

        [Fact]
        public async Task DeletePublisherAsync_PublisherByIdDoesNotExist_NoExceptionThrown()
        {
            var nonExistentPublisherId = Guid.NewGuid();
            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                await repository.DeletePublisherAsync(nonExistentPublisherId);
            }
        }

        [Fact]
        public async Task GetPublisherByGameKeyAsync_GameKeyExists_ReturnsPublisher()
        {
            var key = "existing_key";
            var publisherId = Guid.NewGuid();
            var expectedPublisher = new Publisher
            {
                Id = publisherId,
                CompanyName = "Test Publisher",
                HomePage = "https://test.com",
                Description = "Test description",
            };

            using (var context = new DataContext(_options))
            {
                var game = new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Game",
                    Key = key,
                    Description = "Test description",
                    PublisherId = publisherId,
                };
                context.Games.Add(game);

                context.Publishers.Add(expectedPublisher);
                await context.SaveChangesAsync();

                var repository = new PublisherRepository(context);

                var result = await repository.GetPublisherByGameKeyAsync(key);

                Assert.NotNull(result);
                Assert.Equal(expectedPublisher.Id, result.Id);
                Assert.Equal(expectedPublisher.CompanyName, result.CompanyName);
                Assert.Equal(expectedPublisher.HomePage, result.HomePage);
                Assert.Equal(expectedPublisher.Description, result.Description);
            }
        }

        [Fact]
        public async Task GetPublisherByGameKeyAsync_GameKeyNotFound_ReturnsNull()
        {
            var key = "non_existing_key";

            using (var context = new DataContext(_options))
            {
                var repository = new PublisherRepository(context);

                var result = await repository.GetPublisherByGameKeyAsync(key);

                Assert.Null(result);
            }
        }
    }
}
