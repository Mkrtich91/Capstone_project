// <copyright file="GetOrderResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.ResponseDto
{
    public class GetOrderResponse
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime Date { get; set; }
    }
}
