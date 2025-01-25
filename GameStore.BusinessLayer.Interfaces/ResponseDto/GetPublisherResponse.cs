// <copyright file="GetPublisherResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.ResponseDto
{
    public class GetPublisherResponse
    {
        public Guid Id { get; set; }

        public string?CompanyName { get; set; }

        public string? HomePage { get; set; }

        public string? Description { get; set; }
    }
}
