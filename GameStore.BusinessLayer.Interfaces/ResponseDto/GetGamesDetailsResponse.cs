// <copyright file="GetGamesDetailsResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.ResponseDto
{
    public class GetGamesDetailsResponse
    {
        public IEnumerable<GameResponse>? Games { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }
    }
}
