using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class ResultService : IResultService
{
    private readonly IResultRepository _repository;

    public ResultService(IResultRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ResultWithCandidateDto>> GetAllAsync(Guid? organizationId, LanguageLevel? level, DateTime? dateFrom, DateTime? dateTo)
    {
        var results = await _repository.GetFilteredAsync(organizationId, level, dateFrom, dateTo);
        return results.Select(Map).ToList();
    }

    public async Task<ResultWithCandidateDto> GetByCandidateIdAsync(Guid candidateId)
    {
        var result = await _repository.GetLatestByCandidateIdAsync(candidateId)
            ?? throw new NotFoundException("Результат не найден");
        return Map(result);
    }

    public async Task<OrgStatsDto> GetStatsAsync(Guid? organizationId, DateTime? dateFrom, DateTime? dateTo)
    {
        var results = await _repository.GetFilteredAsync(organizationId, null, dateFrom, dateTo);

        var stats = new OrgStatsDto
        {
            TotalCount = results.Count,
            AverageScore = results.Count > 0 ? results.Average(r => r.TotalScore) : 0,
            ByLevel = new Dictionary<string, int>
            {
                ["a1"] = results.Count(r => r.Level == LanguageLevel.A1),
                ["a2"] = results.Count(r => r.Level == LanguageLevel.A2),
                ["b1"] = results.Count(r => r.Level == LanguageLevel.B1),
                ["b2"] = results.Count(r => r.Level == LanguageLevel.B2)
            }
        };

        return stats;
    }

    private static ResultWithCandidateDto Map(Result r) => new()
    {
        Id = r.Id,
        AttemptId = r.AttemptId,
        CandidateId = r.CandidateId,
        CandidateFullName = r.Candidate.FullName,
        CandidateInn = r.Candidate.Inn,
        OrganizationId = r.Candidate.OrganizationId,
        OrganizationShortName = r.Candidate.Organization?.ShortName,
        Level = r.Level,
        GrammarScore = r.GrammarScore,
        ListeningScore = r.ListeningScore,
        ReadingScore = r.ReadingScore,
        WritingScore = r.WritingScore,
        TotalScore = r.TotalScore,
        CreatedAt = r.CreatedAt
    };
}
