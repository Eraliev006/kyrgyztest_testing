using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace KyrgyzTest.Application.Services;

public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _repository;
    private readonly IAttemptRepository _attemptRepository;
    private readonly IAuditService _audit;
    private readonly IMemoryCache _cache;
    private readonly IExamService _examService;
    private readonly CandidateCacheInvalidator _cacheInvalidator;

    public CandidateService(
        ICandidateRepository repository,
        IAttemptRepository attemptRepository,
        IAuditService audit,
        IMemoryCache cache,
        IExamService examService,
        CandidateCacheInvalidator cacheInvalidator)
    {
        _repository = repository;
        _attemptRepository = attemptRepository;
        _audit = audit;
        _cache = cache;
        _examService = examService;
        _cacheInvalidator = cacheInvalidator;
    }

    public async Task<CandidateResponseDto> CreateAsync(CreateCandidateDto dto)
    {
        var existing = await _repository.GetByInnAsync(dto.Inn);
        if (existing != null)
            throw new BusinessException("Кандидат с таким ИНН уже существует");

        var accessCode = await GenerateUniqueAccessCodeAsync();

        var candidate = new Candidate
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Inn = dto.Inn,
            AccessCode = accessCode,
            IsAllowed = true,
            CreatedAt = DateTime.UtcNow,
            OrganizationId = dto.OrganizationId
        };

        var created = await _repository.CreateAsync(candidate);
        await _audit.LogAsync("CREATE", "Candidate", created.Id, $"Создан кандидат {created.FullName} (ИНН: {created.Inn})");
        InvalidateCandidatesCache();
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

        var cacheKey = $"candidates_{organizationId}_{dateFrom:yyyyMMddHHmm}_{dateTo:yyyyMMddHHmm}_{page}_{pageSize}";
        if (_cache.TryGetValue(cacheKey, out PagedResultDto<CandidateResponseDto>? cached))
            return cached!;

        var (items, total) = await _repository.GetPagedAsync(organizationId, dateFrom, dateTo, page, pageSize);
        var result = new PagedResultDto<CandidateResponseDto>
        {
            Items = items.Select(Map).ToList(),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };

        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
            .AddExpirationToken(_cacheInvalidator.GetChangeToken());
        _cache.Set(cacheKey, result, options);
        return result;
    }

    public async Task<CandidateResponseDto> AllowAccessAsync(Guid id)
    {
        var candidate = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Кандидат не найден");

        candidate.IsAllowed = true;
        candidate.BlockedUntil = null;
        var updated = await _repository.UpdateAsync(candidate);
        await _audit.LogAsync("ALLOW_ACCESS", "Candidate", id, $"Открыт доступ кандидату {candidate.FullName}");
        InvalidateCandidatesCache();
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
            await _examService.SubmitAsync(attempt.Id);

        InvalidateCandidatesCache();
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
        candidate.IsAllowed = false;

        var updated = await _repository.UpdateAsync(candidate);
        await _audit.LogAsync("BLOCK", "Candidate", id, $"Кандидат {candidate.FullName} заблокирован до {candidate.BlockedUntil:dd.MM.yyyy}");

        var activeAttempts = await _attemptRepository.GetAllActiveByCandidate(id);
        foreach (var attempt in activeAttempts)
            await _examService.SubmitAsync(attempt.Id);

        InvalidateCandidatesCache();
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
        InvalidateCandidatesCache();
        return Map(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var candidate = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Кандидат не найден");

        await _repository.DeleteAsync(id);
        await _audit.LogAsync("DELETE", "Candidate", id, $"Удалён кандидат {candidate.FullName} (ИНН: {candidate.Inn})");
        InvalidateCandidatesCache();
    }

    private void InvalidateCandidatesCache() => _cacheInvalidator.Invalidate();

    private async Task<string> GenerateUniqueAccessCodeAsync()
    {
        const int maxAttempts = 10;
        for (var i = 0; i < maxAttempts; i++)
        {
            var code = GenerateAccessCode();
            if (await _repository.GetByAccessCodeAsync(code) == null)
                return code;
        }
        throw new BusinessException("Не удалось сгенерировать уникальный код доступа. Попробуйте снова.");
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