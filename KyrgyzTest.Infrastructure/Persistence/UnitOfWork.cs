using KyrgyzTest.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace KyrgyzTest.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _tx;

    public UnitOfWork(AppDbContext context) => _context = context;

    public async Task BeginTransactionAsync()
        => _tx = await _context.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        await _tx!.CommitAsync();
        await _tx.DisposeAsync();
        _tx = null;
    }

    public async Task RollbackAsync()
    {
        if (_tx is not null)
        {
            await _tx.RollbackAsync();
            await _tx.DisposeAsync();
            _tx = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_tx is not null)
            await _tx.DisposeAsync();
    }
}
