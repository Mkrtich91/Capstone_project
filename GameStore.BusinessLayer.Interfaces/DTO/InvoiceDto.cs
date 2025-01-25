// <copyright file="InvoiceDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    public class InvoiceDto
    {
        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ValidityDate { get; set; }

        public double Sum { get; set; }
    }
}
