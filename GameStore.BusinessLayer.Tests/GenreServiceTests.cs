// <copyright file="GenreServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Services;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Moq;

    public class GenreServiceTests
    {
        private readonly Mock<IGenreRepository> genreRepositoryMock;
        private readonly GenreService genreService;

        public GenreServiceTests()
        {
            this.genreRepositoryMock = new Mock<IGenreRepository>();
            this.genreService = new GenreService(genreRepositoryMock.Object);
        }

        [Fact]
        public async Task AddGenreAsync_ValidGenreDto_ReturnsAddedGenre()
        {
            var genreDto = new CreateGenreRequest { Genre = new GenreDto { Name = "Valid Name", ParentGenreId = null } };
            var addedGenre = new Genre { Id = Guid.NewGuid(), Name = "Valid Name", ParentGenreId = null };

            this.genreRepositoryMock.Setup(repo => repo.AddGenreAsync(It.IsAny<Genre>())).ReturnsAsync(addedGenre);

            var result = await this.genreService.AddGenreAsync(genreDto);

            Assert.NotNull(result);
            Assert.Equal(addedGenre.Id, result.Id);
            Assert.Equal(addedGenre.Name, result.Name);
            Assert.Equal(addedGenre.ParentGenreId, result.ParentGenreId);

            this.genreRepositoryMock.Verify(repo => repo.AddGenreAsync(It.IsAny<Genre>()), Times.Once);
        }

        [Fact]
        public async Task AddGenreAsync_WhenParentGenreDoesNotExist_ShouldThrowException()
        {
            var parentGenreId = Guid.NewGuid();
            var genreDto = new CreateGenreRequest { Genre = new GenreDto { Name = "Valid Name", ParentGenreId = parentGenreId } };

            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(parentGenreId)).ReturnsAsync((Genre)null);

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => this.genreService.AddGenreAsync(genreDto));

            Assert.Equal("ParentGenreId must reference an existing genre.", exception.Message);
        }

        [Fact]
        public async Task AddGenreAsync_InvalidGenreName_ThrowsException()
        {
            var genreDto = new CreateGenreRequest
            {
                Genre = new GenreDto
                {
                    Name = null,
                    ParentGenreId = Guid.NewGuid(),
                },
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await this.genreService.AddGenreAsync(genreDto));
            this.genreRepositoryMock.Verify(repo => repo.AddGenreAsync(It.IsAny<Genre>()), Times.Never);
        }

        [Fact]
        public async Task GetGenreByIdAsync_ExistingGenre_ReturnsGetGenreResponse()
        {
            var genreId = Guid.NewGuid().ToString();
            var expectedGenre = new Genre
            {
                Id = Guid.Parse(genreId),
                Name = "Action",
                ParentGenreId = null,
            };

            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(It.IsAny<Guid>()))
                                .ReturnsAsync(expectedGenre);

            var result = await this.genreService.GetGenreByIdAsync(genreId);

            Assert.NotNull(result);
            Assert.Equal(expectedGenre.Id, result.Id);
            Assert.Equal(expectedGenre.Name, result.Name);
            Assert.Equal(expectedGenre.ParentGenreId, result.ParentGenreId);
            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetGenreByIdAsync_NonExistingGenre_ThrowsNotFoundException()
        {
            var nonExistingGenreId = Guid.NewGuid().ToString();

            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(Guid.Parse(nonExistingGenreId)))
                                .ReturnsAsync((Genre?)null);

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.genreService.GetGenreByIdAsync(nonExistingGenreId));

            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(Guid.Parse(nonExistingGenreId)));
        }

        [Fact]
        public async Task GetAllGenresAsync_ExistThreeGenre_ReturnsAllThreeGenres()
        {
            var expectedGenres = new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Action" },
            new Genre { Id = Guid.NewGuid(), Name = "Adventure" },
            new Genre { Id = Guid.NewGuid(), Name = "Strategy" },
        };

            this.genreRepositoryMock.Setup(repo => repo.GetAllGenresAsync())
                              .ReturnsAsync(expectedGenres);

            var result = await this.genreService.GetAllGenresAsync();

            Assert.NotNull(result);

            var genreResponseDtos = result.ToList();
            Assert.Equal(expectedGenres.Count, genreResponseDtos.Count);

            for (int i = 0; i < expectedGenres.Count; i++)
            {
                Assert.Equal(expectedGenres[i].Id, genreResponseDtos[i].Id);
                Assert.Equal(expectedGenres[i].Name, genreResponseDtos[i].Name);
            }

            this.genreRepositoryMock.Verify(repo => repo.GetAllGenresAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllGenresAsync_NoGenresExist_ReturnsEmptyCollection()
        {
            var emptyGenres = new List<Genre>();
            this.genreRepositoryMock.Setup(repo => repo.GetAllGenresAsync())
                              .ReturnsAsync(emptyGenres);

            var result = await this.genreService.GetAllGenresAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
            this.genreRepositoryMock.Verify(repo => repo.GetAllGenresAsync(), Times.Once);
        }

        [Fact]
        public async Task GetgenresByParentIdAsync_ExistingGenres_ReturnsGenresByParentId()
        {
            var parentId = Guid.NewGuid();
            var expectedGenres = new List<Genre>
    {
        new Genre { Id = Guid.NewGuid(), Name = "Action", ParentGenreId = parentId },
        new Genre { Id = Guid.NewGuid(), Name = "Adventure", ParentGenreId = parentId },
    };

            this.genreRepositoryMock.Setup(repo => repo.GetgenresByParentIdAsync(parentId))
                                .ReturnsAsync(expectedGenres);

            var result = await this.genreService.GetgenresByParentIdAsync(parentId);

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

            Assert.Equal(expectedGenres.Count, result.Count);

            this.genreRepositoryMock.Verify(repo => repo.GetgenresByParentIdAsync(parentId), Times.Once);
        }

        [Fact]
        public async Task GetgenresByParentIdAsync_NoGenresExist_ThrowsNotFoundException()
        {
            var nonExistingParentId = Guid.NewGuid();

            this.genreRepositoryMock.Setup(repo => repo.GetgenresByParentIdAsync(nonExistingParentId))
                              .ReturnsAsync(new List<Genre>());

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.genreService.GetgenresByParentIdAsync(nonExistingParentId));
            this.genreRepositoryMock.Verify(repo => repo.GetgenresByParentIdAsync(nonExistingParentId), Times.Once);
        }

        [Fact]
        public async Task UpdateGenreAsync_ExistingGenrebyId_ReturnsUpdatedGenreDto()
        {
            var genreId = Guid.NewGuid();
            var genreDto = new GenreUpdateDto { Id = genreId, Name = "Updated Genre", ParentGenreId = null };
            var existingGenre = new Genre { Id = genreId, Name = "Existing Genre", ParentGenreId = null };

            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(genreId))
                              .ReturnsAsync(existingGenre);

            var result = await this.genreService.UpdateGenreAsync(genreDto);

            Assert.NotNull(result);
            Assert.Equal(genreDto.Id, result.Id);
            Assert.Equal(genreDto.Name, result.Name);
            Assert.Equal(genreDto.ParentGenreId, result.ParentGenreId);
            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(genreId), Times.Once);
        }

        [Fact]
        public async Task UpdateGenreAsync_NonExistingGenre_ThrowsNotFoundException()
        {
            var nonExistingGenreId = Guid.NewGuid();
            var genreDto = new GenreUpdateDto { Id = nonExistingGenreId, Name = "Updated Genre", ParentGenreId = null };

            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(nonExistingGenreId))
                              .ReturnsAsync((Genre?)null);

            await Assert.ThrowsAsync<NotFoundException>(async () => await this.genreService.UpdateGenreAsync(genreDto));
            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(nonExistingGenreId), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_ExistingGenre_DeletesGenreAndReturnsDeletedGenreId()
        {
            var genreId = Guid.NewGuid();
            var existingGenre = new Genre { Id = genreId, Name = "Existing Genre" };

            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(genreId))
                              .ReturnsAsync(existingGenre);

            var result = await this.genreService.DeleteGenreAsync(genreId);

            Assert.NotNull(result);
            Assert.Equal(genreId, result.Id);
            this.genreRepositoryMock.Verify(repo => repo.DeleteGenreAsync(existingGenre), Times.Once);
        }

        [Fact]
        public async Task DeleteGenreAsync_NonExistingGenre_ThrowsNotFoundException()
        {
            var nonExistingGenreId = Guid.NewGuid();

            this.genreRepositoryMock.Setup(repo => repo.GetGenreByIdAsync(nonExistingGenreId))
                              .ReturnsAsync((Genre?)null);
            await Assert.ThrowsAsync<NotFoundException>(async () => await this.genreService.DeleteGenreAsync(nonExistingGenreId));
            this.genreRepositoryMock.Verify(repo => repo.GetGenreByIdAsync(nonExistingGenreId), Times.Once);
        }
    }
}
