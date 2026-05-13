using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface IAttemptRepository
{
    Task<Attempt?> GetActiveByCandidate(Guid candidateId);
    Task<Attempt?> GetByIdWithDetailsAsync(Guid id);
    Task<Attempt> AddAsync(Attempt attempt);
    Task UpdateAsync(Attempt attempt);
}
