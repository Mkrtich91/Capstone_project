// <copyright file="GetGameResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.ResponseDto
{
    public class GetGameResponse
    {
        public Guid Id { get; set; }

        public string? Description { get; set; }

        public string? Key { get; set; }

        public string? Name { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public int UnitInStock { get; set; }

        public Guid PublisherId { get; set; }

        public DateTime PublishedDate { get; set; }

        public int ViewCount { get; set; }
    }
}
