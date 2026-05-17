using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IAuditService
{
    Task LogAsync(string action, string entityType, Guid entityId, string description);
    Task<List<AuditLogResponseDto>> GetFilteredAsync(DateTime? dateFrom, DateTime? dateTo, Guid? userId, string? entityType);
}
