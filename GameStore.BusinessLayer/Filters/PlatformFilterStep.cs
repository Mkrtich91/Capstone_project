using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.DataAccessLayer.Interfaces.Entities;

namespace GameStore.BusinessLayer.Filters
{
    public class PlatformFilterStep : IPipelineStep
    {
        public IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto)
        {
            if (queryDto.PlatformIds != null && queryDto.PlatformIds.Any())
            {
                input = input.Where(g => g.GamePlatforms.Any(gp => queryDto.PlatformIds.Contains(gp.PlatformId)));
            }

            return input;
        }
    }
}
