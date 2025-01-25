// <copyright file="IGenreService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IGenreService
    {
        Task<Genre> AddGenreAsync(CreateGenreRequest genreDto);

        Task<GetGenreResponse> GetGenreByIdAsync(string id);

        Task<IEnumerable<GetGenreResponse>> GetAllGenresAsync();

        Task<List<GetGenreDetailsResponse>> GetgenresByParentIdAsync(Guid parentId);

        Task<GenreUpdateDto> UpdateGenreAsync(GenreUpdateDto genreDto);

        Task<Genre> DeleteGenreAsync(Guid id);
    }
}
