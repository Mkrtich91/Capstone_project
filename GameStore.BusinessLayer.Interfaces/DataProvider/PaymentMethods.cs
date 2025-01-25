// <copyright file="PaymentMethods.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace GameStore.BusinessLayer.Interfaces.DataProvider
{
    using System.Collections.Generic;
    using GameStore.BusinessLayer.Interfaces.DTO;

    public static class PaymentMethods
    {
        public static List<PaymentMethodDto> Methods { get; } = new List<PaymentMethodDto>
    {
        new PaymentMethodDto
        {
            ImageUrl = "https://example.com/image1.png",
            Title = "Bank",
            Description = "Some text 1",
        },
        new PaymentMethodDto
        {
            ImageUrl = "https://example.com/image2.png",
            Title = "IBox terminal",
            Description = "Some text 2",
        },
        new PaymentMethodDto
        {
            ImageUrl = "https://example.com/image3.png",
            Title = "Visa",
            Description = "Some text 3",
        },
    };
    }
}
