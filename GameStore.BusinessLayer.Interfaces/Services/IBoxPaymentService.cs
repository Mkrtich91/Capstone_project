// <copyright file="IBoxPaymentService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;

    public interface IBoxPaymentService
    {
        Task<GetPaymentResponse> ProcessPaymentAsync(PaymentRequest paymentRequest);
    }
}
