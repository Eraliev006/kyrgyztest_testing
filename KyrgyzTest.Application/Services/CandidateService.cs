using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _repository;
    private readonly IAttemptRepository _attemptRepository;
    private readonly IAuditService _audit;

    public CandidateService(ICandidateRepository repository, IAttemptRepository attemptRepository, IAuditService audit)
    {
        _repository = repository;
        _attemptRepository = attemptRepository;
        _audit = audit;
    }

    public async Task<CandidateResponseDto> CreateAsync(CreateCandidateDto dto)
    {
        var existing = await _repository.GetByInnAsync(dto.Inn);
        if (existing != null)
            throw new BusinessException("Кандидат с таким ИНН уже существует");

        var candidate = new Candidate
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Inn = dto.Inn,
            AccessCode = GenerateAccessCode(),
            IsAllowed = true,
            CreatedAt = DateTime.UtcNow,
            OrganizationId = dto.OrganizationId
        };

        var created = await _repository.CreateAsync(candidate);
        await _audit.LogAsync("CREATE", "Candidate", created.Id, $"Создан кандидат {created.FullName} (ИНН: {created.Inn})");
        return Map(created);
    }

    public async Task<CandidateResponseDto?> GetByIdAsync(Guid id)
    {
        var candidate = await _repository.GetByIdAsync(id);
        return candidate == null ? null : Map(candidate);
    }

    public async Task<CandidateResponseDto?> GetByInnAsync(string inn)
    {
        var candidate = await _repository.GetByInnAsync(inn);
        return candidate == null ? null : Map(candidate);
    }

    public async Task<CandidateResponseDto?> GetByAccessCodeAsync(string accessCode)
    {
        var candidate = await _repository.GetByAccessCodeAsync(accessCode);
        return candidate == null ? null : Map(candidate);
    }

    public async Task<List<CandidateResponseDto>> GetAllAsync()
    {
        var candidates = await _repository.GetAllAsync();
        return candidates.Select(Map).ToList();
    }

    public async Task<List<CandidateResponseDto>> SearchByNameAsync(string name)
    {
        var candidates = await _repository.SearchByNameAsync(name);
        return candidates.Select(Map).ToList();
    }

    public async Task<PagedResultDto<CandidateResponseDto>> GetPagedAsync(
        Guid? organizationId, DateTime? dateFrom, DateTime? dateTo, int page, int pageSize)
    {
        if (organizationId is null && dateFrom is null && dateTo is null)
        {
            var today = DateTime.UtcNow.Date;
            dateFrom = today;
            dateTo = today.AddDays(1);
        }

        var (items, total) = await _repository.GetPagedAsync(organizationId, dateFrom, dateTo, page, pageSize);
        return new PagedResultDto<CandidateResponseDto>
        {
            Items = items.Select(Map).ToList(),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<CandidateResponseDto> AllowAccessAsync(Guid id)
    {
        var candidate = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Кандидат не найден");

        candidate.IsAllowed = true;
        var updated = await _repository.UpdateAsync(candidate);
        await _audit.LogAsync("ALLOW_ACCESS", "Candidate", id, $"Открыт доступ кандидату {candidate.FullName}");
        return Map(updated);
    }

    public async Task<CandidateResponseDto> DenyAccessAsync(Guid id)
    {
        var candidate = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Кандидат не найден");

        candidate.IsAllowed = false;
        var updated = await _repository.UpdateAsync(candidate);
        await _audit.LogAsync("DENY_ACCESS", "Candidate", id, $"Закрыт доступ кандидату {candidate.FullName}");

        var activeAttempts = await _attemptRepository.GetAllActiveByCandidate(id);
        foreach (var attempt in activeAttempts)
        {
            attempt.Status = AttemptStatus.Completed;
            attempt.SubmittedAt = DateTime.UtcNow;
            await _attemptRepository.UpdateAsync(attempt);
        }

        return Map(updated);
    }

    public async Task<CandidateResponseDto> BlockAsync(Guid id, BlockCandidateDto dto)
    {
        var candidate = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Кандидат не найден");

        candidate.BlockedUntil = dto.Unit switch
        {
            "days"   => DateTime.UtcNow.AddDays(dto.Value),
            "weeks"  => DateTime.UtcNow.AddDays(dto.Value * 7),
            "months" => DateTime.UtcNow.AddMonths(dto.Value),
            "years"  => DateTime.UtcNow.AddYears(dto.Value),
            _ => throw new ValidationException("Допустимые единицы: days, weeks, months, years")
        };

        var updated = await _repository.UpdateAsync(candidate);
        await _audit.LogAsync("BLOCK", "Candidate", id, $"Кандидат {candidate.FullName} заблокирован до {candidate.BlockedUntil:dd.MM.yyyy}");
        return Map(updated);
    }

    public async Task<CandidateResponseDto> UploadPhotoAsync(Guid id, string photo)
    {
        var candidate = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Кандидат не найден");

        candidate.Photo = photo;
        var updated = await _repository.UpdateAsync(candidate);
        return Map(updated);
    }

    public async Task<CandidateResponseDto> UpdateAsync(Guid id, UpdateCandidateDto dto)
    {
        if (dto.Inn.Length != 14)
            throw new ValidationException("ИНН должен содержать ровно 14 символов");

        var candidate = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Кандидат не найден");

        var existing = await _repository.GetByInnAsync(dto.Inn);
        if (existing != null && existing.Id != id)
            throw new ValidationException("Кандидат с таким ИНН уже существует");

        candidate.FullName = dto.FullName;
        candidate.Inn = dto.Inn;
        candidate.OrganizationId = dto.OrganizationId;

        var updated = await _repository.UpdateAsync(candidate);
        await _audit.LogAsync("UPDATE", "Candidate", id, $"Обновлён кандидат {candidate.FullName} (ИНН: {candidate.Inn})");
        return Map(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var candidate = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Кандидат не найден");

        await _repository.DeleteAsync(id);
        await _audit.LogAsync("DELETE", "Candidate", id, $"Удалён кандидат {candidate.FullName} (ИНН: {candidate.Inn})");
    }

    private static string GenerateAccessCode()
    {
        var date = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = Random.Shared.Next(1000, 9999).ToString();
        return $"{date}-{random}";
    }

    private static CandidateResponseDto Map(Candidate c) => new()
    {
        Id = c.Id,
        FullName = c.FullName,
        Inn = c.Inn,
        AccessCode = c.AccessCode,
        IsAllowed = c.IsAllowed,
        CreatedAt = c.CreatedAt,
        OrganizationId = c.OrganizationId,
        Photo = c.Photo,
        BlockedUntil = c.BlockedUntil
    };
}