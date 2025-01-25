// <copyright file="GetPaymentResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.ResponseDto
{
    public class GetPaymentResponse
    {
        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public DateTime PaymentDate { get; set; }

        public double Sum { get; set; }
    }
}
