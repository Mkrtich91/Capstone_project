// <copyright file="IPipelineStep.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IPipelineStep
    {
        IQueryable<Game> Execute(IQueryable<Game> input, GameQueryDto queryDto);
    }
}
