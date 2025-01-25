namespace GameStore.BusinessLayer.Sorting
{
    using GameStore.BusinessLayer.Interfaces.DataProvider;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Services;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public class GameSortStep : IPipelineStep
    {
        public IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto)
        {
            return queryDto.SortBy switch
            {
                SortingOptions.MostPopular => input.OrderByDescending(g => g.ViewCount),
                SortingOptions.MostCommented => input.OrderByDescending(g => g.Comments.Count),
                SortingOptions.PriceAsc => input.OrderBy(g => g.Price),
                SortingOptions.PriceDesc => input.OrderByDescending(g => g.Price),
                SortingOptions.New => input.OrderByDescending(g => g.PublishedDate),
                _ => throw new ArgumentException("Invalid sorting option specified.")
            };
        }
    }
}
