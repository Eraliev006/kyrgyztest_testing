using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly AppDbContext _context;
    public CandidateRepository(AppDbContext context)
    {
         _context = context;
    }

    public async Task<Candidate?> GetByExamCodeAsync(string examCode)
    {
        return await _context.Candidates.FirstOrDefaultAsync(c => c.ExamCode == examCode);
    }
    public async Task<Candidate> CreateAsync(Candidate candidate)
    {
        _context.Candidates.Add(candidate);
        await _context.SaveChangesAsync();
        return candidate;
    }
}