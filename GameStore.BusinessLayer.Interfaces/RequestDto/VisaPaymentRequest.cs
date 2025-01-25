// <copyright file="VisaPaymentRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.RequestDto
{
    public class VisaPaymentRequest
    {
        public string? CardHolderName { get; set; }

        public string? CardNumber { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public int Cvv { get; set; }
    }
}
