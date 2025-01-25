// <copyright file="GetGenreResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.ResponseDto
{
    public class GetGenreResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public Guid? ParentGenreId { get; set; }
    }
}
