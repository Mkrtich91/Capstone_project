// <copyright file="ICommentService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface ICommentService
    {
        Task<Comment> AddCommentAsync(CreateCommentRequest request, string gameKey);

        Task<IEnumerable<Comment>> GetCommentsByGameKeyAsync(string gameKey);

        Task<IEnumerable<CommentResponse>> GetAllCommentsByGameKeyAsync(string gameKey);

        Task DeleteCommentAsync(string gameKey, Guid commentId);

        Task<Comment> GetCommentByIdAsync(Guid commentId);
    }
}
