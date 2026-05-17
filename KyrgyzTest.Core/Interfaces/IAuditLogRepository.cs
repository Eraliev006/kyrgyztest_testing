using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log);
    Task<List<AuditLog>> GetFilteredAsync(DateTime? dateFrom, DateTime? dateTo, Guid? userId, string? entityType);
}
