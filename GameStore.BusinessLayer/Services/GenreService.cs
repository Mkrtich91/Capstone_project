namespace GameStore.BusinessLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;

    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<Genre> AddGenreAsync(CreateGenreRequest genreDto)
        {
            ValidateGenreDto(genreDto);

            if (genreDto.Genre.ParentGenreId.HasValue)
            {
                await ValidateParentGenreAsync(genreDto.Genre.ParentGenreId.Value);
            }

            var genre = new Genre
            {
                Id = Guid.NewGuid(),
                Name = genreDto.Genre.Name,
                ParentGenreId = genreDto.Genre.ParentGenreId,
            };

            return await _genreRepository.AddGenreAsync(genre);
        }

        private void ValidateGenreDto(CreateGenreRequest genreDto)
        {
            if (genreDto == null || genreDto.Genre == null || string.IsNullOrWhiteSpace(genreDto.Genre.Name))
            {
                throw new ArgumentException("Genre name cannot be null or empty.");
            }

        }

        private async Task ValidateParentGenreAsync(Guid parentGenreId)
        {
            var parentGenre = await _genreRepository.GetGenreByIdAsync(parentGenreId);
            if (parentGenre == null)
            {
                throw new ArgumentException("ParentGenreId must reference an existing genre.");
            }
        }

        public async Task<GetGenreResponse> GetGenreByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var genreId))
            {
                throw new NotFoundException($"Invalid ID format: {id}. Must be a valid GUID or Integer");
            }

            var genre = await _genreRepository.GetGenreByIdAsync(genreId);

            if (genre == null)
            {
                throw new NotFoundException($"Genre with ID {id} not found.");
            }

            return new GetGenreResponse
            {
                Id = genre.Id,
                Name = genre.Name,
                ParentGenreId = genre.ParentGenreId,
            };
        }

        public async Task<IEnumerable<GetGenreResponse>> GetAllGenresAsync()
        {
            var genres = await _genreRepository.GetAllGenresAsync();

            return genres.Select(genre => new GetGenreResponse
            {
                Id = genre.Id,
                Name = genre.Name,
                ParentGenreId=genre.ParentGenreId,
            });
        }

        public async Task<List<GetGenreDetailsResponse>> GetgenresByParentIdAsync(Guid parentId)
        {
            var genres = await _genreRepository.GetgenresByParentIdAsync(parentId);

            if (genres == null || genres.Count == 0)
            {
                throw new NotFoundException("No genres found for the given parent ID.");
            }

            return genres.Select(genre => new GetGenreDetailsResponse
            {
                Id = genre.Id,
                Name = genre.Name,
            }).ToList();
        }

        public async Task<GenreUpdateDto> UpdateGenreAsync(GenreUpdateDto genreDto)
        {
            var existingGenre = await _genreRepository.GetGenreByIdAsync(genreDto.Id);
            ValidateExistingGenre(existingGenre, genreDto.Id);
            ValidateGenreName(genreDto.Name);

            if (genreDto.ParentGenreId.HasValue)
            {
                await ValidateParentGenreAsyncUp(existingGenre, genreDto.ParentGenreId.Value);
            }

            existingGenre.Name = genreDto.Name;
            existingGenre.ParentGenreId = genreDto.ParentGenreId;

            await _genreRepository.UpdateGenreAsync(existingGenre);

            return genreDto;
        }

        private void ValidateExistingGenre(Genre existingGenre, Guid genreId)
        {
            if (existingGenre == null)
            {
                throw new NotFoundException($"Genre not found for ID: {genreId}");
            }
        }

        private void ValidateGenreName(string genreName)
        {
            if (string.IsNullOrWhiteSpace(genreName))
            {
                throw new ArgumentException("Genre name cannot be null or empty.");
            }
        }

        private async Task ValidateParentGenreAsyncUp(Genre existingGenre, Guid parentGenreId)
        {
            var parentGenre = await _genreRepository.GetGenreByIdAsync(parentGenreId);
            if (parentGenre == null)
            {
                throw new ArgumentException("ParentGenreId must reference an existing genre.");
            }

            if (parentGenre.Id == existingGenre.Id)
            {
                throw new ArgumentException("Cyclic reference detected.");
            }

            if (parentGenre.ParentGenreId.HasValue && parentGenre.ParentGenreId.Value == existingGenre.Id)
            {
                throw new ArgumentException("Cyclic reference detected.");
            }
        }

        public async Task<Genre> DeleteGenreAsync(Guid id)
        {
            var genreToDelete = await _genreRepository.GetGenreByIdAsync(id);
            if (genreToDelete == null)
            {
                throw new NotFoundException($"Genre with ID '{id}' not found.");
            }

            await _genreRepository.DeleteGenreAsync(genreToDelete);
            return genreToDelete;
        }
    }
}
