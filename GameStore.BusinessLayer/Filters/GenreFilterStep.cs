// <copyright file="GenreFilterStep.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Filters
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Services;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public class GenreFilterStep : IPipelineStep
    {
        public IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto)
        {
            if (queryDto.GenreIds != null && queryDto.GenreIds.Any())
            {
                input = input.Where(g => g.GameGenres.Any(gg => queryDto.GenreIds.Contains(gg.GenreId)));
            }

            return input;
        }
    }
}
