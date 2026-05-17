using System.Security.Claims;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KyrgyzTest.Application.Services;

public class AuditService : IAuditService
{
    private readonly IAuditLogRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditService(IAuditLogRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(string action, string entityType, Guid entityId, string description)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userIdStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = user?.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        Guid.TryParse(userIdStr, out var userId);

        await _repository.AddAsync(new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            UserName = userName,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Description = description,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<List<AuditLogResponseDto>> GetFilteredAsync(DateTime? dateFrom, DateTime? dateTo, Guid? userId, string? entityType)
    {
        var logs = await _repository.GetFilteredAsync(dateFrom, dateTo, userId, entityType);
        return logs.Select(l => new AuditLogResponseDto
        {
            Id = l.Id,
            UserName = l.UserName,
            Action = l.Action,
            EntityType = l.EntityType,
            Description = l.Description,
            CreatedAt = l.CreatedAt
        }).ToList();
    }
}
