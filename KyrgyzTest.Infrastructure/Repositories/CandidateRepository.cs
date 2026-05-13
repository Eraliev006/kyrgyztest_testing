using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class CandidateRepository: ICandidateRepository
{
    private readonly AppDbContext _context;
    
    public CandidateRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Candidate?> GetByIdAsync(Guid id)
        => await _context.Candidates.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Candidate?> GetByInnAsync(string inn)
        => await _context.Candidates.FirstOrDefaultAsync(x => x.Inn == inn);

    public async Task<Candidate?> GetByAccessCodeAsync(string accessCode)
        => await _context.Candidates.FirstOrDefaultAsync(x => x.AccessCode == accessCode);

    public async Task<List<Candidate>> GetAllAsync()
        => await _context.Candidates.ToListAsync();

    public async Task<Candidate> CreateAsync(Candidate candidate)
    {
        _context.Candidates.Add(candidate);
        await _context.SaveChangesAsync();
        return candidate;
    }

    public async Task<Candidate> UpdateAsync(Candidate candidate)
    {
        _context.Candidates.Update(candidate);
        await _context.SaveChangesAsync();
        return candidate;
    }

    public async Task DeleteAsync(Guid id)
    {
        var candidate = await _context.Candidates.FindAsync(id);
        if (candidate != null)
        {
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
        }
    }
}