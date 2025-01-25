// <copyright file="PlatformServiceTests.cs" company="PlaceholderCompany">
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

    public class PlatformServiceTests
    {
        private readonly Mock<IPlatformRepository> _platformRepositoryMock;
        private readonly PlatformService _platformService;

        public PlatformServiceTests()
        {
            _platformRepositoryMock = new Mock<IPlatformRepository>();
            _platformService = new PlatformService(_platformRepositoryMock.Object);
        }

        [Fact]
        public async Task CreatePlatformAsync_ValidPlatform_ReturnsCreatedPlatform()
        {
            var platformId = Guid.NewGuid();
            var platformType = "PlayStation";

            var request = new CreatePlatformRequest
            {
                Platform = new PlatformDto { Type = platformType },
            };

            var expectedPlatform = new Platform
            {
                Id = platformId,
                Type = platformType,
            };

            _platformRepositoryMock.Setup(repo => repo.CreatePlatformAsync(It.IsAny<Platform>()))
                                  .ReturnsAsync(expectedPlatform);

            var result = await _platformService.CreatePlatformAsync(request);

            Assert.NotNull(result);
            Assert.Equal(expectedPlatform.Id, result.Id);
            Assert.Equal(expectedPlatform.Type, result.Type);
            _platformRepositoryMock.Verify(repo => repo.CreatePlatformAsync(It.IsAny<Platform>()), Times.Once);
        }

        [Fact]
        public async Task CreatePlatformAsync_UnableToCreatePlatform_ThrowsException()
        {
            var platformType = "PlayStation";

            var request = new CreatePlatformRequest
            {
                Platform = new PlatformDto { Type = platformType },
            };

            _platformRepositoryMock.Setup(repo => repo.CreatePlatformAsync(It.IsAny<Platform>()))
                                  .ThrowsAsync(new Exception("Unable to create platform"));

            await Assert.ThrowsAsync<Exception>(async () => await _platformService.CreatePlatformAsync(request));
            _platformRepositoryMock.Verify(repo => repo.CreatePlatformAsync(It.IsAny<Platform>()), Times.Once);
        }

        [Fact]
        public async Task GetPlatformByIdAsync_ExistingPlatform_ReturnsPlatformResponseModel()
        {
            var platformId = Guid.NewGuid();
            var platformType = "PlayStation";

            var expectedPlatform = new Platform
            {
                Id = platformId,
                Type = platformType,
            };

            _platformRepositoryMock.Setup(repo => repo.GetPlatformByIdAsync(platformId))
                                  .ReturnsAsync(expectedPlatform);

            var result = await _platformService.GetPlatformByIdAsync(platformId);

            Assert.NotNull(result);
            Assert.Equal(expectedPlatform.Id, result.Id);
            Assert.Equal(expectedPlatform.Type, result.Type);
        }

        [Fact]
        public async Task GetPlatformByIdAsync_NonExistingPlatform_ThrowsNotFoundException()
        {
            var nonExistingPlatformId = Guid.NewGuid();

            _platformRepositoryMock.Setup(repo => repo.GetPlatformByIdAsync(nonExistingPlatformId))
                                  .ReturnsAsync((Platform)null);

            await Assert.ThrowsAsync<NotFoundException>(async () => await _platformService.GetPlatformByIdAsync(nonExistingPlatformId));
            _platformRepositoryMock.Verify(repo => repo.GetPlatformByIdAsync(nonExistingPlatformId), Times.Once);
        }

        [Fact]
        public async Task GetAllPlatformsAsync_ReturnsListOfPlatformResponseModel()
        {
            var platforms = new List<Platform>
    {
        new Platform { Id = Guid.NewGuid(), Type = "PlayStation" },
        new Platform { Id = Guid.NewGuid(), Type = "Xbox" },
        new Platform { Id = Guid.NewGuid(), Type = "Nintendo Switch" },
    };

            _platformRepositoryMock.Setup(repo => repo.GetAllPlatformsAsync())
                                   .ReturnsAsync(platforms);

            var result = await _platformService.GetAllPlatformsAsync();

            Assert.Collection(result,
                item =>
                {
                    Assert.Equal(platforms[0].Id, item.Id);
                    Assert.Equal(platforms[0].Type, item.Type);
                },
                item =>
                {
                    Assert.Equal(platforms[1].Id, item.Id);
                    Assert.Equal(platforms[1].Type, item.Type);
                },
                item =>
                {
                    Assert.Equal(platforms[2].Id, item.Id);
                    Assert.Equal(platforms[2].Type, item.Type);
                }
            );

            Assert.Equal(platforms.Count, result.Count);

            _platformRepositoryMock.Verify(repo => repo.GetAllPlatformsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllPlatformsAsync_NoPlatformsFound_ReturnsEmptyList()
        {
            _platformRepositoryMock.Setup(repo => repo.GetAllPlatformsAsync())
                                  .ReturnsAsync(new List<Platform>());

            var result = await _platformService.GetAllPlatformsAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
            _platformRepositoryMock.Verify(repo => repo.GetAllPlatformsAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdatePlatformAsync_ExistingPlatform_ReturnsUpdatedPlatform()
        {
            var platformId = Guid.NewGuid();
            var updatedType = "Updated Type";

            var updateDto = new UpdatePlatformDto
            {
                Platform = new PlatformUpdateDto { Id = platformId, Type = updatedType },
            };

            var existingPlatform = new Platform { Id = platformId, Type = "Existing Type" };

            _platformRepositoryMock.Setup(repo => repo.GetPlatformByIdAsync(platformId))
                                  .ReturnsAsync(existingPlatform);

            var result = await _platformService.UpdatePlatformAsync(updateDto);

            Assert.NotNull(result);
            Assert.Equal(platformId, result.Id);
            Assert.Equal(updatedType, result.Type);
            _platformRepositoryMock.Verify(repo => repo.UpdatePlatformAsync(existingPlatform), Times.Once);
        }

        [Fact]
        public async Task UpdatePlatformAsync_NonExistingPlatform_ThrowsNotFoundException()
        {
            var nonExistingPlatformId = Guid.NewGuid();
            var updateDto = new UpdatePlatformDto
            {
                Platform = new PlatformUpdateDto { Id = nonExistingPlatformId, Type = "Updated Type" },
            };

            _platformRepositoryMock.Setup(repo => repo.GetPlatformByIdAsync(nonExistingPlatformId))
                                  .ReturnsAsync((Platform)null);

            await Assert.ThrowsAsync<NotFoundException>(async () => await _platformService.UpdatePlatformAsync(updateDto));
            _platformRepositoryMock.Verify(repo => repo.GetPlatformByIdAsync(nonExistingPlatformId), Times.Once);
        }

        [Fact]
        public async Task DeletePlatformAsync_ExistingPlatform_DeletesPlatform()
        {
            var platformId = Guid.NewGuid();

            var existingPlatform = new Platform { Id = platformId, Type = "Existing Type" };

            _platformRepositoryMock.Setup(repo => repo.GetPlatformByIdAsync(platformId))
                                  .ReturnsAsync(existingPlatform);

            await _platformService.DeletePlatformAsync(platformId);

            _platformRepositoryMock.Verify(repo => repo.DeleteplatformAsync(existingPlatform), Times.Once);
        }

        [Fact]
        public async Task DeletePlatformAsync_NonExistingPlatform_ThrowsNotFoundException()
        {
            var nonExistingPlatformId = Guid.NewGuid();

            _platformRepositoryMock.Setup(repo => repo.GetPlatformByIdAsync(nonExistingPlatformId))
                                  .ReturnsAsync((Platform)null);

            await Assert.ThrowsAsync<NotFoundException>(async () => await _platformService.DeletePlatformAsync(nonExistingPlatformId));
            _platformRepositoryMock.Verify(repo => repo.GetPlatformByIdAsync(nonExistingPlatformId), Times.Once);
        }
    }
}
