// <copyright file="GenreUpdateDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    public class GenreUpdateDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public Guid? ParentGenreId { get; set; }
    }
}
