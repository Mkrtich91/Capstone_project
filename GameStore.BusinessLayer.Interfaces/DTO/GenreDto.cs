// <copyright file="GenreDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    public class GenreDto
    {
        public string? Name { get; set; }

        public Guid? ParentGenreId { get; set; }
    }
}
