// <copyright file="GameQueryDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    using GameStore.BusinessLayer.Interfaces.DataProvider;

    public class GameQueryDto
    {
        public List<Guid> GenreIds { get; set; } = new List<Guid>();

        public List<Guid> PlatformIds { get; set; } = new List<Guid>();

        public List<Guid> PublisherIds { get; set; } = new List<Guid>();

        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }

        public string? Name { get; set; }

        public string PublishDateRangeOption { get; set; } = PublishDateFilterOptions.None;

        public string SortBy { get; set; } = SortingOptions.New;

        public int PageNumber { get; set; } = 0;

        public int PageSize { get; set; } = 10;
    }
}
