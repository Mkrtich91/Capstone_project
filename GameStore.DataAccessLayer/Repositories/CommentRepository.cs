using GameStore.DataAccessLayer.Database;
using GameStore.DataAccessLayer.Interfaces.Entities;
using GameStore.DataAccessLayer.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccessLayer.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Comment> GetCommentByIdAsync(Guid commentId)
        {
            return await _context.Comments
                .Include(c => c.Replies) 
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByGameIdAsync(Guid gameId)
        {
            return await _context.Comments
                .Where(c => c.GameId == gameId)
                .Include(c => c.Replies)
                .ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByQuotedCommentIdAsync(Guid parentCommentId)
        {
            return await _context.Comments
                .Where(c => c.ParentCommentId == parentCommentId)
                .ToListAsync();
        }
    }
}
