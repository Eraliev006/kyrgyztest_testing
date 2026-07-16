using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace KyrgyzTest.Infrastructure.Repositories;

public class SectionTimingRepository : ISectionTimingRepository
{
    private readonly AppDbContext _context;

    public SectionTimingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SectionTiming?> GetByAttemptAndSectionAsync(Guid attemptId, SectionType section)
        => await _context.SectionTimings
            .FirstOrDefaultAsync(t => t.AttemptId == attemptId && t.Section == section);

    public async Task<SectionTiming> GetOrStartAsync(Guid attemptId, SectionType section)
    {
        var existing = await GetByAttemptAndSectionAsync(attemptId, section);
        if (existing != null)
            return existing;

        var timing = new SectionTiming
        {
            Id = Guid.NewGuid(),
            AttemptId = attemptId,
            Section = section,
            StartedAt = DateTime.UtcNow
        };

        _context.SectionTimings.Add(timing);
        try
        {
            await _context.SaveChangesAsync();
            return timing;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg && pg.SqlState == "23505")
        {
            _context.Entry(timing).State = EntityState.Detached;
            return await GetByAttemptAndSectionAsync(attemptId, section)
                ?? throw new InvalidOperationException("SectionTiming insert conflicted but no row found");
        }
    }
}
