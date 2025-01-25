using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.DataAccessLayer.Interfaces.Entities;

namespace GameStore.BusinessLayer.Pagination
{
    public class GamePaginationStep : IPipelineStep
    {
        public IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto)
        {
            return input.Skip(queryDto.PageNumber * queryDto.PageSize)
                        .Take(queryDto.PageSize);
        }
    }
}
