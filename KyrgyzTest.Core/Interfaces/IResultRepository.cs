using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface IResultRepository
{
    Task AddAsync(Result result);
    Task<Result?> GetByAttemptIdAsync(Guid attemptId);
    Task<List<Result>> GetByCandidateIdAsync(Guid candidateId);
    Task<List<Result>> GetAllAsync();
}
