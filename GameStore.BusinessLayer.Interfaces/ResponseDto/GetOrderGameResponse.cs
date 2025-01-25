// <copyright file="GetOrderGameResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.ResponseDto
{
    public class GetOrderGameResponse
    {
        public Guid GameId { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public int? Discount { get; set; }
    }
}
