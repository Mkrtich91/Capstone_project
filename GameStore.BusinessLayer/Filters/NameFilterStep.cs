using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.DataAccessLayer.Interfaces.Entities;
namespace GameStore.BusinessLayer.Filters
{
    public class NameFilterStep : IPipelineStep
    {
        public IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto)
        {
            if (!string.IsNullOrWhiteSpace(queryDto.Name) && queryDto.Name.Length >= 3)
            {
                var nameToSearch = queryDto.Name.ToLower();
                input = input.Where(g => g.Name.ToLower().Contains(nameToSearch));
            }

            return input;
        }
    }
}
