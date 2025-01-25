// <copyright file="IPaymentBankService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.RequestDto;

    public interface IPaymentBankService
    {
        Task<byte[]> ProcessPaymentAsync(PaymentRequest paymentRequest);
    }
}
