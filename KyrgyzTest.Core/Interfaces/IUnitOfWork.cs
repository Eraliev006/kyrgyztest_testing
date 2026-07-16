namespace KyrgyzTest.Core.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
