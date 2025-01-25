using GameStore.BusinessLayer.Interfaces.DataProvider;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.DataAccessLayer.Interfaces.Entities;

namespace GameStore.BusinessLayer.Filters
{
    public class PublishDateFilterStep : IPipelineStep
    {
        public IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto)
        {
            var currentDate = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(queryDto.PublishDateRangeOption) &&
                queryDto.PublishDateRangeOption != PublishDateFilterOptions.None)
            {
                switch (queryDto.PublishDateRangeOption)
                {
                    case PublishDateFilterOptions.LastWeek:
                        input = input.Where(g => g.PublishedDate >= currentDate.AddDays(-7));
                        break;
                    case PublishDateFilterOptions.LastMonth:
                        input = input.Where(g => g.PublishedDate >= currentDate.AddMonths(-1));
                        break;
                    case PublishDateFilterOptions.LastYear:
                        input = input.Where(g => g.PublishedDate >= currentDate.AddYears(-1));
                        break;
                    case PublishDateFilterOptions.TwoYears:
                        input = input.Where(g => g.PublishedDate >= currentDate.AddYears(-2));
                        break;
                    case PublishDateFilterOptions.ThreeYears:
                        input = input.Where(g => g.PublishedDate >= currentDate.AddYears(-3));
                        break;
                    default:
                        throw new ArgumentException($"Invalid publish date filter option: {queryDto.PublishDateRangeOption}", nameof(queryDto.PublishDateRangeOption));
                }
            }

            return input;
        }
    }
}