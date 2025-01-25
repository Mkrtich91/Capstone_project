// <copyright file="PaymentResultDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public class PaymentResultDto
    {
        public Guid OrderId { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsSuccess { get; set; }

        public Stream? InvoiceStream { get; set; }
    }
}
