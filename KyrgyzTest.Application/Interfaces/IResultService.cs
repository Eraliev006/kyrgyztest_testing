using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.Interfaces;

public interface IResultService
{
    Task<List<ResultWithCandidateDto>> GetAllAsync(Guid? organizationId, LanguageLevel? level, DateTime? dateFrom, DateTime? dateTo);
    Task<ResultWithCandidateDto> GetByCandidateIdAsync(Guid candidateId);
    Task<OrgStatsDto> GetStatsAsync(Guid? organizationId, DateTime? dateFrom, DateTime? dateTo);
    Task<AttemptDetailDto> GetAttemptDetailsAsync(Guid attemptId);
}
