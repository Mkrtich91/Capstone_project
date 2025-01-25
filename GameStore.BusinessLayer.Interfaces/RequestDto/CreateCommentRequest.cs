// <copyright file="CreateCommentRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.RequestDto
{
    using GameStore.BusinessLayer.Interfaces.DTO;

    public class CreateCommentRequest
    {
        public CommentDto? Comment { get; set; }

        public string? ParentId { get; set; }

        public string? Action { get; set; }
    }
}
