// <copyright file="PublisherUpdateDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.DTO
{
    public class PublisherUpdateDto
    {
        public Guid Id { get; set; }

        public string? CompanyName { get; set; }

        public string? HomePage { get; set; }

        public string? Description { get; set; }
    }
}
