// <copyright file="UpdateGenreRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.RequestDto
{
    using GameStore.BusinessLayer.Interfaces.DTO;

    public class UpdateGenreRequest
    {
        public GenreUpdateDto? Genre { get; set; }
    }
}
