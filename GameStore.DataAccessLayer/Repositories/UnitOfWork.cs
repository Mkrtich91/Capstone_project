using GameStore.DataAccessLayer.Database;
using GameStore.DataAccessLayer.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameStore.DataAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DataContext _dbContext;

        private bool _disposed;

        private IDbContextTransaction _transaction;

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGameRepository GameRepository => new GameRepository(_dbContext);

        public IGenreRepository GenreRepository => new GenreRepository(_dbContext);

        public IPlatformRepository PlatformRepository => new PlatformRepository(_dbContext);

        public IPublisherRepository PublisherRepository => new PublisherRepository(_dbContext);

        public IOrderRepository OrderRepository => new OrderRepository(_dbContext);

        public IOrderGameRepository OrderGameRepository => new OrderGameRepository(_dbContext);

        public ICommentRepository CommentRepository => new CommentRepository(_dbContext);

        public IPermissionRepository PermissionRepository => new PermissionRepository(_dbContext);

        public IRoleRepository RoleRepository => new RoleRepository(_dbContext);

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }
    }
}
