// <copyright file="IVisaPaymentService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;

    public interface IVisaPaymentService
    {
        Task ProcessVisaPaymentAsync(VisaPaymentModelDto visaPaymentModel);
    }
}
