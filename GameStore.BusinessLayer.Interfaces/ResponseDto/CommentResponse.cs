// <copyright file="CommentResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.ResponseDto
{
    public class CommentResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Body { get; set; }

        public List<CommentResponse> ChildComments { get; set; } = new List<CommentResponse>();
    }
}
