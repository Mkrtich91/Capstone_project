// <copyright file="PublisherGameDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    public class PublisherGameDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Key { get; set; }

        public double Price { get; set; }

        public decimal Discount { get; set; }

        public int UnitInStock { get; set; }
    }
}
