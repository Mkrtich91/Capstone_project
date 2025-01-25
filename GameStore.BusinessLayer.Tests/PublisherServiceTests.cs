// <copyright file="PublisherServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Tests
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Services;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Moq;

    public class PublisherServiceTests
    {
        private readonly Mock<IPublisherRepository> _publisherRepositoryMock;
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly PublisherService _publisherService;

        public PublisherServiceTests()
        {
            _publisherRepositoryMock = new Mock<IPublisherRepository>();
            _gameRepositoryMock = new Mock<IGameRepository>();
            _publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);
        }

        [Fact]
        public async Task AddPublisherAsync_ValidPublisherRequest_ShouldReturnAddedPublisher()
        {
            var publisherRequest = new PublisherRequest
            {
                Publisher = new PublisherDto
                {
                    CompanyName = "Test Company",
                    HomePage = "http://test.com",
                    Description = "Test Description",
                },
            };

            var expectedPublisher = new Publisher
            {
                Id = Guid.NewGuid(),
                CompanyName = publisherRequest.Publisher.CompanyName,
                HomePage = publisherRequest.Publisher.HomePage,
                Description = publisherRequest.Publisher.Description,
            };

            _publisherRepositoryMock
                .Setup(repo => repo.AddPublisherAsync(It.IsAny<Publisher>()))
                .ReturnsAsync((Publisher pub) => pub);

            var result = await _publisherService.AddPublisherAsync(publisherRequest);

            Assert.NotNull(result);
            Assert.Equal(expectedPublisher.CompanyName, result.CompanyName);
            Assert.Equal(expectedPublisher.HomePage, result.HomePage);
            Assert.Equal(expectedPublisher.Description, result.Description);

            _publisherRepositoryMock.Verify(repo => repo.AddPublisherAsync(It.IsAny<Publisher>()), Times.Once);
        }

        [Fact]
        public async Task AddPublisherAsync_InvalidPublisherRequest_ShouldThrowArgumentException()
        {
            var publisherRequest = new PublisherRequest
            {
                Publisher = new PublisherDto
                {
                    CompanyName = string.Empty,
                    HomePage = "http://test.com",
                    Description = "Test Description",
                },
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _publisherService.AddPublisherAsync(publisherRequest));

            _publisherRepositoryMock.Verify(repo => repo.AddPublisherAsync(It.IsAny<Publisher>()), Times.Never);
        }

        [Fact]
        public async Task AddPublisherAsync_NullCompanyName_ShouldThrowArgumentException()
        {
            var publisherRequest = new PublisherRequest
            {
                Publisher = new PublisherDto
                {
                    CompanyName = null,
                    HomePage = "http://test.com",
                    Description = "Test Description",
                },
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _publisherService.AddPublisherAsync(publisherRequest));

            _publisherRepositoryMock.Verify(repo => repo.AddPublisherAsync(It.IsAny<Publisher>()), Times.Never);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_ValidId_ReturnsPublisher()
        {
            var publisherId = Guid.NewGuid().ToString();
            var publisher = new Publisher
            {
                Id = Guid.Parse(publisherId),
                CompanyName = "Test Company",
                HomePage = "http://test.com",
                Description = "Test Description",
            };

            _publisherRepositoryMock
                .Setup(repo => repo.GetPublisherByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(publisher);

            var result = await _publisherService.GetPublisherByIdAsync(publisherId);

            Assert.NotNull(result);
            Assert.Equal(publisher.Id, result.Id);
            Assert.Equal(publisher.CompanyName, result.CompanyName);
            Assert.Equal(publisher.HomePage, result.HomePage);
            Assert.Equal(publisher.Description, result.Description);

            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_InvalidId_ThrowsNotFoundException()
        {
            var publisherId = Guid.NewGuid().ToString();
            _ = _publisherRepositoryMock
                .Setup(repo => repo.GetPublisherByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Publisher)null);
            await Assert.ThrowsAsync<NotFoundException>(() => _publisherService.GetPublisherByIdAsync(publisherId));

            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByNameAsync_ValidName_ReturnsPublisher()
        {
            var companyName = "Test Company";
            var publisher = new Publisher
            {
                Id = Guid.NewGuid(),
                CompanyName = companyName,
                HomePage = "http://test.com",
                Description = "Test Description",
            };

            _publisherRepositoryMock
                .Setup(repo => repo.GetPublisherByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(publisher);

            var result = await _publisherService.GetPublisherByNameAsync(companyName);

            Assert.NotNull(result);
            Assert.Equal(publisher.Id, result.Id);
            Assert.Equal(publisher.CompanyName, result.CompanyName);
            Assert.Equal(publisher.HomePage, result.HomePage);
            Assert.Equal(publisher.Description, result.Description);

            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByNameAsync_NonExistentName_ThrowsNotFoundException()
        {
            var companyName = "NonExistentCompany";

            _publisherRepositoryMock
                .Setup(repo => repo.GetPublisherByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((Publisher)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _publisherService.GetPublisherByNameAsync(companyName));
            Assert.Equal($"Publisher with name '{companyName}' not found.", exception.Message);

            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPublishersAsync_Existing3Publishers_ReturnsAllThreePublishers()
        {
            var publishers = new List<Publisher>
    {
        new Publisher { Id = Guid.NewGuid(), CompanyName = "Publisher 1", HomePage = "http://publisher1.com", Description = "Description 1" },
        new Publisher { Id = Guid.NewGuid(), CompanyName = "Publisher 2", HomePage = "http://publisher2.com", Description = "Description 2" },
        new Publisher { Id = Guid.NewGuid(), CompanyName = "Publisher 3", HomePage = "http://publisher3.com", Description = "Description 3" },
    };

            _publisherRepositoryMock.Setup(repo => repo.GetAllPublishersAsync()).ReturnsAsync(publishers);

            var result = await _publisherService.GetAllPublishersAsync();

            Assert.NotNull(result);
            Assert.Equal(publishers.Count, result.Count());

            foreach (var expectedResponse in result)
            {
                var matchingPublisher = publishers.FirstOrDefault(p => p.Id == expectedResponse.Id);
                Assert.NotNull(matchingPublisher);
                Assert.Equal(expectedResponse.CompanyName, matchingPublisher.CompanyName);
                Assert.Equal(expectedResponse.HomePage, matchingPublisher.HomePage);
                Assert.Equal(expectedResponse.Description, matchingPublisher.Description);
            }

            _publisherRepositoryMock.Verify(repo => repo.GetAllPublishersAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdatePublisherAsync_ValidRequest_ReturnsUpdatedPublisher()
        {
            var publisherId = Guid.NewGuid();
            var request = new UpdatePublisherRequest
            {
                Publisher = new PublisherUpdateDto
                {
                    Id = publisherId,
                    CompanyName = "Updated Company",
                    HomePage = "http://updatedhomepage.com",
                    Description = "Updated description",
                },
            };
            var publisher = new Publisher
            {
                Id = publisherId,
                CompanyName = "Original Company",
                HomePage = "http://originalhomepage.com",
                Description = "Original description",
            };

            _publisherRepositoryMock.Setup(repo => repo.GetPublisherByIdAsync(publisherId))
                                    .ReturnsAsync(publisher);

            var updatedPublisher = new Publisher
            {
                Id = publisherId,
                CompanyName = request.Publisher.CompanyName,
                HomePage = request.Publisher.HomePage,
                Description = request.Publisher.Description
            };
            _publisherRepositoryMock.Setup(repo => repo.UpdatePublisherAsync(It.IsAny<Publisher>()))
                                    .ReturnsAsync(updatedPublisher);

            var publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);

            var result = await publisherService.UpdatePublisherAsync(request);

            Assert.NotNull(result);
            Assert.Equal(request.Publisher.Id, result.Id);
            Assert.Equal(request.Publisher.CompanyName, result.CompanyName);
            Assert.Equal(request.Publisher.HomePage, result.HomePage);
            Assert.Equal(request.Publisher.Description, result.Description);

            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByIdAsync(publisherId), Times.Once);
            _publisherRepositoryMock.Verify(repo => repo.UpdatePublisherAsync(It.IsAny<Publisher>()), Times.Once);
        }

        [Theory]
        [InlineData("", "http://updatedhomepage.com", "Updated description")]
        [InlineData("Updated Company", "", "Updated description")]
        [InlineData("Updated Company", "http://updatedhomepage.com", "")]
        public async Task UpdatePublisherAsync_MissingFields_ThrowsArgumentException(string companyName, string homePage, string description)
        {
            var publisherId = Guid.NewGuid();
            var request = new UpdatePublisherRequest
            {
                Publisher = new PublisherUpdateDto
                {
                    Id = publisherId,
                    CompanyName = companyName,
                    HomePage = homePage,
                    Description = description,
                },
            };

            var publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);

            await Assert.ThrowsAsync<ArgumentException>(async () => await publisherService.UpdatePublisherAsync(request));
        }

        [Fact]
        public async Task UpdatePublisherAsync_NonExistentPublisher_ThrowsException()
        {
            var publisherId = Guid.NewGuid();
            var request = new UpdatePublisherRequest
            {
                Publisher = new PublisherUpdateDto
                {
                    Id = publisherId,
                    CompanyName = "Updated Company",
                    HomePage = "http://updatedhomepage.com",
                    Description = "Updated description",
                },
            };

            _publisherRepositoryMock.Setup(repo => repo.GetPublisherByIdAsync(publisherId))
                                    .ReturnsAsync((Publisher)null);

            var publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);

            await Assert.ThrowsAsync<Exception>(async () => await publisherService.UpdatePublisherAsync(request));
            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByIdAsync(publisherId), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByGameKeyAsync_ValidKey_ReturnsPublisher()
        {
            var key = "test_key";
            var publisherId = Guid.NewGuid();
            var publisher = new Publisher
            {
                Id = publisherId,
                CompanyName = "Test Publisher",
                Description = "Test Description",
                HomePage = "http://testhomepage.com",
            };

            _publisherRepositoryMock.Setup(repo => repo.GetPublisherByGameKeyAsync(key))
                                    .ReturnsAsync(publisher);

            var publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);

            var result = await publisherService.GetPublisherByGameKeyAsync(key);

            Assert.NotNull(result);
            Assert.Equal(publisher.Id, result.Id);
            Assert.Equal(publisher.CompanyName, result.CompanyName);
            Assert.Equal(publisher.Description, result.Description);
            Assert.Equal(publisher.HomePage, result.HomePage);

            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByGameKeyAsync(key), Times.Once);
        }

        [Fact]
        public async Task GetPublisherByGameKeyAsync_GameKeyNotFound_ThrowsNotFoundException()
        {
            var key = "non_existing_key";
            _publisherRepositoryMock.Setup(repo => repo.GetPublisherByGameKeyAsync(key))
                                    .ReturnsAsync((Publisher)null);

            var publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await publisherService.GetPublisherByGameKeyAsync(key));
            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByGameKeyAsync(key), Times.Once);
        }

        [Fact]
        public async Task DeletePublisherAsync_PublisherExists_DeletesPublisher()
        {
            var existingPublisherId = Guid.NewGuid();
            _publisherRepositoryMock.Setup(repo => repo.GetPublisherByIdAsync(existingPublisherId))
                                    .ReturnsAsync(new Publisher { Id = existingPublisherId });

            var publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);

            await publisherService.DeletePublisherAsync(existingPublisherId);

            _publisherRepositoryMock.Verify(repo => repo.DeletePublisherAsync(existingPublisherId), Times.Once);
        }

        [Fact]
        public async Task DeletePublisherAsync_PublisherDoesNotExist_ThrowsNotFoundException()
        {
            var nonExistentPublisherId = Guid.NewGuid();
            _publisherRepositoryMock.Setup(repo => repo.GetPublisherByIdAsync(nonExistentPublisherId))
                                    .ReturnsAsync((Publisher)null);

            var publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await publisherService.DeletePublisherAsync(nonExistentPublisherId));
            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByIdAsync(nonExistentPublisherId), Times.Once);
            _publisherRepositoryMock.Verify(repo => repo.DeletePublisherAsync(nonExistentPublisherId), Times.Never);
        }

        [Fact]
        public async Task GetGamesByPublisherNameAsync_PublisherExists_ReturnsGames()
        {
            var companyName = "PublisherName";
            var publisherId = Guid.NewGuid();
            var games = new List<Game>
    {
        new Game { Id = Guid.NewGuid(), Name = "Game 1", Description = "Description 1", Key = "Key1", Price = 49.99, Discount = 10, UnitInStock = 100, PublisherId = publisherId },
        new Game { Id = Guid.NewGuid(), Name = "Game 2", Description = "Description 2", Key = "Key2", Price = 59.99, Discount = 15, UnitInStock = 150, PublisherId = publisherId },
    };

            _publisherRepositoryMock.Setup(repo => repo.GetPublisherByNameAsync(companyName))
                                    .ReturnsAsync(new Publisher { Id = publisherId, CompanyName = companyName });

            _gameRepositoryMock.Setup(repo => repo.GetGamesByPublisherIdAsync(publisherId))
                               .ReturnsAsync(games);

            var publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);

            var result = await publisherService.GetGamesByPublisherNameAsync(companyName);

            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByNameAsync(companyName), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.GetGamesByPublisherIdAsync(publisherId), Times.Once);
            Assert.Equal(games.Count, result.Count);
            Assert.All(result, game => Assert.Contains(games, g => g.Id == game.Id));
        }

        [Fact]
        public async Task GetGamesByPublisherNameAsync_PublisherDoesNotExist_ThrowsNotFoundException()
        {
            var companyName = "NonExistentPublisher";

            _publisherRepositoryMock.Setup(repo => repo.GetPublisherByNameAsync(companyName))
                                    .ReturnsAsync((Publisher)null);

            var publisherService = new PublisherService(_publisherRepositoryMock.Object, _gameRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await publisherService.GetGamesByPublisherNameAsync(companyName));

            _publisherRepositoryMock.Verify(repo => repo.GetPublisherByNameAsync(companyName), Times.Once);
            _gameRepositoryMock.Verify(repo => repo.GetGamesByPublisherIdAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
