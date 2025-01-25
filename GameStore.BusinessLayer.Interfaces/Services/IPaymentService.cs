// <copyright file="IPaymentService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;

    public interface IPaymentService
    {
        IEnumerable<PaymentMethodDto> GetPaymentMethods();
    }
}
