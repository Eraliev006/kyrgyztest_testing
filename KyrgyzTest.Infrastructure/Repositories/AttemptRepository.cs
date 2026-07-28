using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class AttemptRepository : IAttemptRepository
{
    private readonly AppDbContext _context;

    public AttemptRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Attempt?> GetActiveByCandidate(Guid candidateId)
        => await _context.Attempts
            .FirstOrDefaultAsync(a => a.CandidateId == candidateId && a.Status == AttemptStatus.InProgress);

    public async Task<List<Attempt>> GetAllActiveByCandidate(Guid candidateId)
        => await _context.Attempts
            .Where(a => a.CandidateId == candidateId && a.Status == AttemptStatus.InProgress)
            .ToListAsync();

    public async Task<Attempt?> GetByIdWithDetailsAsync(Guid id)
        => await _context.Attempts
            .Include(a => a.Candidate)
            .Include(a => a.TestVariant)
                .ThenInclude(tv => tv.Questions)
                    .ThenInclude(tvq => tvq.Question)
                        .ThenInclude(q => q.AnswerOptions)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<List<Attempt>> GetAllActiveWithDetailsAsync()
        => await _context.Attempts
            .Include(a => a.Candidate)
            .Include(a => a.TestVariant)
                .ThenInclude(tv => tv.Questions)
                    .ThenInclude(tvq => tvq.Question)
            .Where(a => a.Status == AttemptStatus.InProgress)
            .ToListAsync();

    public async Task<List<Attempt>> GetCompletedWithDetailsAsync()
        => await _context.Attempts
            .Include(a => a.Candidate)
            .Include(a => a.TestVariant)
                .ThenInclude(tv => tv.Questions)
                    .ThenInclude(tvq => tvq.Question)
            .Where(a => a.Status == AttemptStatus.Completed)
            .ToListAsync();

    public async Task<Attempt> AddAsync(Attempt attempt)
    {
        _context.Attempts.Add(attempt);
        await _context.SaveChangesAsync();
        return attempt;
    }

    public async Task UpdateAsync(Attempt attempt)
    {
        _context.Attempts.Update(attempt);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByTestVariantIdAsync(Guid testVariantId)
        => await _context.Attempts.AnyAsync(a => a.TestVariantId == testVariantId);
}
