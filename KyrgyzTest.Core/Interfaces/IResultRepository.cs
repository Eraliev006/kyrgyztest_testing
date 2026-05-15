using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Interfaces;

public interface IResultRepository
{
    Task AddAsync(Result result);
    Task<Result?> GetByAttemptIdAsync(Guid attemptId);
    Task<List<Result>> GetByCandidateIdAsync(Guid candidateId);
    Task<List<Result>> GetAllAsync();
    Task<List<Result>> GetFilteredAsync(Guid? organizationId, LanguageLevel? level, DateTime? dateFrom, DateTime? dateTo);
    Task<Result?> GetLatestByCandidateIdAsync(Guid candidateId);
}
