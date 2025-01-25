// <copyright file="GameResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.ResponseDto
{
    public class GameResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Key { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public double Discount { get; set; }

        public int UnitInStock { get; set; }
    }
}
