// <copyright file="PaymentRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.RequestDto
{
    using GameStore.BusinessLayer.Interfaces.DTO;

    public class PaymentRequest
    {
        public string? Method { get; set; }

        public VisaPaymentModelDto? Model { get; set; }
    }
}
