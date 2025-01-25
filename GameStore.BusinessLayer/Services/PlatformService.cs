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

    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _platformRepository;

        public PlatformService(IPlatformRepository platformRepository)
        {
            _platformRepository = platformRepository;
        }

        public async Task<Platform> CreatePlatformAsync(CreatePlatformRequest request)
        {
            if (request?.Platform == null)
            {
                throw new ArgumentNullException(nameof(request), "CreatePlatformRequest or Platform cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.Platform.Type))
            {
                throw new ArgumentException("Platform type is mandatory and cannot be empty.");
            }

            Platform platform = new Platform
            {
                Id = Guid.NewGuid(),
                Type = request.Platform.Type
            };

            return await _platformRepository.CreatePlatformAsync(platform);
        }

        public async Task<GetPlatformResponseModel> GetPlatformByIdAsync(Guid id)
        {
            var platform = await _platformRepository.GetPlatformByIdAsync(id);
            if (platform == null)
            {
                throw new NotFoundException($"Platform not found for ID: {id}");
            }

            return new GetPlatformResponseModel
            {
                Id = platform.Id,
                Type = platform.Type,
            };
        }

        public async Task<List<GetPlatformResponseModel>> GetAllPlatformsAsync()
        {
            var platforms = await _platformRepository.GetAllPlatformsAsync();
            return platforms.Select(p => new GetPlatformResponseModel { Id = p.Id, Type = p.Type }).ToList();
        }

        public async Task<Platform> UpdatePlatformAsync(UpdatePlatformDto updateDto)
        {
            if (updateDto?.Platform == null)
            {
                throw new ArgumentNullException(nameof(updateDto), "UpdatePlatformDto or Platform cannot be null.");
            }

            if (updateDto.Platform.Id == Guid.Empty)
            {
                throw new ArgumentException("Platform ID is mandatory.");
            }

            if (string.IsNullOrWhiteSpace(updateDto.Platform.Type))
            {
                throw new ArgumentException("Platform type is mandatory and cannot be empty.");
            }

            var platform = await _platformRepository.GetPlatformByIdAsync(updateDto.Platform.Id);

            if (platform == null)
            {
                throw new NotFoundException($"Platform with ID {updateDto.Platform.Id} not found");
            }

            platform.Type = updateDto.Platform.Type;

            await _platformRepository.UpdatePlatformAsync(platform);
            return platform;
        }

        public async Task DeletePlatformAsync(Guid id)
        {
            var platform = await _platformRepository.GetPlatformByIdAsync(id);
            if (platform == null)
            {
                throw new NotFoundException($"Platform with ID {id} not found");
            }

            await _platformRepository.DeleteplatformAsync(platform);
        }
    }
}
