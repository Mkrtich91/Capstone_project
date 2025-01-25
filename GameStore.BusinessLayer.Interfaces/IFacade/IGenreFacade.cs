// <copyright file="IGenreFacade.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IFacade
{
    using GameStore.BusinessLayer.Interfaces.ResponseDto;

    public interface IGenreFacade
    {
        Task<IEnumerable<GetGenreResponse>> GetGenresAndCategoriesAsync();

        Task<GetGenreResponse> GetGenreOrCategoryByIdAsync(string id);
    }
}
