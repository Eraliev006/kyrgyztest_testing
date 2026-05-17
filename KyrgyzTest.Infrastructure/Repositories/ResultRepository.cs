using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class ResultRepository : IResultRepository
{
    private readonly AppDbContext _context;

    public ResultRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Result result)
    {
        _context.Results.Add(result);
        await _context.SaveChangesAsync();
    }

    public async Task<Result?> GetByAttemptIdAsync(Guid attemptId)
        => await _context.Results.FirstOrDefaultAsync(r => r.AttemptId == attemptId);

    public async Task<List<Result>> GetByCandidateIdAsync(Guid candidateId)
        => await _context.Results.Where(r => r.CandidateId == candidateId).ToListAsync();

    public async Task<List<Result>> GetAllAsync()
        => await _context.Results.ToListAsync();

    public async Task<List<Result>> GetFilteredAsync(Guid? organizationId, LanguageLevel? level, DateTime? dateFrom, DateTime? dateTo)
    {
        if (dateFrom.HasValue)
            dateFrom = DateTime.SpecifyKind(dateFrom.Value, DateTimeKind.Utc);
        if (dateTo.HasValue)
            dateTo = DateTime.SpecifyKind(dateTo.Value, DateTimeKind.Utc);

        var query = _context.Results
            .Include(r => r.Candidate)
                .ThenInclude(c => c.Organization)
            .AsQueryable();

        if (organizationId.HasValue)
            query = query.Where(r => r.Candidate.OrganizationId == organizationId.Value);

        if (level.HasValue)
            query = query.Where(r => r.Level == level.Value);

        if (dateFrom.HasValue)
            query = query.Where(r => r.CreatedAt >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(r => r.CreatedAt <= dateTo.Value);

        return await query.ToListAsync();
    }

    public async Task<Result?> GetLatestByCandidateIdAsync(Guid candidateId)
        => await _context.Results
            .Include(r => r.Candidate)
                .ThenInclude(c => c.Organization)
            .Where(r => r.CandidateId == candidateId)
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();
}
