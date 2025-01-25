// <copyright file="CartDetailsDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    public class CartDetailsDto
    {
        public Guid OrderId { get; set; }

        public IEnumerable<CartItemDto>? Items { get; set; }

        public double TotalAmount { get; set; }
    }
}
