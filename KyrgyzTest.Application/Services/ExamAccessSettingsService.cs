using System.Security.Claims;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KyrgyzTest.Application.Services;

public class ExamAccessSettingsService : IExamAccessSettingsService
{
    private readonly IExamAccessSettingsRepository _repository;
    private readonly IAuditService _audit;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExamAccessSettingsService(
        IExamAccessSettingsRepository repository,
        IAuditService audit,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _audit = audit;
        _httpContextAccessor = httpContextAccessor;
    }

    private Guid CurrentUserId()
    {
        var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(idClaim!);
    }

    public async Task<ExamAccessSettingsDto> GetAsync()
    {
        var settings = await _repository.GetAsync()
            ?? throw new NotFoundException("Настройки доступа к экзамену не найдены");

        return new ExamAccessSettingsDto
        {
            AccessPassword = settings.AccessPassword,
            UpdatedAt = settings.UpdatedAt,
            UpdatedByFullName = settings.UpdatedBy?.FullName
        };
    }

    public async Task<ExamAccessSettingsDto> UpdateAsync(UpdateExamAccessPasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NewPassword))
            throw new BusinessException("Пароль не может быть пустым");

        var settings = await _repository.GetAsync()
            ?? throw new NotFoundException("Настройки доступа к экзамену не найдены");

        settings.AccessPassword = dto.NewPassword;
        settings.UpdatedAt = DateTime.UtcNow;
        settings.UpdatedByUserId = CurrentUserId();
        await _repository.UpdateAsync(settings);

        await _audit.LogAsync("UPDATE", "ExamAccessSettings", settings.Id, "Изменён пароль доступа к экзамену");

        return await GetAsync();
    }
}
