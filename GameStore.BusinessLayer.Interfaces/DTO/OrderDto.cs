// <copyright file="OrderDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime Date { get; set; }
    }
}
