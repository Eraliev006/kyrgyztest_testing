using KyrgyzTest.Core.Entities;
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
}
