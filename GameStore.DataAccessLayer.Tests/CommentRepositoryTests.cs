// <copyright file="CommentRepositoryTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.DataAccessLayer.Tests
{
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class CommentRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public CommentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetCommentByIdAsync_ValidId_ReturnsComment()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Body = "Test Comment",
                GameId = Guid.NewGuid(),
            };

            using (var context = new DataContext(_options))
            {
                context.Comments.Add(comment);
                await context.SaveChangesAsync();
            }

            Comment result;
            using (var context = new DataContext(_options))
            {
                var repository = new CommentRepository(context);
                result = await repository.GetCommentByIdAsync(comment.Id);
            }

            Assert.NotNull(result);
            Assert.Equal(comment.Id, result.Id);
            Assert.Equal(comment.Body, result.Body);
            Assert.Equal(comment.Name, result.Name);
        }

        [Fact]
        public async Task GetCommentsByGameIdAsync_ValidGameId_ReturnsComments()
        {
            var gameId = Guid.NewGuid();
            var comments = new List<Comment>
        {
            new Comment { Id = Guid.NewGuid(), Name = "User1", Body = "Comment1", GameId = gameId },
            new Comment { Id = Guid.NewGuid(), Name = "User2", Body = "Comment2", GameId = gameId },
        };

            using (var context = new DataContext(_options))
            {
                context.Comments.AddRange(comments);
                await context.SaveChangesAsync();
            }

            IEnumerable<Comment> result;
            using (var context = new DataContext(_options))
            {
                var repository = new CommentRepository(context);
                result = await repository.GetCommentsByGameIdAsync(gameId);
            }

            Assert.NotNull(result);
            Assert.Equal(comments.Count, result.Count());
            Assert.Contains(result, c => c.Body == "Comment1");
            Assert.Contains(result, c => c.Body == "Comment2");
        }

        [Fact]
        public async Task AddCommentAsync_ValidComment_AddsCommentToDatabase()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Name = "User1",
                Body = "This is a test comment",
                GameId = Guid.NewGuid(),
            };

            using (var context = new DataContext(_options))
            {
                var repository = new CommentRepository(context);
                await repository.AddCommentAsync(comment);
            }

            using (var context = new DataContext(_options))
            {
                var result = await context.Comments.FindAsync(comment.Id);
                Assert.NotNull(result);
                Assert.Equal(comment.Body, result.Body);
                Assert.Equal(comment.Name, result.Name);
            }
        }

        [Fact]
        public async Task UpdateCommentAsync_ExistingComment_UpdatesCommentInDatabase()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Name = "User1",
                Body = "Original Comment",
                GameId = Guid.NewGuid(),
            };

            using (var context = new DataContext(this._options))
            {
                context.Comments.Add(comment);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new CommentRepository(context);
                comment.Body = "Updated Comment";
                await repository.UpdateCommentAsync(comment);
            }

            using (var context = new DataContext(_options))
            {
                var result = await context.Comments.FindAsync(comment.Id);
                Assert.NotNull(result);
                Assert.Equal("Updated Comment", result.Body);
            }
        }

        [Fact]
        public async Task GetCommentsByQuotedCommentIdAsync_ValidParentCommentId_ReturnsReplies()
        {
            var parentCommentId = Guid.NewGuid();
            var replies = new List<Comment>
        {
            new Comment { Id = Guid.NewGuid(), Name = "User1", Body = "Reply1", ParentCommentId = parentCommentId },
            new Comment { Id = Guid.NewGuid(), Name = "User2", Body = "Reply2", ParentCommentId = parentCommentId },
        };

            using (var context = new DataContext(_options))
            {
                context.Comments.AddRange(replies);
                await context.SaveChangesAsync();
            }

            IEnumerable<Comment> result;
            using (var context = new DataContext(this._options))
            {
                var repository = new CommentRepository(context);
                result = await repository.GetCommentsByQuotedCommentIdAsync(parentCommentId);
            }

            Assert.NotNull(result);
            Assert.Equal(replies.Count, result.Count());
            Assert.Contains(result, c => c.Body == "Reply1");
            Assert.Contains(result, c => c.Body == "Reply2");
        }

        [Fact]
        public async Task GetCommentByIdAsync_InvalidId_ReturnsNull()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new CommentRepository(context);
                var result = await repository.GetCommentByIdAsync(Guid.NewGuid());
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetCommentsByGameIdAsync_NoComments_ReturnsEmptyList()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new CommentRepository(context);
                var result = await repository.GetCommentsByGameIdAsync(Guid.NewGuid());
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task AddCommentAsync_NullComment_ThrowsException()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new CommentRepository(context);
                await Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddCommentAsync(null));
            }
        }

        [Fact]
        public async Task UpdateCommentAsync_NonExistentComment_ThrowsException()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Name = "NonExistent",
                Body = "This comment does not exist in the database",
                GameId = Guid.NewGuid(),
            };

            using (var context = new DataContext(_options))
            {
                var repository = new CommentRepository(context);
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repository.UpdateCommentAsync(comment));
            }
        }

        [Fact]
        public async Task GetCommentsByQuotedCommentIdAsync_NoReplies_ReturnsEmptyList()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new CommentRepository(context);
                var result = await repository.GetCommentsByQuotedCommentIdAsync(Guid.NewGuid());
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }
    }
}
