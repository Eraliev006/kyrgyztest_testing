using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
}
