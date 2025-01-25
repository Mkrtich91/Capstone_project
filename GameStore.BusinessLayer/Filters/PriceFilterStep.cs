using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.DataAccessLayer.Interfaces.Entities;

namespace GameStore.BusinessLayer.Filters
{
    public class PriceFilterStep : IPipelineStep
    {
        public IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto)
        {
            if (queryDto.MinPrice.HasValue)
            {
                input = input.Where(g => g.Price >= queryDto.MinPrice.Value);
            }

            if (queryDto.MaxPrice.HasValue)
            {
                input = input.Where(g => g.Price <= queryDto.MaxPrice.Value);
            }

            return input;
        }
    }
}
