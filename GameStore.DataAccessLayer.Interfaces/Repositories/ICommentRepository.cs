using GameStore.DataAccessLayer.Interfaces.Entities;
using System;
namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task AddCommentAsync(Comment comment);

        Task<IEnumerable<Comment>> GetCommentsByGameIdAsync(Guid gameId);

        Task<Comment> GetCommentByIdAsync(Guid commentId);

        Task UpdateCommentAsync(Comment comment);

        Task<IEnumerable<Comment>> GetCommentsByQuotedCommentIdAsync(Guid parentCommentId);
    }
}
