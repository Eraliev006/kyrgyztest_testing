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

    public async Task<List<Candidate>> SearchByNameAsync(string name)
        => await _context.Candidates
            .Where(x => EF.Functions.ILike(x.FullName, $"%{name}%"))
            .ToListAsync();

    public async Task<(List<Candidate> Items, int TotalCount)> GetPagedAsync(
        Guid? organizationId, DateTime? dateFrom, DateTime? dateTo, int page, int pageSize)
    {
        var from = dateFrom.HasValue
            ? DateTime.SpecifyKind(dateFrom.Value, DateTimeKind.Utc)
            : DateTime.SpecifyKind(DateTime.UtcNow.Date, DateTimeKind.Utc);

        var to = dateTo.HasValue
            ? DateTime.SpecifyKind(dateTo.Value.AddDays(1), DateTimeKind.Utc)
            : DateTime.SpecifyKind(DateTime.UtcNow.Date.AddDays(1), DateTimeKind.Utc);

        var query = _context.Candidates.AsQueryable();

        if (organizationId.HasValue)
            query = query.Where(x => x.OrganizationId == organizationId);

        query = query.Where(x => x.CreatedAt >= from);
        query = query.Where(x => x.CreatedAt < to);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

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