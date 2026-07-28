using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface ICompletedSectionRepository
{
    Task<List<CompletedSection>> GetByAttemptIdAsync(Guid attemptId);
    Task AddAsync(CompletedSection completedSection);
    Task<bool> TryAddAsync(CompletedSection completedSection);
}
