namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IGenreRepository
    {
        Task<Genre> AddGenreAsync(Genre genre);

        Task<Genre> GetGenreByIdAsync(Guid id);

        Task<IEnumerable<Genre>> GetAllGenresAsync();

        Task<List<Genre>> GetgenresByParentIdAsync(Guid parentId);

        Task UpdateGenreAsync(Genre genre);

        Task DeleteGenreAsync(Genre genre);
    }
}
