using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace KyrgyzTest.Infrastructure.Repositories;

public class CompletedSectionRepository : ICompletedSectionRepository
{
    private readonly AppDbContext _context;

    public CompletedSectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CompletedSection>> GetByAttemptIdAsync(Guid attemptId)
        => await _context.CompletedSections
            .Where(cs => cs.AttemptId == attemptId)
            .ToListAsync();

    public async Task AddAsync(CompletedSection completedSection)
    {
        _context.CompletedSections.Add(completedSection);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> TryAddAsync(CompletedSection completedSection)
    {
        _context.CompletedSections.Add(completedSection);
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg && pg.SqlState == "23505")
        {
            _context.Entry(completedSection).State = EntityState.Detached;
            return false;
        }
    }
}
