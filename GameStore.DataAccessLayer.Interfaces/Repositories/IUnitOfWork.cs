
namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IGameRepository GameRepository { get; }

        IGenreRepository GenreRepository { get; }

        IPlatformRepository PlatformRepository { get; }

        IPublisherRepository PublisherRepository { get; }

        IOrderRepository OrderRepository { get; }

        IOrderGameRepository OrderGameRepository { get; }

        ICommentRepository CommentRepository { get; }

        IPermissionRepository PermissionRepository { get; }

        IRoleRepository RoleRepository { get; }

        Task SaveAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
