using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface ICompletedSectionRepository
{
    Task<List<CompletedSection>> GetByAttemptIdAsync(Guid attemptId);
    Task AddAsync(CompletedSection completedSection);
    /// <summary>
    /// Returns true if inserted, false if (AttemptId, Section) unique constraint was violated.
    /// </summary>
    Task<bool> TryAddAsync(CompletedSection completedSection);
}
