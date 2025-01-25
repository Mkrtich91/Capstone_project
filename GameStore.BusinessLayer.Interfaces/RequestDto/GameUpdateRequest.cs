// <copyright file="GameUpdateRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.RequestDto
{
    using GameStore.BusinessLayer.Interfaces.DTO;

    public class GameUpdateRequest
    {
        public GameUpdateDto? Game { get; set; }

        public List<Guid>? Genres { get; set; }

        public List<Guid>? Platforms { get; set; }

        public Guid Publisher { get; set; }
    }
}
