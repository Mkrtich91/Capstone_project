using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.DataAccessLayer.Interfaces.Entities;

namespace GameStore.BusinessLayer.Filters
{
    public class PublisherFilterStep : IPipelineStep
    {
        public IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto)
        {
            if (queryDto.PublisherIds.Any())
            {
                input = input.Where(g => queryDto.PublisherIds.Contains(g.PublisherId));
            }

            return input;
        }
    }

}
