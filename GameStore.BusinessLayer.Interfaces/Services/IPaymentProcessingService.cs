// <copyright file="IPaymentProcessingService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using Microsoft.AspNetCore.Mvc;

    public interface IPaymentProcessingService
    {
        Task<IActionResult> ProcessPaymentAsync(PaymentRequest paymentRequest);
    }
}
