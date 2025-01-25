namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IPlatformRepository
    {

        Task<Platform> CreatePlatformAsync(Platform platform);

        Task<Platform> GetPlatformByIdAsync(Guid id);

        Task<List<Platform>> GetAllPlatformsAsync();

        Task UpdatePlatformAsync(Platform platform);

        Task DeleteplatformAsync(Platform platform);
    }
}
