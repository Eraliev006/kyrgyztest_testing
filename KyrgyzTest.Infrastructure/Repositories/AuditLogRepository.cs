using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog log)
    {
        await _context.AuditLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task<List<AuditLog>> GetFilteredAsync(DateTime? dateFrom, DateTime? dateTo, Guid? userId, string? entityType)
    {
        if (dateFrom.HasValue)
            dateFrom = DateTime.SpecifyKind(dateFrom.Value, DateTimeKind.Utc);
        if (dateTo.HasValue)
            dateTo = DateTime.SpecifyKind(dateTo.Value, DateTimeKind.Utc);

        var query = _context.AuditLogs.AsQueryable();

        if (dateFrom.HasValue)
            query = query.Where(l => l.CreatedAt >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(l => l.CreatedAt <= dateTo.Value);

        if (userId.HasValue)
            query = query.Where(l => l.UserId == userId.Value);

        if (!string.IsNullOrEmpty(entityType))
            query = query.Where(l => l.EntityType == entityType);

        return await query.OrderByDescending(l => l.CreatedAt).ToListAsync();
    }
}
