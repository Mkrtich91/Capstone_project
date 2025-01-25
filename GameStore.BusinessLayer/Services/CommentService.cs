namespace GameStore.BusinessLayer.Services
{
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.Extensions.Logging;

    public class CommentService : ICommentService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IBanService _banService;
        private readonly ILogger<CommentService> _logger;
        private const string CommentDeletedMessage = "A comment was deleted.";

        public CommentService(IGameRepository gameRepository, ICommentRepository commentRepository,
                              IBanService banService, ILogger<CommentService> logger)
        {
            _gameRepository = gameRepository;
            _commentRepository = commentRepository;
            _banService = banService;
            _logger = logger;
        }

        public async Task<Comment> AddCommentAsync(CreateCommentRequest request, string gameKey)
        {
            if (_banService.IsUserBanned(request.Comment.Name))
            {
                throw new InvalidOperationException($"User '{request.Comment.Name}' is banned and cannot add comments.");
            }

            if (request.Comment == null ||
                string.IsNullOrWhiteSpace(request.Comment.Name) ||
                string.IsNullOrWhiteSpace(request.Comment.Body))
            {
                throw new ArgumentException("All comment fields are required.");
            }

            var game = await _gameRepository.GetGameByKeyAsync(gameKey);
            if (game == null)
            {
                throw new NotFoundException($"Game with Key {gameKey} not found.");
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Name = request.Comment.Name,
                Body = request.Comment.Body,
                GameId = game.Id,
                ParentCommentId = string.IsNullOrEmpty(request.ParentId) ? (Guid?)null : Guid.Parse(request.ParentId),
            };

            if (!string.IsNullOrEmpty(request.ParentId))
            {
                var parentComment = await _commentRepository.GetCommentByIdAsync(Guid.Parse(request.ParentId));
                if (parentComment == null)
                {
                    throw new NotFoundException($"Parent comment with ID {request.ParentId} not found.");
                }

                if (string.Equals(request.Action, "reply", StringComparison.OrdinalIgnoreCase))
                {
                    comment.Body = $"{parentComment.Name}, {request.Comment.Body}";
                }
                else if (string.Equals(request.Action, "quote", StringComparison.OrdinalIgnoreCase))
                {
                    comment.Body = $"{parentComment.Body}, {request.Comment.Body}";
                }
                else
                {
                    _logger.LogWarning($"Unknown action '{request.Action}' received for comment.");
                    throw new InvalidOperationException($"Unknown action '{request.Action}' for comment.");
                }

                parentComment.Replies.Add(comment);
            }
            else
            {
                comment.ParentCommentId = null;
            }

            await _commentRepository.AddCommentAsync(comment);

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByGameKeyAsync(string gameKey)
        {
            var game = await _gameRepository.GetGameByKeyAsync(gameKey);
            if (game == null)
            {
                throw new ArgumentException($"Game with key {gameKey} not found.");
            }

            return await _commentRepository.GetCommentsByGameIdAsync(game.Id);
        }

        public async Task<Comment> GetCommentByIdAsync(Guid commentId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new NotFoundException($"Comment with ID {commentId} not found.");
            }

            return comment;
        }

        public async Task<IEnumerable<CommentResponse>> GetAllCommentsByGameKeyAsync(string gameKey)
        {
            var game = await _gameRepository.GetGameByKeyAsync(gameKey);
            if (game == null)
            {
                throw new ArgumentException($"Game with key {gameKey} not found.");
            }

            var comments = await _commentRepository.GetCommentsByGameIdAsync(game.Id);

            return comments
                .Where(c => c.ParentCommentId == null)
                .Select(c => new CommentResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Body = c.Body,
                    ChildComments = GetChildComments(c.Id, comments),
                }).ToList();
        }

        private List<CommentResponse> GetChildComments(Guid parentId, IEnumerable<Comment> comments)
        {
            return comments
                .Where(c => c.ParentCommentId == parentId)
                .Select(c => new CommentResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Body = c.Body,
                    ChildComments = GetChildComments(c.Id, comments),
                }).ToList();
        }

        public async Task DeleteCommentAsync(string gameKey, Guid commentId)
        {
            var game = await _gameRepository.GetGameByKeyAsync(gameKey);
            if (game == null)
            {
                throw new NotFoundException($"Game with Key {gameKey} not found.");
            }

            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new NotFoundException($"Comment with ID {commentId} not found.");
            }

            if (comment.GameId != game.Id)
            {
                throw new NotFoundException($"Comment with ID {commentId} does not belong to game with Key {gameKey}.");
            }

            var originalBody = comment.Body;

            comment.Body = CommentDeletedMessage;
            await _commentRepository.UpdateCommentAsync(comment);

            await UpdateQuotedCommentsAsync(commentId, originalBody);
        }

        private async Task UpdateQuotedCommentsAsync(Guid commentId, string originalBody)
        {
            var quotedComments = await _commentRepository.GetCommentsByQuotedCommentIdAsync(commentId);
            foreach (var quotedComment in quotedComments)
            {
                if (quotedComment.Body.StartsWith(originalBody))
                {
                    quotedComment.Body = CommentDeletedMessage + quotedComment.Body.Substring(originalBody.Length);
                }
                else
                {
                    quotedComment.Body = System.Text.RegularExpressions.Regex.Replace(quotedComment.Body,
                        $@"(?<=, )?{System.Text.RegularExpressions.Regex.Escape(originalBody)}",
                        CommentDeletedMessage);
                }

                await _commentRepository.UpdateCommentAsync(quotedComment);

                await UpdateQuotedCommentsAsync(quotedComment.Id, originalBody);
            }
        }
    }
}
