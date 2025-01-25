// <copyright file="VisaPaymentModelDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    public class VisaPaymentModelDto
    {
        public string? Holder { get; set; }

        public string? CardNumber { get; set; }

        public int MonthExpire { get; set; }

        public int YearExpire { get; set; }

        public int Cvv2 { get; set; }
    }
}
