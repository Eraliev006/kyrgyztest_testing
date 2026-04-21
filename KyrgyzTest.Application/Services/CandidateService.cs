using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _repository;

    public CandidateService(ICandidateRepository repository)
    {
        _repository = repository;
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
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(candidate);
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

    public async Task<CandidateResponseDto> AllowAccessAsync(Guid id)
    {
        var candidate = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Кандидат не найден");

        candidate.IsAllowed = true;
        var updated = await _repository.UpdateAsync(candidate);
        return Map(updated);
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
        CreatedAt = c.CreatedAt
    };
}