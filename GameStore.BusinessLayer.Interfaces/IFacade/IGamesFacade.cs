// <copyright file="IGamesFacade.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IFacade
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;

    public interface IGamesFacade
    {
        Task<IEnumerable<GetGameResponse>> GetAllGamesAsync();

        Task<GetGameResponse> GetGameByIdAsync(string id);

        Task<IEnumerable<GameOverviewDto>> GetAllGameAndProductOverviewsAsync();
    }
}
